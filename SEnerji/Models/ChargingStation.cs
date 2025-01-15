namespace SEnerji.Models
{
    public class ChargingStation
    {
        public string Title { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CountryCode { get; set; }
        public double? PowerKW { get; set; }
        public GeoLocation Location { get; set; }
    }
}
