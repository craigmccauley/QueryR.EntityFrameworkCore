namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class Kerbal
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AssignedSpaceCraftId { get; set; }
        public Craft AssignedSpaceCraft { get; set; }
        public ICollection<KerbalPlanetaryBody> PlanetaryBodiesVisited { get; set; }
        public ICollection<Snack> SnacksOnHand { get; set; }
    }
}
