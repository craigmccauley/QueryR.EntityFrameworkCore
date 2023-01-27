namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class PlanetaryBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<KerbalPlanetaryBody> KerbalPlanetaryBodies { get; set; }
    }
}
