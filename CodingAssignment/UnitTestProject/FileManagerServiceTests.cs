using CodingAssignment.Models;
using CodingAssignment.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class FileManagerServiceTests
    {

        private FileManagerService _service;
        private IFileSystem _fileSystem;
        private DataFileModel _testDataFile;

        [TestInitialize]
        public void Init()
        {
            _testDataFile = new DataFileModel()
            {
                Data = new List<DataModel>
                {
                    new DataModel()
                    {
                        Id = 1,
                        Dictionary = new Dictionary<int, List<string>>()
                        {
                            { 0, new List<string>() { "One", "Two", "Three", "Four" } },
                            { 1, new List<string>() { "Five", "Six", "Seven", "Eight" } }
                        }
                    },
                    new DataModel()
                    {
                        Id = 2,
                        Dictionary = new Dictionary<int, List<string>>()
                        {
                            { 2, new List<string>() { "Nine", "Ten", "Eleven", "Twelve" } },
                            { 3, new List<string>() { "Thirteen", "Fourteen", "Fifteen", "Sixteen" } }
                        }
                    }
                }
            };
            _fileSystem = Substitute.For<IFileSystem>();
            _fileSystem.File.ReadAllText(default).ReturnsForAnyArgs(JsonConvert.SerializeObject(_testDataFile));
            _service = new FileManagerService(_fileSystem);
        }

        [TestMethod]
        public void DeserailizeDataTest()
        {
            //Arrange

            //Act
            var res = _service.GetData();

            //Assert
            Assert.IsNotNull(res);

            Assert.IsInstanceOfType(res, typeof(DataFileModel));
        }

        [TestMethod]
        public void Get_FileExist_ReturnsCorrectDataFile()
        {
            //Arrange
            var expected = _testDataFile;

            //Act
            var res = _service.GetData();

            //Assert
            Assert.AreEqual(expected.Data.Count, res.Data.Count);
            for (var i = 0; i < expected.Data.Count; i++)
            {
                Assert.AreEqual(expected.Data[i].Id, res.Data[i].Id);
                Assert.IsTrue(CompareDataDictionary(expected.Data[i].Dictionary, res.Data[i].Dictionary));
            }
        }

        [TestMethod]
        public void Get_IdExists_ReturnsCorrectData()
        {
            //Arrange
            var expected = _testDataFile.Data[0];

            //Act
            var res = _service.GetData(1);

            //Assert
            Assert.IsNotNull(res);
            Assert.IsInstanceOfType(res, typeof(DataModel));
            Assert.AreEqual(expected.Id, res.Id);
            Assert.IsTrue(CompareDataDictionary(expected.Dictionary, res.Dictionary));
        }

        [TestMethod]
        public void Get_IdDoesntExists_ReturnsNull()
        {
            //Arrange

            //Act
            var res = _service.GetData(3);

            //Assert
            Assert.IsNull(res);
        }

        [TestMethod]
        public void Insert_IdDoesntExists_WritesUpdatedData()
        {
            //Arrange
            var data = new DataModel
            {
                Id = 3,
                Dictionary = new Dictionary<int, List<string>>()
                {
                    {0, new List<string>(){ "One", "Two", "Three", "Four"} },
                    {1, new List<string>(){ "Five", "Six", "Seven", "Eight"} }
                }
            };
            _testDataFile.Data.Add(data);

            //Act
            var res = _service.Insert(data);

            //Assert
            Assert.IsTrue(res);

            _fileSystem.File.Received(1).WriteAllText(Arg.Any<string>(), JsonConvert.SerializeObject(_testDataFile));
        }

        [TestMethod]
        public void Insert_IdExists_DoesNotWriteUpdatedData()
        {
            //Arrange
            var data = new DataModel
            {
                Id = 1,
                Dictionary = new Dictionary<int, List<string>>()
                {
                    {0, new List<string>(){ "One", "Two", "Three", "Four"} },
                    {1, new List<string>(){ "Five", "Six", "Seven", "Eight"} }
                }
            };

            //Act
            var res = _service.Insert(data);

            //Assert
            Assert.IsFalse(res);

            _fileSystem.File.DidNotReceive().WriteAllText(Arg.Any<string>(), JsonConvert.SerializeObject(_testDataFile));
        }

        [TestMethod]
        public void Update_IdExists_WritesUpdatedData()
        {
            //Arrange
            var data = _testDataFile.Data[0];
            data.Dictionary[1] = new List<string>
            {
                "One",
                "Two"
            };

            //Act
            var res = _service.Update(data, data.Id);

            //Assert
            Assert.IsTrue(res);

            _fileSystem.File.Received(1).WriteAllText(Arg.Any<string>(), JsonConvert.SerializeObject(_testDataFile));
        }

        [TestMethod]
        public void Update_DoesNotExists_DoesNotWritesUpdatedData()
        {
            //Arrange
            var data = _testDataFile.Data[0];
            data.Dictionary[1] = new List<string>
            {
                "One",
                "Two"
            };

            //Act
            var res = _service.Update(data, 3);

            //Assert
            Assert.IsFalse(res);

            _fileSystem.File.DidNotReceive().WriteAllText(Arg.Any<string>(), Arg.Any<string>());
        }

        [TestMethod]
        public void Delete_IdExists_WritesUpdatedData()
        {
            //Arrange
            var data = _testDataFile.Data[0];
            var deleteId = data.Id;
            _testDataFile.Data.Remove(data);

            //Act
            var res = _service.Delete(deleteId);

            //Assert
            Assert.IsTrue(res);

            _fileSystem.File.Received(1).WriteAllText(Arg.Any<string>(), JsonConvert.SerializeObject(_testDataFile));
        }

        [TestMethod]
        public void Delete_DoesNotExists_DoesNotWritesUpdatedData()
        {
            //Arrange

            //Act
            var res = _service.Delete(3);

            //Assert
            Assert.IsFalse(res);

            _fileSystem.File.DidNotReceive().WriteAllText(Arg.Any<string>(), Arg.Any<string>());
        }

        private bool CompareDataDictionary(Dictionary<int, List<string>> dict1, Dictionary<int, List<string>> dict2)
        {
            if (dict1.Keys.Count != dict2.Keys.Count)
            {
                return false;
            }

            foreach (var key in dict1.Keys)
            {
                if (Enumerable.SequenceEqual(dict1[key], dict2[key]) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
