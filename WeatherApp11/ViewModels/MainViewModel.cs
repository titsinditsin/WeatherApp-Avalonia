using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.Core;

namespace WeatherApp11.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        // Один HttpClient на всё приложение — так правильно, не создавать новый на каждый запрос
        private static readonly HttpClient _httpClient = new HttpClient();

        // Привязываемые свойства для UI (Source Generator генерирует TempDisplay, WeatherDisplay и т.д.)
        [ObservableProperty]
        private string _tempDisplay = string.Empty;

        [ObservableProperty]
        private string _weatherDisplay = string.Empty;

        [ObservableProperty]
        private string _feelsLikeDisplay = string.Empty;

        // Иконка погоды как Bitmap — загружается из встроенных ресурсов (avares://)
        [ObservableProperty]
        private Bitmap? _weatherIcon;

        // Команда, привязанная к кнопке "Обновить" в AXAML
        [RelayCommand]
        public async Task GetWeather()
        {
            var result = await GetWeatherAsync("Moscow");

            if (result.IsSuccess)
            {
                // Округляем температуру без дробной части
                TempDisplay = Math.Round(result.Data.MainData.Temperature).ToString() + "°C";
                WeatherDisplay = result.Data.Weather[0].Description;
                FeelsLikeDisplay = $"Ощущается как {Math.Round(result.Data.MainData.FeelTemp)}°C";

                // Получаем код иконки от OWM (например "01d") и строим путь к ресурсу
                string iconCode = result.Data.Weather[0].Icon;
                string path = WeatherCondition.GetIconPath(iconCode);

                try
                {
                    // Загружаем иконку из встроенных ресурсов Avalonia
                    using var stream = AssetLoader.Open(new Uri(path));
                    WeatherIcon = new Bitmap(stream);
                }
                catch { }
            }
            else
            {
                // При ошибке показываем текст ошибки вместо температуры
                TempDisplay = result.ErrorMessage;
            }
        }

        // Читает API-ключ из JSON-файла, встроенного в ресурсы приложения
        public string GetApiKey()
        {
            using var stream = AssetLoader.Open(
                new Uri("avares://WeatherApp11/Assets/APIKEY/WeathAPI.json"));
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)["OpenWeatherApiKey"];
        }

        // Делает запрос к OpenWeatherMap API и возвращает обёрнутый результат
        public async Task<WeatherResult> GetWeatherAsync(string city)
        {
            string apiKey = GetApiKey();

            if (string.IsNullOrEmpty(apiKey))
                return new WeatherResult { IsSuccess = false, ErrorMessage = "API ключ не найден!" };

            try
            {
                // units=metric — градусы Цельсия, lang=ru — описание на русском
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ru";
                var data = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
                return new WeatherResult { IsSuccess = true, Data = data };
            }
            catch (Exception ex)
            {
                return new WeatherResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }
}