using ServicesContracts;
using Models;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        public CityWeather? GetWeatherByCityCode(string cityCode)
        {
            List<CityWeather> cityWeatherList = new List<CityWeather>()
            {
                new CityWeather() {CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33},
                new CityWeather() {CityUniqueCode = "NYC", CityName = "New York City", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60},
                new CityWeather() {CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82}

            };

            CityWeather? city = cityWeatherList.Where(cityWeather => cityWeather.CityUniqueCode == cityCode).FirstOrDefault();

            return city;
        }

        public List<CityWeather> GetWeatherDetails()
        {
            List<CityWeather> cityWeatherList = new List<CityWeather>()
            {
                new CityWeather() {CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33},
                new CityWeather() {CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60},
                new CityWeather() {CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82}

            };

            return cityWeatherList;
        }
    }
}
