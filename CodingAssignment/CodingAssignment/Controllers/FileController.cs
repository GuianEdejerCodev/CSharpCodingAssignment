using CodingAssignment.Models;
using CodingAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodingAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {

        private readonly IFileManagerService _fileManager;

        public FileController(IFileManagerService fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpGet]
        public ActionResult<DataFileModel> Get()
        {
            var data = _fileManager.GetData();
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpGet("{id}")]
        public ActionResult<DataModel> Get(int id)
        {
            var dataModel = _fileManager.GetData(id);
            if (dataModel == null)
            {
                return NotFound();
            }
            return dataModel;
        }

        [HttpPost]
        public ActionResult<DataFileModel> Post(DataModel model)
        {
            var result = _fileManager.Insert(model);
            if (!result)
            {
                return NotFound();
            }

            return _fileManager.GetData();
        }

        [HttpPut]
        public ActionResult<DataFileModel> Put(DataModel model, int id)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var result = _fileManager.Update(model, id);
            if (!result)
            {
                return NotFound();
            }

            return _fileManager.GetData();
        }

        [HttpDelete]
        public ActionResult<DataFileModel> Delete(int id)
        {
            var result = _fileManager.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return _fileManager.GetData();
        }
    }
}
