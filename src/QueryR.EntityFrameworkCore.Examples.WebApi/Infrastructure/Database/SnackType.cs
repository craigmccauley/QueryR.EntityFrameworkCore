namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class SnackType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Snack> Snacks { get; set; }
    }
}