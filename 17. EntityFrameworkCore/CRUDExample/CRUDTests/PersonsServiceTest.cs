using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
            _personService = new PersonsService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
            _testOutputHelper = testOutputHelper;
        }

        private async Task<List<PersonResponse>> SamplePersonsForTest()
        {
            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest request2 = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(request1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(request2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "person@sample.com",
                Address = "address 1",
                CountryId = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "person@sample.com",
                Address = "address 2",
                CountryId = countryResponse2.CountryID,
                DateOfBirth = DateTime.Parse("1980-01-01"),
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "person@sample.com",
                Address = "address 3",
                CountryId = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("2005-01-01"),
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Female
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2, personAddRequest3
            };
            List<PersonResponse> personResponses = new List<PersonResponse>();

            foreach (PersonAddRequest request in personRequests)
            {
                personResponses.Add(await _personService.AddPerson(request));
            }

            return personResponses;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException 
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.AddPerson(request);
            });
        }

        //When we supply null vaule as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            PersonAddRequest request = new PersonAddRequest() { PersonName = null };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.AddPerson(request);
            });
        }

        //When we supply proper person details, it should insert the person into the persons list;
        //it should also return a new object of PersonResponse which contains the newly generated PersonId
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "person@email.com",
                Address = "sample address",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse = await _personService.AddPerson(request);
            List<PersonResponse> personsList = await _personService.GetAllPersons();

            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, personsList);
        }
        #endregion

        #region GetAllPersons

        //The persons list must be empty by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            List<PersonResponse> persons = await _personService.GetAllPersons();

            Assert.Empty(persons);
        }

        //First, we will add a few persons; then, when calling GetAllPersons,
        //it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            List<PersonResponse> personResponses = await SamplePersonsForTest();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> getAllPersonsResult = await _personService.GetAllPersons();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getAllPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            foreach (PersonResponse expected in personResponses)
            {
                Assert.Contains(expected, getAllPersonsResult);
            }
        }

        #endregion

        #region GetPersonByPersonId

        //If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            Guid? personId = null;

            PersonResponse? response = await _personService.GetPersonByPersonId(personId);

            Assert.Null(response);
        }

        //If we supply a valid PersonId, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonId_WithPersonId()
        {
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse countryResponse = await _countriesService.AddCountry(request);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "person@sample.com",
                Address = "address",
                CountryId = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Male
            };

            PersonResponse responseFromAdd = await _personService.AddPerson(personAddRequest);
            PersonResponse? responseFromGetById = await _personService.GetPersonByPersonId(responseFromAdd.PersonId);

            Assert.Equal(responseFromAdd, responseFromGetById);
        }

        #endregion

        #region GetFilteredPersons

        //If the search text is empty and search by is PersonName, it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            List<PersonResponse> personResponses = await SamplePersonsForTest();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponses)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> getFilteredPersonsResult = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getFilteredPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            foreach (PersonResponse expected in personResponses)
            {
                Assert.Contains(expected, getFilteredPersonsResult);
            }
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByName()
        {
            List<PersonResponse> personResponses = await SamplePersonsForTest();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponses)
            {
                if (person.PersonName is not null && person.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    _testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> getFilteredPersonsResult = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getFilteredPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            foreach (PersonResponse expected in personResponses)
            {
                if (expected.PersonName is not null)
                {
                    if (expected.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(expected, getFilteredPersonsResult);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending order on PersonName
        [Fact]
        public async Task GetSortedPersons_SearchByName()
        {
            await SamplePersonsForTest();

            List<PersonResponse> allPersons = await _personService.GetAllPersons();
            //Sorting allPersons for expected value
            allPersons = allPersons.OrderByDescending(p => p.PersonName).ToList();

            //Printing expected values
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in allPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse> getSortedPersonsResult = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //Printing actual values
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getSortedPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            for (int i = 0; i < allPersons.Count; i++)
            {
                Assert.Equal(allPersons[i], getSortedPersonsResult[i]);
            }
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When we supply invalid PersonId, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid()
            };
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //When the PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponse.CountryID,
                Email = "john@sample.com",
                Address = "road 10",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Male
            };
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponse.CountryID,
                Address = "road 10",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "john@sample.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "william@sample.com";

            PersonResponse updatedPersonResponse = await _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? getPersonByIdResponse = await _personService.GetPersonByPersonId(updatedPersonResponse.PersonId);
            
            Assert.Equal(updatedPersonResponse, getPersonByIdResponse);
        }
        #endregion

        #region DeletePerson

        //If you supply an invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            Assert.False(await _personService.DeletePerson(Guid.NewGuid()));
        }

        //If you supply a valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryId = countryResponse.CountryID,
                Address = "road 10",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "john@sample.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            bool isDeleted = await _personService.DeletePerson(personResponse.PersonId);

            Assert.True(isDeleted);
        }

        //If you supply null as personId, it should return ArgumentNullException
        [Fact]
        public async Task DeletePerson_NullPersonId()
        {
            Guid? personId = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.DeletePerson(personId);
            });
        }
        #endregion
    }
}
