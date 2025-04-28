using AirPollutionMap.Models;
using AirPollutionMap.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AirPollutionMap.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PurpleAirService _purpleAirService;

        public List<SensorData> Sensors { get; set; } = new();

        public IndexModel(PurpleAirService purpleAirService)
        {
            _purpleAirService = purpleAirService;
        }

        public async Task OnGetAsync()
        {
            Sensors = await _purpleAirService.GetSensorDataAsync();

            foreach (var sensor in Sensors)
            {
                Console.WriteLine($"Fetched Sensor: {sensor.Name} at ({sensor.Latitude}, {sensor.Longitude}) with PM2.5: {sensor.PM2_5}");
            }
        }
    }
}
