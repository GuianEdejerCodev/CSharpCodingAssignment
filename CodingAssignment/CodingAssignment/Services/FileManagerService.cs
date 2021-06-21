using System.IO.Abstractions;
using System.Linq;
using CodingAssignment.Models;
using CodingAssignment.Services.Interfaces;
using Newtonsoft.Json;

namespace CodingAssignment.Services
{
    public class FileManagerService : IFileManagerService
    {
        const string DataFilePath = "./AppData/DataFile.json";
        readonly IFileSystem _fileSystem;

        public FileManagerService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public DataFileModel GetData()
        {
            var data = JsonConvert.DeserializeObject<DataFileModel>(_fileSystem.File.ReadAllText(DataFilePath));

            return data;
        }

        public DataModel GetData(int id)
        {
            var data = GetData();

            var result = data.Data.FirstOrDefault(d => d.Id == id);

            return result;
        }

        public bool Insert(DataModel model)
        {
            var data = GetData();

            if (!data.Data.Exists(d => d.Id == model.Id))
            {
                data.Data.Add(model);
                return SaveChanges(data);
            }

            return false;
        }

        public bool Update(DataModel model, int id)
        {
            var data = GetData();

            var dataForUpdate = data.Data.FirstOrDefault(d => d.Id == id);

            if (dataForUpdate != null)
            {
                dataForUpdate.Dictionary = model.Dictionary;
                return SaveChanges(data);
            }

            return false;
        }

        public bool Delete(int id)
        {
            var data = GetData();

            var dataForDeletion = data.Data.FirstOrDefault(d => d.Id == id);

            if (dataForDeletion != null)
            {
                data.Data.Remove(dataForDeletion);
                return SaveChanges(data);
            }

            return false;
        }

        private bool SaveChanges(DataFileModel data)
        {
            try
            {
                _fileSystem.File.WriteAllText(DataFilePath, JsonConvert.SerializeObject(data));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
