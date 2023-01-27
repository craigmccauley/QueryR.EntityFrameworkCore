using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.QueryModels;
using QueryR.Services;

namespace QueryR.EntityFrameworkCore.Services
{
    internal class MaxDepthService : IMaxDepthService
    {
        public int? GetMaxDepth(Query query)
        {
            if (query is EfQuery efQuery)
            {
                return efQuery.Includes != null && efQuery.Includes.Any() ?
                efQuery.Includes.Select(inc => inc.NavigationPropertyPath?.Split('.').Length).Max()
                : 0;
            }
            return null;
        }
    }
}
