using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using LabXand.Core;
using System.Data.Entity;
using System.Data.Entity.Core;

namespace LabXand.Infrastructure.Data.EF
{
    public static class ObjectContextHelper
    {
        public static void AttachUpdated<T>(this ObjectContext context, T objectDetached, List<string> navigationProperties) where T : EntityObject, new()
        {

            if (objectDetached.EntityState == EntityState.Detached)
            {

                object currentEntityInDb = new object();

                if (context.TryGetObjectByKey(objectDetached.EntityKey, out currentEntityInDb))
                {
                    T oldEntity = (T)currentEntityInDb;
                    //MZ.Framework.Helper.ObjectContextHelper.SafeUpdate<T>(objectDetached, ref oldEntity);
                    context.ApplyCurrentValues<T>(objectDetached.EntityKey.EntitySetName, objectDetached);
                    ApplyCollectionRefrenceChanges<T>(context, objectDetached, (T)currentEntityInDb, navigationProperties);
                }
                else
                {
                    throw new ObjectNotFoundException();
                }
            }

        }

        private static object GetRelatedEndPropertyValue(RelatedEnd inputObject, string propertyName)
        {
            object result = null;

            if (inputObject != null)
            {
                PropertyInfo property = null;

                property = inputObject.GetType().GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (property != null)
                {
                    result = property.GetValue(inputObject, null);
                }
            }

            return result;
        }

        public static void ApplyCollectionRefrenceChanges<T>(ObjectContext context, T newEntity, T oldEntity, List<string> navigationProperties) where T : EntityObject, new()
        {
            Type entityType = typeof(T);

            var edmEntityTypeAttribute = typeof(T).GetCustomAttributes(typeof(EdmEntityTypeAttribute), true).Cast<EdmEntityTypeAttribute>().FirstOrDefault();

            string identity = string.Format("{0}.{1}", edmEntityTypeAttribute.NamespaceName, edmEntityTypeAttribute.Name);

            var entityNavigationProperties = context.MetadataWorkspace.GetItem<EntityType>(identity, DataSpace.CSpace).NavigationProperties;
            string EntitySetName = string.Format("{0}.{1}", context.DefaultContainerName, context.CreateObjectSet<T>().EntitySet.Name);


            foreach (var item in navigationProperties)
            {
                PropertyInfo property = entityType.GetProperty(item);
                if (property != null)
                {
                    var navigationProperty = entityNavigationProperties.SingleOrDefault(p => p.Name == item);
                    Type relatedEntityType;
                    if (navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                    {
                        relatedEntityType = property.PropertyType.GetGenericArguments().First();
                    }
                    else
                        relatedEntityType = property.PropertyType;

                    string relatedEntitySetName = TypeHelper.InvokeGenericMethod(typeof(ObjectContextHelper), "GetEntitySetName", null, new object[] { context }, relatedEntityType) as string;

                    if (property != null && navigationProperty != null)
                    {
                        if (navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                        {
                            if (navigationProperty.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many && navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                                TypeHelper.InvokeGenericMethod(typeof(ObjectContextHelper), "UpdateManyToManyRelated", null,
                                        new object[] { context, relatedEntitySetName, navigationProperty.RelationshipType.Name, navigationProperty.ToEndMember.Name, newEntity, oldEntity, property }, typeof(T), relatedEntityType);
                            else
                                TypeHelper.InvokeGenericMethod(typeof(ObjectContextHelper), "UpdateRelatedCollection", null,
                                    new object[] { context, EntitySetName, relatedEntitySetName, navigationProperty.RelationshipType.Name, navigationProperty.ToEndMember.Name, newEntity, oldEntity, property, navigationProperty }, typeof(T), relatedEntityType);
                        }
                        else
                        {
                            TypeHelper.InvokeGenericMethod(typeof(ObjectContextHelper), "UpdateRelatedEntity", null,
                                    new object[] { context, EntitySetName, relatedEntitySetName, navigationProperty.RelationshipType.Name, navigationProperty.ToEndMember.Name, newEntity, oldEntity, property, navigationProperty }, typeof(T), relatedEntityType);
                        }
                    }
                }
                else
                    throw new Exception(string.Format("{0} does not contains {1} property.", entityType.Name, item));
            }

        }

        public static string GetEntitySetName<T>(ObjectContext context) where T : EntityObject, new()
        {
            return string.Format("{0}.{1}", context.DefaultContainerName, context.CreateObjectSet<T>().EntitySet.Name);
        }

        public static void UpdateManyToManyRelated<T, N>(ObjectContext context, string relatedEntitySetName, string relationShipName, string targetRoleName,
                T newEntity, T oldEntity, PropertyInfo UpdateNavigationProperty)
            where T : EntityObject, new()
            where N : EntityObject, new()
        {
            ((IEntityWithRelationships)oldEntity).RelationshipManager.GetRelatedEnd(relationShipName, targetRoleName).Load();

            EntityCollection<N> originalValues = UpdateNavigationProperty.GetValue(oldEntity, null) as EntityCollection<N>;
            EntityCollection<N> currentValues = UpdateNavigationProperty.GetValue(newEntity, null) as EntityCollection<N>;

            List<N> tempCurrentValues = PrepareEntityKeyForEntityCollection<N>(context, currentValues);
            List<N> tempOriginValues = originalValues.ToList();

            Type relatedEntityType = typeof(N);
            Type sourceEntityType = typeof(T);

            foreach (var item in tempOriginValues)
            {
                N temp = null;
                if (currentValues.Count(t => t.EntityKey == item.EntityKey) > 0)
                    temp = currentValues.Single(t => t.EntityKey == item.EntityKey);

                if (temp == null)
                    originalValues.Remove(item);
                else
                    tempCurrentValues.Remove(temp);
            }

            foreach (var item in tempCurrentValues)
            {
                N newRelatedEntity = item;
                if ((item.EntityKey != null && Convert.ToInt32(item.EntityKey.EntityKeyValues.First().Value) > 0))
                {
                    //context.AddObject(relatedEntitySetName, item);
                    newRelatedEntity = (N)context.GetObjectByKey(item.EntityKey);
                }
                else
                {
                    //context.AddObject(relatedEntitySetName, ReflectionHelper.GetShallowEntityClone(item));
                    newRelatedEntity = (N)GetShallowEntityClone(item);
                }
                if (newRelatedEntity != null)
                    originalValues.Add(newRelatedEntity);
                else
                    throw new Exception("Related entity does not exist.");
            }
        }

        public static void UpdateRelatedCollection<T, N>(ObjectContext context, string sourceEntitySetName, string relatedEntitySetName, string relationShipName, string targetRoleName,
            T newEntity, T oldEntity, PropertyInfo UpdateNavigationProperty, NavigationProperty navigationProperty)
            where T : EntityObject, new()
            where N : EntityObject, new()
        {
            ((IEntityWithRelationships)oldEntity).RelationshipManager.GetRelatedEnd(relationShipName, targetRoleName).Load();
            EntityCollection<N> originalValues = UpdateNavigationProperty.GetValue(oldEntity, null) as EntityCollection<N>;
            EntityCollection<N> currentValues = UpdateNavigationProperty.GetValue(newEntity, null) as EntityCollection<N>;

            List<N> tempCurrentValues = PrepareEntityKeyForEntityCollection<N>(context, currentValues);
            List<N> tempOriginValues = originalValues.ToList();

            Type relatedEntityType = typeof(N);
            Type sourceEntityType = typeof(T);

            foreach (var item in tempOriginValues)
            {
                N temp = currentValues.SingleOrDefault(t => t.EntityKey == item.EntityKey);
                if (temp != null)
                {
                    N oldRelatedEntity = (N)context.GetObjectByKey(temp.EntityKey);
                    //SafeUpdate<N>(temp, ref oldRelatedEntity);
                    context.ApplyCurrentValues<N>(relatedEntitySetName, oldRelatedEntity);
                    tempCurrentValues.Remove(temp);
                }
                else
                    context.DeleteObject(item);
            }

            foreach (var item in tempCurrentValues)
            {
                N newValue;

                if (item.EntityKey == null || Convert.ToInt32(item.EntityKey.EntityKeyValues[0].Value) <= 0)
                    newValue = context.CreateObject<N>();
                else
                {
                    N oldRelatedEntity = (N)context.GetObjectByKey(item.EntityKey); ;
                    //SafeUpdate<N>(item, ref oldRelatedEntity);
                    context.ApplyCurrentValues<N>(relatedEntitySetName, oldRelatedEntity);
                }

                newValue = item;

                var prop = (EntityCollection<N>)sourceEntityType.GetProperty(navigationProperty.Name).GetValue(oldEntity, null);
                if (prop == null)
                {
                    prop = new EntityCollection<N>();
                }
                prop.Add(newValue);
            }
        }

        public static void UpdateRelatedEntity<T, N>(ObjectContext context, string sourceEntitySetName, string relatedEntitySetName, string relationShipName, string targetRoleName,
            T newEntity, T oldEntity, PropertyInfo UpdateNavigationProperty, NavigationProperty navigationProperty)
            where T : EntityObject, new()
            where N : EntityObject, new()
        {
            ((IEntityWithRelationships)oldEntity).RelationshipManager.GetRelatedEnd(relationShipName, targetRoleName).Load();
            N originalValue = UpdateNavigationProperty.GetValue(oldEntity, null) as N;
            N currentValue = UpdateNavigationProperty.GetValue(newEntity, null) as N;

            Type relatedEntityType = typeof(N);
            Type sourceEntityType = typeof(T);

            if (currentValue == null)
            {
                if (originalValue != null)
                {
                    sourceEntityType.GetProperty(navigationProperty.Name).SetValue(oldEntity, null, null);
                    context.DeleteObject(originalValue);
                }
                else
                    return;
            }
            else
            {
                N tempOriginValues = originalValue;
                PrepareEntityKey<N>(context, currentValue);
                //context.ObjectStateManager.ChangeRelationshipState<T>(oldEntity,tempOriginValues,, System.Data.EntityState.Added);
                if (currentValue.EntityKey == null || Convert.ToInt32(currentValue.EntityKey.EntityKeyValues[0].Value) <= 0)
                {
                    N newValue = context.CreateObject<N>();

                    //newValue = currentValue;
                    //context.ObjectStateManager.GetRelationshipManager(oldEntity).GetRelatedReference<N>(navigationProperty.RelationshipType.FullName,navigationProperty.ToEndMember.Name).Value = newValue;
                    //context.ObjectStateManager.GetRelationshipManager(oldEntity).GetRelatedReference<N>(navigationProperty.RelationshipType.FullName, navigationProperty.ToEndMember.Name).Attach(newValue);
                    //context.AddObject(relatedEntitySetName, newValue);
                    //ParameterExpression parameter = Expression.Parameter(sourceEntityType);
                    //MemberExpression member = Expression.MakeMemberAccess(parameter, sourceEntityType.GetProperty(navigationProperty.Name));
                    //context.ObjectStateManager.ChangeRelationshipState<T>(oldEntity, newValue, Expression.Lambda<Func<T, object>>(member, parameter), System.Data.EntityState.Added);
                    sourceEntityType.GetProperty(navigationProperty.Name).SetValue(oldEntity, newValue, null);
                    var stateEntry = context.ObjectStateManager.GetObjectStateEntry(newValue.EntityKey);
                    var propertyNameList = stateEntry.CurrentValues.DataRecordInfo.FieldMetadata.Select(pn => pn.FieldType.Name);
                    foreach (var propName in propertyNameList)
                    {
                        var prop = relatedEntityType.GetProperty(propName);
                        prop.SetValue(newValue, prop.GetValue(currentValue, null), null);
                    }
                }
                else
                {
                    //SafeUpdate<N>(currentValue, ref originalValue);
                    //context.ApplyCurrentValues<N>(relatedEntitySetName, originalValue);
                    var stateEntry = context.ObjectStateManager.GetObjectStateEntry(originalValue.EntityKey);
                    var propertyNameList = stateEntry.CurrentValues.DataRecordInfo.FieldMetadata.Select(pn => pn.FieldType.Name);
                    foreach (var propName in propertyNameList)
                    {
                        if (originalValue.EntityKey.EntityKeyValues.Count(K => K.Key == propName) <= 0)
                        {
                            var prop = relatedEntityType.GetProperty(propName);
                            prop.SetValue(originalValue, prop.GetValue(currentValue, null), null);
                        }
                    }
                }
            }
        }

        public static void SafeUpdate<T>(T newEntity, ref T oldEntity, List<string> fixedValueProperties)
        {
            Type entityType = typeof(T);
            foreach (var item in fixedValueProperties)
            {
                PropertyInfo propertyInfo = entityType.GetProperty(item);
                propertyInfo.SetValue(newEntity, propertyInfo.GetValue(oldEntity, null), null);
            }

            oldEntity = newEntity;
        }

        public static void PrepareEntityKey<T>(ObjectContext context, T entity) where T : EntityObject, new()
        {
            string entitySetName = context.CreateObjectSet<T>().EntitySet.Name;
            entity.EntityKey = context.CreateEntityKey(entitySetName, entity);
        }

        public static List<T> PrepareEntityKeyForEntityCollection<T>(ObjectContext context, EntityCollection<T> entityCollection) where T : EntityObject, new()
        {
            List<T> tempList = new List<T>();
            if (entityCollection != null)
            {
                foreach (var item in entityCollection)
                {
                    T tempItem = new T();
                    tempItem = item;
                    PrepareEntityKey<T>(context, tempItem);
                    tempList.Add(tempItem);
                }
            }
            return tempList;
        }

        public static void ApplyReferencePropertyChanges(ObjectContext context, IEntityWithRelationships newEntity, IEntityWithRelationships oldEntity)
        {

            foreach (var relatedEnd in oldEntity.RelationshipManager.GetAllRelatedEnds())
            {

                var oldRef = relatedEnd as EntityReference;

                if (oldRef != null)
                {

                    // this related end is a reference not a collection

                    var newRef = newEntity.RelationshipManager.GetRelatedEnd(oldRef.RelationshipName, oldRef.TargetRoleName) as EntityReference;

                    oldRef.EntityKey = newRef.EntityKey;

                }

            }

        }

        public static Expression<Func<ObjectSet<T>, ObjectQuery<T>>> GetExpressionForInclude<T>(ObjectSet<T> objectSet, List<string> navigationProperties) where T : EntityObject
        {
            if (navigationProperties != null && navigationProperties.Count > 0)
            {
                Type type = objectSet.GetType();
                MethodInfo method = type.GetMethod("Include");

                string first = navigationProperties.First();
                var str = Expression.Constant(first);
                var expersion = Expression.Call(Expression.Constant(objectSet, typeof(ObjectSet<T>)), method, str);
                //navigationProperties.Remove(first);
                foreach (var item in navigationProperties.Skip(1))
                {
                    expersion = Expression.Call(expersion, method, Expression.Constant(item));
                }
                return Expression.Lambda<Func<ObjectSet<T>, ObjectQuery<T>>>(expersion, Expression.Parameter(typeof(ObjectSet<T>)));
            }
            return null;
        }

        public static object GetShallowEntityClone(object entity)
        {
            object clone = Activator.CreateInstance(entity.GetType());
            foreach (PropertyInfo prop in entity.GetType().GetProperties())
            {
                if (typeof(RelatedEnd).IsAssignableFrom(prop.PropertyType)) continue;
                if (typeof(IEntityWithKey).IsAssignableFrom(prop.PropertyType)) continue;
                try
                {
                    prop.SetValue(clone, prop.GetValue(entity, null), null);
                }
                catch
                {
                }
            }
            return clone;
        }
    }
}
