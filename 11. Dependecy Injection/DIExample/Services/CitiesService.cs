using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;
        private Guid _serviceInstanceGuid { get; }

        public Guid ServiceInstanceId
        {
            get
            {
                return _serviceInstanceGuid;
            }
        }

        public CitiesService() 
        { 
            _serviceInstanceGuid = Guid.NewGuid();
            _cities = new List<string>()
            {
                "London", "Paris", "New York", "Tokyo", "Rome"
            };
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
