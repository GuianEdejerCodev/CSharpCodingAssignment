using CodingAssignment.Models;

namespace CodingAssignment.Services.Interfaces
{
    public interface IFileManagerService
    {
        /// <summary>
        /// Returns the complete data model 
        /// </summary>
        /// <returns></returns>
        DataFileModel GetData();

        /// <summary>
        /// Returns the data model with the specified id
        /// </summary>
        /// <returns></returns>
        DataModel GetData(int id);

        /// <summary>
        /// Insert new data model 
        /// </summary>
        /// <returns></returns>
        bool Insert(DataModel model);

        /// <summary>
        /// Updates a specific data model using the id as the key
        /// </summary>
        /// <returns></returns>
        bool Update(DataModel model, int id);

        /// <summary>
        /// Deletes a specific data model using the id as the key
        /// </summary>
        /// <returns></returns>
        bool Delete(int id);
    }
}
