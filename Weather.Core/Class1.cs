using System.Text.Json.Serialization;

namespace Weather.Core
{
    // Соответствует блоку "main" в JSON-ответе OWM
    public class TempResponse
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelTemp { get; set; }
    }

    // Соответствует элементу массива "weather" — описывает тип погоды и иконку
    public class WeatherCondition
    {
        [JsonPropertyName("main")]
        public string MainWeather { get; set; }

        // Текстовое описание на языке, заданном в запросе (lang=ru)
        [JsonPropertyName("description")]
        public string Description { get; set; }

        // Код иконки от OWM, например "01d" (ясно, день) или "10n" (дождь, ночь)
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        // Преобразует код иконки OWM в путь к локальному ресурсу Avalonia.
        // Если код неизвестен — падбэк на "03d" (облачно)
        public static string GetIconPath(string owmIcon)
        {
            var validCodes = new HashSet<string>
            {
                "01d","01n","02d","02n","03d","03n","04d","04n",
                "09d","09n","10d","10n","11d","11n","13d","13n","50d","50n"
            };

            string code = validCodes.Contains(owmIcon) ? owmIcon : "03d";
            return $"avares://WeatherApp11/Assets/WeathIcons/{code}.png";
        }
    }

    // Корневой объект JSON-ответа от OWM
    public class WeatherResponse
    {
        [JsonPropertyName("main")]
        public TempResponse MainData { get; set; }

        // Массив погодных условий — обычно содержит один элемент
        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; }
    }

    // Обёртка над результатом запроса — вместо исключений используем явный IsSuccess
    public class WeatherResult
    {
        public bool IsSuccess { get; set; }

        // Заполняется только если IsSuccess == false
        public string ErrorMessage { get; set; }

        // Заполняется только если IsSuccess == true
        public WeatherResponse Data { get; set; }
    }
}