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
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using wc = Weather.Core;

namespace WeatherApp11.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _tempDisplay; // Для температуры (например, "15°C")

        [ObservableProperty]
        private string _weatherDisplay; // Для описания (например, "Облачно")

        [ObservableProperty]
        private string _prikolambus;


        [RelayCommand]
        public async Task GetWeather()
        {

            string city = "Moscow"; // Здесь можно заменить на любой другой город
            string FileName = "C:\\Users\\Danil\\source\\repos\\WeatherApp11\\WeathAPI.json";

            var result = await wc.JSONTaker.GetWeatherJSON(city, FileName);
            if (result != null)
            {

                TempDisplay = $"{result.MainData.Temperature} °C";


                WeatherDisplay = result.Weather[0].Description;

                Prikolambus = $"Город: {result.CityName},темпа:{TempDisplay}, {WeatherDisplay}";
            }
        }

        
         



    }
   }
