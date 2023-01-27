namespace QueryR.EntityFrameworkCore.Examples.WebApi.Infrastructure.Database
{
    public class Snack
    {
        public int Id { get; set; }
        public int KerbalId { get; set; }
        public Kerbal Kerbal { get; set; }
        public int SnackTypeId { get; set; }
        public SnackType SnackType { get; set; }
        public int Amount { get; set; }
    }
}
