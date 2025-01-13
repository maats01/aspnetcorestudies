using Microsoft.AspNetCore.Mvc;
using ServicesContracts;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View(_weatherService.GetWeatherDetails());
        }

        [Route("weather/{cityUniqueCode}")]
        public IActionResult City(string? cityUniqueCode)
        {
            if (cityUniqueCode == null)
            {
                BadRequest("You must enter a city unique code to access the view");
            }

            return View(_weatherService.GetWeatherByCityCode(cityUniqueCode!));
        }
    }
}