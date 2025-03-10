using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a new person object to the data store
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>The person object after inserting it into the data store</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all persons in the data store
        /// </summary>
        /// <returns>Returns a list of person type </returns>
        Task<List<Person>> GetPersons();

        /// <summary>
        /// Returns a person based on person id; otherwise, returns null
        /// </summary>
        /// <param name="personId">Person id to search</param>
        /// <returns>Matching person or null</returns>
        Task<Person?> GetPersonByPersonId(Guid personId);

        /// <summary>
        /// Returns all person objects based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with the given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the person id 
        /// </summary>
        /// <param name="personId">Person id to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePersonByPersonId(Guid personId);
        
        /// <summary>
        /// Updates a person object based on the given person id
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
