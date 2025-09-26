
using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public static class UpdateConfigarationExtender
    {
        public static IPropertyUpdater<TRoot> CreateUpdater<TRoot>(this IUpdateConfiguration<TRoot> config)
            where TRoot : class
        {
            return config.CreateUpdater(config.PropertyUpdaterCustomizer);
        }

        #region Create Collection
        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            UpdateCollectionConfiguration<TRoot, T, I> config = new UpdateCollectionConfiguration<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector,
                ParentPropertyName = parentPropertyName,
                ChildtPropertyName = childtPropertyName,
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

        #endregion

        #region Create ManyToMany
        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
                    Expression<Func<TRoot, ICollection<T>>> itemSelector)
                    where TRoot : class
                    where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
                    Expression<Func<TRoot, ICollection<T>>> itemSelector, List<string> constantFields)
                    where TRoot : class
                    where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            UpdateManyToManyCollection<TRoot, T, I> config = new UpdateManyToManyCollection<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

        #endregion

        #region Create One
        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            UpdateOneEntityConfiguration<TRoot, T, I> config = new UpdateOneEntityConfiguration<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }
        #endregion

        public static IUpdateConfiguration<TRoot> HasCollection<TRoot, T, I>(this IUpdateConfiguration<TRoot> config,
            IUpdateConfiguration<TRoot> innerConfiguration)
            where TRoot : class
            where T : class, IDomainEntity<I>
        {
            ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }
        public static IUpdateConfiguration<TRoot> HasOne<TRoot, T, I>(this IUpdateConfiguration<TRoot> config,
    IUpdateConfiguration<TRoot> innerConfiguration)
    where TRoot : class
    where T : class, IDomainEntity<I>
        {
            ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

    }
}
