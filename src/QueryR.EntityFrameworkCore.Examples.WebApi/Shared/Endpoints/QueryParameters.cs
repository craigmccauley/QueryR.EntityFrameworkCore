namespace QueryR.EntityFrameworkCore.Examples.WebApi.Shared.Endpoints
{
    public class QueryParameters
    {
        public Dictionary<string, Dictionary<string, string>> Filter { get; set; } = new();
        public string Include { get; set; } = string.Empty;
        public Dictionary<string, string> Page { get; set; } = new();
        public string Sort { get; set; } = string.Empty;
        public Dictionary<string, string> Fields { get; set; } = new();
    }
}
