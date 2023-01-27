using Microsoft.EntityFrameworkCore;
using QueryR.EntityFrameworkCore;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore
{
    public static class QueryResultExtensions
    {
        public static async Task<int> CountAsync<T>(this QueryResult<T> queries, CancellationToken cancellationToken = default)
            => await queries.CountQuery.CountAsync(cancellationToken);
        public static async Task<List<T>> ToListAsync<T>(this QueryResult<T> queries, CancellationToken cancellationToken = default)
            => await queries.PagedQuery.ToListAsync(cancellationToken);
        public static async Task<(int Count, List<T> Items)> GetCountAndListAsync<T>(this QueryResult<T> queries, CancellationToken cancellationToken = default)
            => (await queries.CountAsync(cancellationToken), await queries.ToListAsync(cancellationToken));
    }
}
