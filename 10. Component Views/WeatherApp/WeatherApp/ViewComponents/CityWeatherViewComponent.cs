using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.ViewComponents
{
    public class CityWeatherViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CityWeather model)
        {
            ViewBag.CityCssClass = GetCssClassByFahrenheit(model.TemperatureFahrenheit);

            return View(model);
        }

        private string GetCssClassByFahrenheit(int fahrenheit)
        {
            return fahrenheit switch
            {
                (< 44) => "blue-back",
                (>= 44) and (< 75) => "green-back",
                (>= 75) => "orange-back"
            };
        }
    }
}
