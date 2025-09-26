using System;
using System.Collections.Generic;
using System.Linq;

namespace LabXand.Infrastructure.Data
{
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Get IQueryable
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="includeNavigationProperties">navigation properties must be load</param>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> EntitySet<TEntity>(List<string> includeNavigationProperties) where TEntity : class;

        string GetTableName<T>() where T : class;

        TEntity Add<TEntity>(TEntity entity) where TEntity : class;

        TEntity Edit<TEntity>(TEntity entity, List<string> navigationPropertiesMustbeUpdate, List<string> constantFields) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : class, new();

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        long ExecuteSqlCommand(string sqlCommand, string taleName);        
    }
}
