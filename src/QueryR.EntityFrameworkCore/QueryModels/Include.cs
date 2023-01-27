using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.QueryModels
{
    public class Include : IQueryPart
    {
        public string? NavigationPropertyPath { get; set; }
    }
}
