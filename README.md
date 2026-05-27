#  WeatherApp — Avalonia UI

Кроссплатформенное приложение для просмотра погоды, написанное на **C# / .NET 9** с использованием фреймворка **Avalonia UI**.  
Данные о погоде получаются в реальном времени через **OpenWeatherMap API**.

## Возможности

-  Отображение текущей температуры, ощущаемой температуры и описания погоды
-  Иконки погодных условий (ясно, облачно, дождь и т.д.)
-  Асинхронные запросы к API — интерфейс не блокируется
-  Безопасная обработка ошибок без падений приложения

## Поддерживаемые платформы

| Платформа | Статус |
|-----------|--------|
| Windows   | ✅ |
| macOS     | ✅ |
| Linux     | ✅ |
| Android   | ✅ |
| iOS       | ✅ |
| Browser (WASM) | ✅ |

##  Технологический стек

| Технология | Назначение |
|-----------|------------|
| .NET 9 | Платформа |
| Avalonia UI | UI-фреймворк (XAML) |
| CommunityToolkit.Mvvm | MVVM: Source Generators, RelayCommand, ObservableProperty |
| HttpClient + System.Text.Json | Асинхронные HTTP-запросы, десериализация JSON |
| OpenWeatherMap API | Источник данных о погоде |

## Архитектура

Проект разделён на два модуля:

```
WeatherApp-Avalonia/
├── WeatherApp11/           # Основной проект (UI-слой)
│   ├── Views/              # AXAML-разметка экранов
│   ├── ViewModels/         # Бизнес-логика, команды, состояние
│   └── Assets/             # Иконки погоды, конфигурация
│
├── Weather.Core/           # Библиотека классов (доменный слой)
│   ├── DTO/                # WeatherResponse, TempResponse, WeatherCondition
│   └── WeatherResult       # Обёртка для безопасной обработки ошибок
│
├── WeatherApp11.Desktop/   # Точка входа: Desktop
├── WeatherApp11.Android/   # Точка входа: Android
├── WeatherApp11.iOS/       # Точка входа: iOS
└── WeatherApp11.Browser/   # Точка входа: WebAssembly
```

Слабая связанность между слоями: `Weather.Core` не зависит от UI и может быть переиспользован в любом другом проекте.

##  Быстрый старт

### Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- API-ключ от [OpenWeatherMap](https://openweathermap.org/api) (бесплатный тариф подходит)

### Установка

```bash
git clone https://github.com/titsinditsin/WeatherApp-Avalonia.git
cd WeatherApp-Avalonia
```

### Настройка API-ключа

1. Перейди в папку `WeatherApp11/Assets/APIKEY/`
2. Скопируй шаблон:
   ```bash
   cp WeathAPI.json.example WeathAPI.json
   ```
3. Вставь свой ключ в `WeathAPI.json`:
   ```json
   {
     "OpenWeatherApiKey": "ВАШ_КЛЮЧ"
   }
   ```

### Запуск

```bash
dotnet run --project WeatherApp11.Desktop
```

## Лицензия

MIT

