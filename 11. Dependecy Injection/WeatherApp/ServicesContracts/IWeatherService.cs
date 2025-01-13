using Models;

namespace ServicesContracts
{
    public interface IWeatherService
    {
        List<CityWeather> GetWeatherDetails();
        CityWeather? GetWeatherByCityCode(string cityCode);
    }
}
