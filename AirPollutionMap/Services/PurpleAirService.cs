using AirPollutionMap.Models;
using System.Text.Json;

namespace AirPollutionMap.Services
{
    public class PurpleAirService
    {
        private readonly HttpClient _httpClient;
        private const string apiUrl = "https://api.purpleair.com/v1/sensors";

        // Inject HttpClient
        public PurpleAirService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SensorData>> GetSensorDataAsync()
        {
            var sensorList = new List<SensorData>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "9F50E217-2447-11F0-81BE-42010A80001F");

                var response = await client.GetAsync("https://api.purpleair.com/v1/sensors?fields=name,latitude,longitude,pm2.5_atm");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseContent);

                foreach (var sensor in jsonDoc.RootElement.GetProperty("data").EnumerateArray())
                {
                    try
                    {
                        if (sensor[1].ValueKind != JsonValueKind.Null && sensor[2].ValueKind != JsonValueKind.Null)
                        {
                            double pm2_5 = 0;

                            if (sensor.GetArrayLength() > 3)
                            {
                                var pm25Element = sensor[3];

                                if (pm25Element.ValueKind == JsonValueKind.Number)
                                {
                                    pm2_5 = pm25Element.GetDouble();
                                }
                                else if (pm25Element.ValueKind == JsonValueKind.String)
                                {
                                    if (double.TryParse(pm25Element.GetString(), out var parsedValue))
                                    {
                                        pm2_5 = parsedValue;
                                    }
                                }
                            }

                            string name = "";
                            if (sensor.GetArrayLength() > 4 && sensor[4].ValueKind == JsonValueKind.String)
                            {
                                name = sensor[4].GetString();
                            }

                            // Print all sensor names here
                            Console.WriteLine($"Found Sensor Name: {name}");

                            sensorList.Add(new SensorData
                            {
                                Id = sensor[0].GetInt32(),
                                Latitude = sensor[1].GetDouble(),
                                Longitude = sensor[2].GetDouble(),
                                PM2_5 = pm2_5,
                                Name = name
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing sensor: {ex.Message}");
                    }
                }
            }

            // 💥 Don't filter anything here for now!

            return sensorList;
        }


    }
}
