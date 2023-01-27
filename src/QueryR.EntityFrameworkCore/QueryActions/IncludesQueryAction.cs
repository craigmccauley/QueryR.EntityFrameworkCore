using Microsoft.EntityFrameworkCore;
using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.QueryActions
{
    internal class IncludesQueryAction : IEfQueryAction
    {
        public QueryResult<T> Execute<T>(EfQuery querySpec, QueryResult<T> queryResult) where T : class, new()
        {
            foreach (var include in querySpec.Includes ?? Enumerable.Empty<Include>())
            {
                if (!string.IsNullOrWhiteSpace(include.NavigationPropertyPath))
                {
                    queryResult.PagedQuery = queryResult.PagedQuery.Include(include.NavigationPropertyPath);
                }
            }
            return queryResult;
        }

        //TODO: This is a bit of a kludge. Not sure of a cleaner way of doing this.

        /// <summary>
        /// Use <see cref="IEfQueryAction.Execute{T}(EfQuery, QueryResult{T})" /> instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {
            throw new NotImplementedException($"Use {nameof(IEfQueryAction)}.{nameof(IEfQueryAction.Execute)} instead.");
        }
    }
}
