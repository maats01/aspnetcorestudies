﻿using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;
using OfficeOpenXml;
using RepositoryContracts;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;

        public PersonsService(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            await _personsRepository.AddPerson(person);

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personsRepository.GetPersons();

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId is null)
            {
                return null;
            }

            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (person is null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            if (searchString == null)
                searchString = string.Empty;

            List<Person> persons = searchBy switch
            {
                nameof(PersonResponse.PersonName) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.PersonName.Contains(searchString)),

                nameof(PersonResponse.Email) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.Email.Contains(searchString)),

                nameof(PersonResponse.DateOfBirth) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                nameof(PersonResponse.Gender) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.Gender.Contains(searchString)),

                nameof(PersonResponse.CountryName) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.Country.CountryName.Contains(searchString)),

                nameof(PersonResponse.Address) =>
                    await _personsRepository.GetFilteredPersons(p =>
                    p.Address.Contains(searchString)),

                _ => await _personsRepository.GetPersons()
            };

            return persons.Select(temp => temp.ToPersonResponse()).ToList();
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //model validations
            ValidationHelper.ModelValidation(personUpdateRequest);

            //getting matching person by id 
            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);

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

            await _personsRepository.UpdatePerson(matchingPerson);

            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId is null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (matchingPerson is null)
            {
                return false;
            }

            await _personsRepository.DeletePersonByPersonId(personId.Value);

            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
           
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
                CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
                //csvWriter.WriteHeader<PersonResponse>();
                csvWriter.WriteField(nameof(PersonResponse.PersonName));
                csvWriter.WriteField(nameof(PersonResponse.Email));
                csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
                csvWriter.WriteField(nameof(PersonResponse.Age));
                csvWriter.WriteField(nameof(PersonResponse.Gender));
                csvWriter.WriteField(nameof(PersonResponse.CountryName));
                csvWriter.WriteField(nameof(PersonResponse.Address));
                csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
                csvWriter.NextRecord();

                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    csvWriter.WriteField(person.PersonName);
                    csvWriter.WriteField(person.Email);
                    if (person.DateOfBirth.HasValue)
                        csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                    else
                        csvWriter.WriteField("");
                    csvWriter.WriteField(person.Age);
                    csvWriter.WriteField(person.Gender);
                    csvWriter.WriteField(person.CountryName);
                    csvWriter.WriteField(person.Address);
                    csvWriter.WriteField(person.ReceiveNewsLetters);
                    csvWriter.NextRecord();
                }
            }
            MemoryStream newMemoryStream = new MemoryStream(memoryStream.ToArray());
            newMemoryStream.Position = 0;

            return newMemoryStream;
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                workSheet.Cells["A1"].Value = "Person Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Date of Birth";
                workSheet.Cells["D1"].Value = "Age";
                workSheet.Cells["E1"].Value = "Gender";
                workSheet.Cells["F1"].Value = "Country";
                workSheet.Cells["G1"].Value = "Address";
                workSheet.Cells["H1"].Value = "Receive News Letters";

                using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                
                List<PersonResponse> persons = await GetAllPersons();
                foreach (PersonResponse person in persons)
                {
                    workSheet.Cells[row, 1].Value = person.PersonName;
                    workSheet.Cells[row, 2].Value = person.Email;
                    if (person.DateOfBirth.HasValue)
                        workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    workSheet.Cells[row, 4].Value = person.Age;
                    workSheet.Cells[row, 5].Value = person.Gender;
                    workSheet.Cells[row, 6].Value = person.CountryName;
                    workSheet.Cells[row, 7].Value = person.Address;
                    workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                    row++;
                }

                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
