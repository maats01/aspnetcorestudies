using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _persons.Add(new Person() 
                { 
                    PersonId = Guid.Parse("441F9BA8-EC46-41BA-B3A4-E348E747D2EC"), 
                    PersonName = "Brianna", Email = "bvearncombe0@biglobe.ne.jp", 
                    DateOfBirth = DateTime.Parse("1995-12-06"), 
                    CountryId = Guid.Parse("0364C2F1-44C7-486D-AE9C-3F1E4B3CDAC9"),
                    Gender = "Female",
                    Address = "29998 Havey Court",
                    ReceiveNewsLetters = false
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("CAEB9CEC-3A52-4260-86A8-41D3EF5525FC"),
                    PersonName = "Candace",
                    Email = "crivelon1@bizjournals.com",
                    DateOfBirth = DateTime.Parse("1995-06-04"),
                    CountryId = Guid.Parse("63617046-FDB4-442D-A00F-80850B60FFB5"),
                    Gender = "Female",
                    Address = "382 American Ash Plaza",
                    ReceiveNewsLetters = true
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("7729E3A4-5439-404A-9C2F-82B8DE8776C0"),
                    PersonName = "Gaylor",
                    Email = "glamping2@psu.edu",
                    DateOfBirth = DateTime.Parse("1990-01-02"),
                    CountryId = Guid.Parse("0364C2F1-44C7-486D-AE9C-3F1E4B3CDAC9"),
                    Gender = "Male",
                    Address = "16989 North Pass",
                    ReceiveNewsLetters = true
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("27997D6F-9506-482B-A49D-74E739BCA08C"),
                    PersonName = "Mirabel",
                    Email = "mespinha3@spotify.com",
                    DateOfBirth = DateTime.Parse("1999-02-17"),
                    CountryId = Guid.Parse("A88EA912-1FED-4C96-9F05-E9FB8A4B9546"),
                    Gender = "Female",
                    Address = "203 Tennessee Parkway",
                    ReceiveNewsLetters = false
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("CF3C827C-57F0-453B-99C5-C6BBBFA15839"),
                    PersonName = "Adria",
                    Email = "adana4@trellian.com",
                    DateOfBirth = DateTime.Parse("1993-06-04"),
                    CountryId = Guid.Parse("A88EA912-1FED-4C96-9F05-E9FB8A4B9546"),
                    Gender = "Female",
                    Address = "9387 Red Cloud Parkway",
                    ReceiveNewsLetters = true
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("679A0362-E329-454F-8A62-BD3BAED481CD"),
                    PersonName = "John",
                    Email = "john31@email.com",
                    DateOfBirth = DateTime.Parse("1997-02-25"),
                    CountryId = Guid.Parse("521B318F-2E8E-484E-B052-0BBD20A902DE"),
                    Gender = "Male",
                    Address = "9387 Red Cloud Parkway",
                    ReceiveNewsLetters = true
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("C1348AFE-63D2-4552-9998-C2093347F477"),
                    PersonName = "Myriam",
                    Email = "mklimko5@sitemeter.com",
                    DateOfBirth = DateTime.Parse("1997-12-11"),
                    CountryId = Guid.Parse("2BFF6716-7B91-4476-AB55-7DE717B5562C"),
                    Gender = "Female",
                    Address = "6429 Center Park",
                    ReceiveNewsLetters = true
                });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse response = person.ToPersonResponse();
            response.CountryName = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;

            return response;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId is null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);

            if (person is null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> persons = GetAllPersons();
            List<PersonResponse> matchingPersons = persons;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = persons.Where(p =>
                    (!string.IsNullOrEmpty(p.PersonName) 
                    ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    : true)
                    ).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = persons.Where(p =>
                    (!string.IsNullOrEmpty(p.Email)
                    ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    : true)
                    ).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = persons.Where(p =>
                    (p.DateOfBirth is not null
                    ? p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    : true)
                    ).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = persons.Where(p =>
                    (!string.IsNullOrEmpty(p.Gender)
                    ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    : true)
                    ).ToList();
                    break;

                case nameof(PersonResponse.CountryName):
                    matchingPersons = persons.Where(p =>
                    (!string.IsNullOrEmpty(p.CountryName)
                    ? p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    : true)
                    ).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = persons.Where(p =>
                    (!string.IsNullOrEmpty(p.Address)
                    ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    : true)
                    ).ToList();
                    break;

                default:
                    matchingPersons = persons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //model validations
            ValidationHelper.ModelValidation(personUpdateRequest);

            //getting matching person by id 
            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);

            if (matchingPerson is null)
            {
                throw new ArgumentException("Invalid person id");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return ConvertPersonToPersonResponse(matchingPerson);
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId is null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonId == personId);

            if (matchingPerson is null)
            {
                return false;
            }

            return _persons.Remove(matchingPerson);
        }
    }
}
