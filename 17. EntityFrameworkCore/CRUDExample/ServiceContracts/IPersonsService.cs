using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="person">Person to add</param>
        /// <returns>Converted person into a new PersonResponse object with newly generated person id</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>List of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Fetches a person object based on the given person id
        /// </summary>
        /// <param name="personId">PersonId to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Field to search</param>
        /// <param name="searchString">String to search</param>
        /// <returns>Returns all matching based PersonResponse objects as a list, based on given paramaters</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents the list to be sorted</param>
        /// <param name="sortBy">Name of the field, based on which the list should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns the List<PersonResponse> object sorted</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified person details
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person object after update</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes the person with specified id
        /// </summary>
        /// <param name="personId">Id of the person to be deleted</param>
        /// <returns>True of false, indicating if the operation was sucessful or not</returns>
        Task<bool> DeletePerson(Guid? personId);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with CSV data of persons</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
