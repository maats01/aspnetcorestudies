using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();

            if (initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country() { CountryID = Guid.Parse("0364C2F1-44C7-486D-AE9C-3F1E4B3CDAC9"), CountryName= "USA" },
                    new Country() { CountryID = Guid.Parse("63617046-FDB4-442D-A00F-80850B60FFB5"), CountryName= "UK" },
                    new Country() { CountryID = Guid.Parse("A88EA912-1FED-4C96-9F05-E9FB8A4B9546"), CountryName= "Canada" },
                    new Country() { CountryID = Guid.Parse("521B318F-2E8E-484E-B052-0BBD20A902DE"), CountryName= "India" },
                    new Country() { CountryID = Guid.Parse("2BFF6716-7B91-4476-AB55-7DE717B5562C"), CountryName= "Australia" },
                });
            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest is null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName is null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (_countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? result = _countries.FirstOrDefault(c => c.CountryID == countryId);

            if (result == null)
            {
                return null;
            }

            return result.ToCountryResponse();
        }
    }
}
