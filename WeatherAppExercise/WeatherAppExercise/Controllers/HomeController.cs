using Microsoft.AspNetCore.Mvc;
using WeatherAppExercise.Models;

namespace WeatherAppExercise.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            List<CityWeather> cityWeatherList = new List<CityWeather>()
            {
                new CityWeather() {CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33},
                new CityWeather() {CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60},
                new CityWeather() {CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82}

            };

            return View(cityWeatherList);
        }

        [Route("weather/{cityUniqueCode}")]
        public IActionResult City(string? cityUniqueCode)
        {
            if (cityUniqueCode == null)
            {
                BadRequest("You must enter a city unique code to access the view");
            }

            List<CityWeather> cityWeatherList = new List<CityWeather>()
            {
                new CityWeather() {CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),  TemperatureFahrenheit = 33},
                new CityWeather() {CityUniqueCode = "NYC", CityName = "New York City", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),  TemperatureFahrenheit = 60},
                new CityWeather() {CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),  TemperatureFahrenheit = 82}

            };

            CityWeather? city = cityWeatherList.Where(cityWeather => cityWeather.CityUniqueCode == cityUniqueCode).FirstOrDefault();

            return View(city);
        }
    }
}
