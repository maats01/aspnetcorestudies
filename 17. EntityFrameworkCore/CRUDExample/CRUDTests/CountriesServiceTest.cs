using System;
using System.Collections.Generic;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryDuplicate()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //When you supply propper CountryName, it should insert in the list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
        }

        #endregion

        #region GetAllCountries

        //The list of countries should be empty by default
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            Assert.Empty(countries); 
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> countriesRequestList = new List<CountryAddRequest>() 
            { 
                new CountryAddRequest() { CountryName = "USA" },
                new CountryAddRequest() { CountryName = "UK" }
            };

            List<CountryResponse> expectedCountries = new List<CountryResponse>();

            foreach (CountryAddRequest request in countriesRequestList)
            {
                expectedCountries.Add(await _countriesService.AddCountry(request));
            }

            List<CountryResponse> actualCountries = await _countriesService.GetAllCountries();

            foreach (CountryResponse expectedCountry in expectedCountries)
            {
                Assert.Contains(expectedCountry, actualCountries);
            }
        }

        #endregion

        #region GetCountryByCountryId
        //If we supply null as CountryId, it should return null as CountryResponse
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            Guid? countryId = null;

            CountryResponse? response = await _countriesService.GetCountryByCountryId(countryId);

            Assert.Null(response);
        }

        //If we supply a valid CountryId, it should return the matching country details as a CountryResponse object
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "China" };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            CountryResponse? actualCountry = await _countriesService.GetCountryByCountryId(countryResponse.CountryID);

            Assert.Equal(countryResponse, actualCountry);
        }
        #endregion
    }
}
