namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class KerbalPlanetaryBody
    {
        public int Id { get; set; }
        public int KerbalId { get; set; }
        public Kerbal Kerbal { get; set; }
        public int PlanetaryBodyId { get; set; }
        public PlanetaryBody PlanetaryBody { get; set; }
    }
}
