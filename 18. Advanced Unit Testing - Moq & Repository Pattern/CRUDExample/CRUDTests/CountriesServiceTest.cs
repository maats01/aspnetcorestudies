using System;
using System.Collections.Generic;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using AutoFixture.Kernel;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;
        private readonly ICountriesRepository _countriesRepository;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

        public CountriesServiceTest()
        {
            _fixture = new Fixture();
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            _countriesService = new CountriesService(_countriesRepository);
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry_ToBeArgumentNullException()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Act
            Func<Task> action = async () =>
            {
                await _countriesService.AddCountry(request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
            _countriesRepositoryMock.Verify(temp => temp.AddCountry(It.IsAny<Country>()), Times.Never);
        }

        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _countriesService.AddCountry(request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
            _countriesRepositoryMock.Verify(temp => temp.AddCountry(It.IsAny<Country>()), Times.Never);
            _countriesRepositoryMock.Verify(temp => temp.GetCountryByCountryName(It.IsAny<string>()), Times.Never);
        }

        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryDuplicate_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest request1 = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "USA")
                .Create();
            CountryAddRequest request2 = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "USA")
                .Create();

            Country country = request1.ToCountry();

            //Act
            Func<Task> action = async () =>
            {
                await _countriesService.AddCountry(request1);

                _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(country.CountryName!))
                .ReturnsAsync(country);

                await _countriesService.AddCountry(request2);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
            _countriesRepositoryMock.Verify(temp => temp.AddCountry(It.IsAny<Country>()), Times.Once);
        }

        //When you supply propper CountryName, it should insert in the list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "USA")
                .Create();

            Country country = request.ToCountry();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(request.CountryName!))
                .ReturnsAsync(null as Country);

            _countriesRepositoryMock
                .Setup(temp => temp.AddCountry(country))
                .ReturnsAsync(country);

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);

            //Assert
            response.CountryID.Should().NotBe(Guid.Empty);
            response.CountryID.Should().NotBe(country.CountryID);
            _countriesRepositoryMock.Verify(temp => temp.GetCountryByCountryName(country.CountryName!), Times.Once);
            _countriesRepositoryMock.Verify(temp => temp.AddCountry(It.IsAny<Country>()), Times.Once);
        }

        #endregion

        #region GetAllCountries

        //The list of countries should be empty by default
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            List<Country> countries = new List<Country>();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountries())
                .ReturnsAsync(countries);

            List<CountryResponse> countriesResponse = await _countriesService.GetAllCountries();

            countriesResponse.Should().NotBeNull();
            countriesResponse.Should().BeEmpty();
            _countriesRepositoryMock.Verify(temp => temp.GetCountries(), Times.Once);
        }

        [Fact]
        public async Task GetAllCountries_ToBeSuccessful()
        {
            List<Country> countries = new List<Country>()
            {
                _fixture.Build<Country>()
                .With(temp => temp.Persons, null as ICollection<Person>)
                .Create(),
                _fixture.Build<Country>()
                .With(temp => temp.Persons, null as ICollection<Person>)
                .Create(),
            };

            List<CountryResponse> expectedCountries = countries.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountries())
                .ReturnsAsync(countries);

            List<CountryResponse> actualCountries = await _countriesService.GetAllCountries();

            actualCountries.Should().BeEquivalentTo(expectedCountries);
            _countriesRepositoryMock.Verify(temp => temp.GetCountries(), Times.Once);
        }

        #endregion

        #region GetCountryByCountryId
        //If we supply null as CountryId, it should return null as CountryResponse
        [Fact]
        public async Task GetCountryByCountryId_NullCountryId_ToBeNull()
        {
            Guid? countryId = null;

            CountryResponse? response = await _countriesService.GetCountryByCountryId(countryId);

            response.Should().BeNull();
        }

        //If we supply a valid CountryId, it should return the matching country details as a CountryResponse object
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId_ToBeSuccessful()
        {
            Country country = _fixture.Build<Country>()
                .With(temp => temp.Persons, null as ICollection<Person>)
                .Create();

            CountryResponse countryExpected = country.ToCountryResponse();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryId(country.CountryID))
                .ReturnsAsync(country);

            CountryResponse? actualCountry = await _countriesService.GetCountryByCountryId(country.CountryID);

            actualCountry.Should().NotBeNull();
            actualCountry.Should().Be(countryExpected);
            _countriesRepositoryMock.Verify(temp => temp.GetCountryByCountryId(country.CountryID), Times.Once);
        }
        #endregion
    }
}
