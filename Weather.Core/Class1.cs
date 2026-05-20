using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;


namespace Weather.Core
{
    

    public class WeatherApiClient
    {


        public WeatherApiClient() { }
    }


    // Класс для описания блока "main" в JSON 
    public class TempResponse
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelTemp { get; set; }

        /// <summary>
        /// минмакс температура, если нужно будет
        ///[JsonPropertyName("temp_min")]
        ///public double MinT { get; set; }
        ///
        ///[JsonPropertyName("temp_max")]
        ///public double MaxT { get; set; }
        /// </summary>
    }

    // Класс для описания погоды 
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


    // Класс для всего ответа от API
    public class WeatherResponse
    {
        // Связь с блоком температур
        [JsonPropertyName("main")]
        public TempResponse MainData { get; set; }

        // Связь со списком описаний погоды
        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; }
    }

    // Класс для результата, который мы будем возвращать из метода GetWeatherJSON
    public class WeatherResult
    {
        public bool IsSuccess { get; set; }

        // Текст ошибки (если IsSuccess == false)
        public string ErrorMessage { get; set; }

        // Сами данные (если IsSuccess == true)
        public WeatherResponse Data { get; set; }
    }
}