using QueryR.QueryModels;

namespace QueryR.EntityFrameworkCore.QueryModels
{
    public class EfQuery : Query
    {
        public List<Include> Includes { get; set; } = new List<Include>();
    }
}
