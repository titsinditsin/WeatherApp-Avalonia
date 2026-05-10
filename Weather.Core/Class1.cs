using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weather.Core
{
    public class JSONTaker
    {
        
        public static string GetKeyApi(string FileName)
        {
            try
            {
                string jsonString = File.ReadAllText(FileName);
                using JsonDocument doc = JsonDocument.Parse(jsonString);
                string ApiK = doc.RootElement.GetProperty("OpenWeatherApiKey").GetString();
                return ApiK;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<WeatherResponse> GetWeatherJSON(string city, string FileName)
        {

            JSONTaker JSONTaker = new JSONTaker();

            
            using var client = new HttpClient();
            string URL = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={GetKeyApi(FileName)}&units=metric";


            // Вызываем метод для парсинга
            return JSONTaker.ParseWeather(await client.GetStringAsync(URL));
        }
        public WeatherResponse ParseWeather(string json)
        {
            return JsonSerializer.Deserialize<WeatherResponse>(json);
        }
    }
    public class TempResponse
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelTemp { get; set; }


        [JsonPropertyName("temp_min")]
        public double MinT { get; set; }

        [JsonPropertyName("temp_max")]
        public double MaxT { get; set; }


    }


    public class WeatherCondition
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string MainWeather { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
    public class WeatherResponse
    {
        // Связь с блоком температур
        [JsonPropertyName("main")]
        public TempResponse MainData { get; set; }

        // Связь со списком описаний погоды
        // Используем List, потому что в JSON стоят квадратные скобки [ ]
        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; }

        [JsonPropertyName("name")]
        public string CityName { get; set; }

        
    }
}
