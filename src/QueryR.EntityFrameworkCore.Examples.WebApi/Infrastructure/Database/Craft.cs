using System.Collections.Generic;

namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class Craft
    {
        public int Id { get; set; }
        public string CraftName { get; set; }
        public ICollection<Kerbal> AssignedKerbals { get; set; } = new List<Kerbal>();
    }
}
