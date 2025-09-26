using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using LabXand.Infrastructure.Data.EF.UpdateConfiguration;
using System.Text;
using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using System.Data.Common;
using System.Data;
using System;
using System.Data.Entity.Core;
using Nessos.LinqOptimizer.Core;
using LabXand.Core;
using System.ComponentModel;

namespace LabXand.Infrastructure.Data.EF
{
    public static class DbContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static void UpdateDetachedObject<TEntity, I>(this DbContextBase context, TEntity currentEntity, TEntity originalEntity, IUpdateConfiguration<TEntity> configuration)
            where TEntity : DomainEntityBase<I>
        {
            if (configuration != null)
            {
                configuration.CreateUpdater().Update(context, currentEntity, originalEntity);
            }
        }

        public static void SafeUpdate<TEntity>(this DbContextBase context, TEntity currentValue, TEntity originalValue, List<string> constantFields)
            where TEntity : class
        {
            DbEntityEntry entry = context.Entry(originalValue);
            entry.CurrentValues.SetValues(currentValue);

            if (constantFields != null)
            {
                foreach (string field in constantFields)
                {
                    entry.Property(field).IsModified = false;
                }
            }
        }
        public static T UnProxy<T>(this DbContext context, T proxyObject) where T : class
        {
            var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;
                var entry = context.Entry(proxyObject);
                T poco = (entry.State != EntityState.Deleted ? entry.CurrentValues.ToObject() : entry.OriginalValues.ToObject()) as T;
                return poco;
            }
            finally
            {
                context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }
        public static string StringifyChangedRelationships(
        this DbContext context)
        {
            StringBuilder sb = new StringBuilder();
            var addedString = GetAddedRelationships(context).Select(e => $"'{e.Key}' به '{e.Value}' اضافه شد");
            var dicremovedString = GetDeletedRelationships(context).Select(e => $"'{e.Key}' از '{e.Value}' حذف شد");
            addedString.Each(s => sb.AppendLine(s));
            dicremovedString.Each(s => sb.AppendLine(s));
            return sb.ToString();
        }

        public static Dictionary<string, string> GetAddedRelationships(
            this DbContext context)
        {
            return GetRelationships(context, EntityState.Added, (e, i) => e.CurrentValues[i]);
        }

        public static Dictionary<string, string> GetDeletedRelationships(
            this DbContext context)
        {
            return GetRelationships(context, EntityState.Deleted, (e, i) => e.OriginalValues[i]);
        }

        private static Dictionary<string, string> GetRelationships(
            this DbContext context,
            EntityState relationshipState,
            Func<ObjectStateEntry, int, object> getValue)
        {
            context.ChangeTracker.DetectChanges();
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            var t = objectContext.ObjectStateManager
                                .GetObjectStateEntries(relationshipState)
                                .Where(e => e.IsRelationship)
                                .Select(e => new Tuple<object, object>(
                                    objectContext.GetObjectByKey((EntityKey)getValue(e, 1)),
                                    objectContext.GetObjectByKey((EntityKey)getValue(e, 0))))
                                .Select(e => new KeyValuePair<string, string>(GetNameOfEntry(e.Item1), GetNameOfEntry(e.Item2)))
                                .GroupBy(e => e.Key).ToDictionary(e => e.Key, e => e.First().Value);

            return t;
        }

        private static readonly List<string> originalEntitiesList = new List<string>
            {
                "PermissionRole"
            };
        static string GetNameOfEntry(object changeLogEntity)
        {

            var description = $" {changeLogEntity} ";
            if (changeLogEntity is IDomainEntity domainEntity)
                description = domainEntity.EntityDescriptor();
            return description;

        }
        static string GetNameOfEntry(EntityChangeLog changeLog)
        {

            var description = $" {changeLog.OriginalEntity} ";
            if (changeLog.OriginalEntity is IDomainEntity domainEntity)
                description = domainEntity.EntityDescriptor();
            return description;
        }
        static string GetTypeNeme(object changeLog)
        {
            string result = TypeHelper.GetDescription(changeLog);
            return string.IsNullOrWhiteSpace(result) ? changeLog.GetType().Name : result;
        }

        /// <summary>
        /// Returns an overview of the state of tracked entities in the context
        /// </summary>
        /// <param name="context">The EF dbContext</param>
        /// <returns>string representation of tracked entities</returns>
        public static string StringifyDbContextChanges(this DbContext context)

        {
            var trackedEntities = context
                       .ChangeTracker
                       .Entries()
                       .Where(t => t.State != EntityState.Detached && t.Entity != null)
                       .Select(t => new EntityChangeLog()
                       {
                           Entity = context.UnProxy(t.Entity),
                           OriginalEntity = t.Entity,
                           State = t.State,
                           Original = t.State != EntityState.Added
                                   ? t.OriginalValues.PropertyNames.ToDictionary(pn => pn, pn => t.OriginalValues[pn])
                                   : new Dictionary<string, object>(),
                           Current = t.State != EntityState.Deleted
                                   ? t.CurrentValues.PropertyNames.ToDictionary(pn => pn, pn => t.CurrentValues[pn])
                                   : new Dictionary<string, object>(),
                       })
                       .OrderBy(e => e.State).ThenBy(e => e.EntityType.Name)
                       .ToList();

            var builder = new StringBuilder();

            foreach (var entity in trackedEntities)
            {
                if (entity.State == EntityState.Unchanged)
                    continue;
                if (!originalEntitiesList.Contains(entity.EntityType.Name))
                    builder.AppendLine($" {GetNameOfEntry(entity.Entity)} {entity.State} <--> ").AppendLine();
                else
                    builder.AppendLine($" {GetNameOfEntry(entity)} {entity.State} <--> ").AppendLine();


                if (entity.State == EntityState.Modified)
                {
                    var changeList = DetectChangeDetails(entity);
                    if (changeList.Count > 0)
                    {
                        builder.AppendLine("Details of changes: ");
                        changeList.Each(s => builder.AppendLine(s).AppendLine());
                        builder.AppendLine();
                    }
                }

                List<string> DetectChangeDetails(EntityChangeLog changeLog)
                {
                    List<string> changeDetails = new List<string>();
                    bool outerIsOriginal = changeLog.Original.Count >= changeLog.Current.Count;
                    Dictionary<string, object> outer = outerIsOriginal ? changeLog.Original : changeLog.Current;
                    Dictionary<string, object> inner = outerIsOriginal ? changeLog.Current : changeLog.Original;

                    var propertyValues = from fd in outer
                                         join pd in inner on fd.Key equals pd.Key into joinedT
                                         from pd in joinedT.DefaultIfEmpty()
                                         select new
                                         {
                                             fd.Key,
                                             OriginalValue = outerIsOriginal ? fd.Value : pd.Value,
                                             CurrentValue = outerIsOriginal ? pd.Value : fd.Value
                                         };
                    foreach (var propertyValue in propertyValues)
                    {
                        if (propertyValue.OriginalValue == null && propertyValue.CurrentValue == null)
                            continue;
                        var isSimpleType = propertyValue.OriginalValue != null ? IsSimple(propertyValue.OriginalValue.GetType()) : IsSimple(propertyValue.CurrentValue.GetType());
                        if (!isSimpleType)
                            continue;
                        string currentAsString = propertyValue.CurrentValue != null ? propertyValue.CurrentValue.ToString() : string.Empty;
                        string originalAsString = propertyValue.OriginalValue != null ? propertyValue.OriginalValue.ToString() : string.Empty;
                        if (!originalAsString.Equals(currentAsString, StringComparison.OrdinalIgnoreCase))
                            changeDetails.Add($"{propertyValue.Key}: changed from '{propertyValue.OriginalValue}' to '{propertyValue.CurrentValue}' <---> ");
                    }
                    return changeDetails;


                    bool IsSimple(Type type)
                    {
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            return IsSimple(type.GetGenericArguments()[0]);
                        return type.IsPrimitive
                          || type.IsEnum
                          || type.Equals(typeof(string))
                          || type.Equals(typeof(DateTime))
                          || type.Equals(typeof(decimal));
                    }
                }
            }
            builder.AppendLine(context.StringifyChangedRelationships());

            return builder.ToString();
        }

        public static void ApplyCorrectYeKe(this DbCommand command)
        {
            command.CommandText = command.CommandText.ApplyCorrectYeKe();

            foreach (DbParameter parameter in command.Parameters)
            {
                switch (parameter.DbType)
                {
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                        parameter.Value = parameter.Value is DBNull ? parameter.Value : parameter.Value.ApplyCorrectYeKe();
                        break;
                }
            }
        }
        public static void ApplyHtmlSanitize(this DbCommand command)
        {
            foreach (DbParameter parameter in command.Parameters)
            {
                switch (parameter.DbType)
                {
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                        parameter.Value = parameter.Value is DBNull ? parameter.Value : parameter.Value.ApplyHtmlSanitize();
                        break;
                }
            }
        }
        public static void CleanData(this DbCommand command)
        {
            command.ApplyCorrectYeKe();
            command.ApplyHtmlSanitize();
        }
        class EntityChangeLog
        {
            public object Entity { get; set; }
            public object OriginalEntity { get; set; }
            public EntityState State { get; set; }
            public Type EntityType => Entity.GetType();
            public Dictionary<string, object> Current { get; set; }
            public Dictionary<string, object> Original { get; set; }
        }
    }
}

