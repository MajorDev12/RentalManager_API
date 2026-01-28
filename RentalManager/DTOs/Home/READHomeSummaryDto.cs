namespace RentalManager.DTOs.Home
{
    public class READHomeSummaryDto
    {
        public int totalProperties { get; set; }
        public int totalHouses { get; set; }
        public int totalActiveTenants { get; set; }
        public int totalVacants { get; set; }
        public int totalLandlords { get; set; }
    }
}
