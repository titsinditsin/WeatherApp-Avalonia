using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Metadata;
using Avalonia.Platform;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.Core;
using wc = Weather.Core;

namespace WeatherApp11.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        [ObservableProperty]
        private string _tempDisplay; // Для температуры 

        [ObservableProperty]
        private string _weatherDisplay; // Для описания 

        [RelayCommand]
        public async Task GetWeather()
        {
            var result = await GetWeatherAsync("Moscow");

            if (result.IsSuccess)
            {
                TempDisplay = result.Data.MainData.Temperature.ToString();
                WeatherDisplay = result.Data.Weather[0].Description;
            }
            else
            {
                TempDisplay = result.ErrorMessage;
            }
        }


        public string GetApiKey()
        {
            using var stream = AssetLoader.Open(
            new Uri("avares://WeatherApp11/Assets/APIKEY/WeathAPI.json"));

            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd();

            string ApiK = JsonSerializer.Deserialize<Dictionary<string, string>>(json)["OpenWeatherApiKey"];
            return ApiK;
        }


        public async Task<WeatherResult> GetWeatherAsync(string city)
        {
            HttpClient client = new HttpClient();
            string apiKey = GetApiKey();

            if (string.IsNullOrEmpty(apiKey))
            {
                return new WeatherResult { IsSuccess = false, ErrorMessage = "API ключ не найден!" };
            }

            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=ru";

                var data = await client.GetFromJsonAsync<WeatherResponse>(url);

                return new WeatherResult { IsSuccess = true, Data = data };
            }
            catch (Exception ex)
            {
                return new WeatherResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

    }
}