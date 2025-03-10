using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit;
using Xunit.Abstractions;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        private readonly IPersonsRepository _personsRepository;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _personService = new PersonsService(_personsRepository);
            _testOutputHelper = testOutputHelper;
        }

        private List<Person> SamplePersonsForTest()
        {
            Person person1 = _fixture.Build<Person>()
                .With(temp => temp.Email, "scott@email.com")
                .With(temp => temp.PersonName, "scott")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person person2 = _fixture.Build<Person>()
                .With(temp => temp.Email, "john@email.com")
                .With(temp => temp.PersonName, "john")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person person3 = _fixture.Build<Person>()
                .With(temp => temp.Email, "mary@email.com")
                .With(temp => temp.PersonName, "mary")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<Person> persons = new List<Person>()
            {
                person1, person2, person3
            };

            return persons;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException 
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            PersonAddRequest? request = null;

            Func<Task> action = async () =>
            {
                await _personService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null vaule as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_NullPersonName_ToBeArgumentException()
        {
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            Person person = request.ToPerson();

            _personsRepositoryMock
                .Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            Func<Task> action = async () =>
            {
                await _personService.AddPerson(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply proper person details, it should insert the person into the persons list;
        //it should also return a new object of PersonResponse which contains the newly generated PersonId
        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
        {
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@example.com")
                .Create();

            Person person = request.ToPerson();
            PersonResponse expectedPersonResponse = person.ToPersonResponse();
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            PersonResponse personResponse = await _personService.AddPerson(request);
            expectedPersonResponse.PersonId = personResponse.PersonId;

            //Assert.True(personResponse.PersonId != Guid.Empty);
            personResponse.PersonId.Should().NotBe(Guid.Empty);
            personResponse.Should().Be(expectedPersonResponse);
        }
        #endregion

        #region GetAllPersons

        //The persons list must be empty by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            List<Person> persons = new List<Person>();

            _personsRepositoryMock
                .Setup(temp => temp.GetPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> personsResponse = await _personService.GetAllPersons();

            //Assert.Empty(persons);
            personsResponse.Should().BeEmpty();
        }

        //First, we will add a few persons; then, when calling GetAllPersons,
        //it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons_ToBeSuccessful()
        {
            List<Person> persons = SamplePersonsForTest();

            List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personsRepositoryMock
                .Setup(temp => temp.GetPersons())
                .ReturnsAsync(persons);

            List<PersonResponse> getAllPersonsResult = await _personService.GetAllPersons();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getAllPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            getAllPersonsResult.Should().BeEquivalentTo(personResponsesExpected);
        }

        #endregion

        #region GetPersonByPersonId

        //If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId_ToReturnNull()
        {
            Guid? personId = null;

            PersonResponse? response = await _personService.GetPersonByPersonId(personId);

            response.Should().BeNull();
        }

        //If we supply a valid PersonId, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonId_WithPersonId_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "example@email.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            PersonResponse personResponseExpected = person.ToPersonResponse();

            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse? responseFromGetById = await _personService.GetPersonByPersonId(person.PersonId);

            //Assert.Equal(responseFromAdd, responseFromGetById);
            responseFromGetById.Should().Be(personResponseExpected);
        }

        #endregion

        #region GetFilteredPersons

        //If the search text is empty and search by is PersonName, it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            List<Person> persons = SamplePersonsForTest();

            List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesExpected)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            _personsRepositoryMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);

            List<PersonResponse> getFilteredPersonsResult = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getFilteredPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            getFilteredPersonsResult.Should().BeEquivalentTo(personResponsesExpected);
        }

        //Search based on person name with some search string, it should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByName_ToBeSuccessful()
        {
            List<Person> persons = SamplePersonsForTest();

            List<PersonResponse> personResponsesExpected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in personResponsesExpected)
            {
                if (person.PersonName is not null && person.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    _testOutputHelper.WriteLine(person.ToString());
            }

            _personsRepositoryMock
                .Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync((Expression<Func<Person, bool>> predicate) => persons.AsQueryable().Where(predicate).ToList());

            List<PersonResponse> getFilteredPersonsResult = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Print personResponses elements with output helper
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getFilteredPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            getFilteredPersonsResult.Should().OnlyContain(temp => temp.PersonName != null && temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending order on PersonName
        [Fact]
        public void GetSortedPersons_SortByNameInDesc_ToBeSuccessful()
        {
            List<Person> persons = SamplePersonsForTest();

            List<PersonResponse> allPersons = persons.Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> getSortedPersonsResult = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //Sorting allPersons for expected value
            allPersons = allPersons.OrderByDescending(p => p.PersonName).ToList();

            //Printing expected values
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in allPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            //Printing actual values
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getSortedPersonsResult)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }

            getSortedPersonsResult.Should().BeInDescendingOrder(temp => temp.PersonName);
        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid PersonId, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ToBeArgumentException()
        {
            PersonUpdateRequest? personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When the PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Email, "josh@email.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse personResponse = person.ToPersonResponse();

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Josh")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Email, "josh@email.com")
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse personResponseExpected = person.ToPersonResponse();

            PersonUpdateRequest personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

            _personsRepositoryMock
                .Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse updatedPersonResponse = await _personService.UpdatePerson(personUpdateRequest);

            updatedPersonResponse.Should().Be(personResponseExpected);
        }
        #endregion

        #region DeletePerson

        //If you supply an invalid PersonId, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());
            isDeleted.Should().BeFalse();
        }

        //If you supply a valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Josh")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .With(temp => temp.Email, "josh@email.com")
                .Create();

            _personsRepositoryMock
                .Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            bool isDeleted = await _personService.DeletePerson(person.PersonId);

            isDeleted.Should().BeTrue();

            _personsRepositoryMock.Verify(temp => temp.GetPersonByPersonId(person.PersonId), Times.Once);
            _personsRepositoryMock.Verify(temp => temp.DeletePersonByPersonId(person.PersonId), Times.Once);
        }

        //If you supply null as personId, it should return ArgumentNullException
        [Fact]
        public async Task DeletePerson_NullPersonId()
        {
            Guid? personId = null;

            Func<Task> action = async () =>
            {
                await _personService.DeletePerson(personId);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        #endregion
    }
}
