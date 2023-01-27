using QueryR.EntityFrameworkCore.QueryActions;
using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.EntityFrameworkCore.Services;
using QueryR.QueryActions;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore
{
    public static class IQueryableExtensions
    {

        private static List<IQueryAction>? EfQueryActions { get; set; } = null;

        private static void InitializeQueryActions()
        {
            EfQueryActions ??= new List<IQueryAction>
            {
                new FilterQueryAction(),
                new IncludesQueryAction(),
                new SortQueryAction(),
                new PagingQueryAction(),
                new SparseFieldsQueryAction(new MaxDepthService()),
            };
        }

        /// <summary>
        /// Performs the EfQuery on the source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryResult<T> Query<T>(this IQueryable<T> source, EfQuery query) where T : class, new()
        {
            var result = new QueryResult<T>
            {
                CountQuery = source,
                PagedQuery = source
            };

            InitializeQueryActions();

            foreach (var action in EfQueryActions!)
            {
                if (action is IEfQueryAction efAction)
                {
                    efAction.Execute(query, result);
                }
                else
                {
                    action.Execute(query, result);
                }
            }

            return result;
        }
    }
}
