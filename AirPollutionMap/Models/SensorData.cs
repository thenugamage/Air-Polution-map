namespace AirPollutionMap.Models
{
    public class SensorData
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double PM2_5 { get; set; }
        public string Name { get; set; }
    }
}
