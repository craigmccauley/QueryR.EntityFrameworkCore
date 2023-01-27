using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.QueryActions;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.QueryActions
{
    internal interface IEfQueryAction : IQueryAction
    {
        QueryResult<T> Execute<T>(EfQuery query, QueryResult<T> queryResult) where T : class, new();
    }
}
