using QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Constants;
using QueryR.EntityFrameworkCore.QueryModels;
using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints
{
    public interface IQueryParametersMapper
    {
        EfQuery ToQuery(QueryParameters queryParameters);
    }

    public class QueryParametersMapper : IQueryParametersMapper
    {
        public EfQuery ToQuery(QueryParameters queryParameters) => new EfQuery
        {
            Filters = queryParameters.Filter.SelectMany(filter => filter.Value.Select(@operator => new Filter
            {
                PropertyName = filter.Key,
                Operator = FilterOperators.ToItem[@operator.Key],
                Value = @operator.Value
            })).ToList(),

            Includes = queryParameters.Include.Split(QueryStringParts.Comma, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(item => new Include
            {
                NavigationPropertyPath = item,
            }).ToList(),

            PagingOptions = new PagingOptions
            {
                PageNumber = GetIntFromDictionary(queryParameters.Page, QueryStringParts.PageNumber, 1, null),
                PageSize = GetIntFromDictionary(queryParameters.Page, QueryStringParts.PageSize, 20, 50),
            },

            Sorts = queryParameters.Sort.Split(QueryStringParts.Comma, StringSplitOptions.RemoveEmptyEntries).Select(item => new Sort
            {
                PropertyName = item.StartsWith(QueryStringParts.Hyphen) ? item[QueryStringParts.Hyphen.Length..] : item,
                IsAscending = item.StartsWith(QueryStringParts.Hyphen),
            }).ToList(),

            SparseFields = queryParameters.Fields.Select(kvp => new SparseField
            {
                EntityName = kvp.Key,
                PropertyNames = kvp.Value.Split(QueryStringParts.Comma, StringSplitOptions.RemoveEmptyEntries).ToList(),
            }).ToList()
        };


        private static int GetIntFromDictionary(Dictionary<string, string> dictionary, string key, int defaultValue, int? maxValue)
        {
            if (dictionary.ContainsKey(key)
                && int.TryParse(dictionary[key], out var pageNum))
            {
                if (maxValue.HasValue && maxValue.Value < pageNum)
                {
                    return maxValue.Value;
                }
                return pageNum;
            }
            return defaultValue;
        }
    }
}
