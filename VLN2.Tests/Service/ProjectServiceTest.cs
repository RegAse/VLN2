using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Example.BusinessLogicTest;
using VLN2.Models;
using VLN2.Tests.ServiceClasses;

namespace VLN2.Tests.Service
{
    [TestClass]
    public class ProjectServiceTest
    {
        //ProjectService _service = new ProjectService();
        private ProjectService _service;

        [TestInitialize]
        public void Initialize()
        {
            MockDatabase context = new MockDatabase();
            var f1 = new Project
            {
                ID = 1,
                Name = "Test",
                Description = "test1"
            };
            var f2 = new ProjectFile
            {
                ID = 1,
                Content = "test",
                Name = "test.html",
                IsFolder = false
            };
            context.Projects.Add(f1);
            context.ProjectFiles.Add(f2);
            _service = new ProjectService(context);
        }

        [TestMethod]
        public void TestGetProjectByID()
        {
            int id = 1;
            var result = _service.GetProjectByID(id);
            Assert.IsTrue(result.ID == id);
        }

        [TestMethod]
        public void TestGetProjectByIDNone()
        {
            int id = 2;
            var result = _service.GetProjectByID(id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetProjectFileByID()
        {
            //int id = 1;
            //var result = _service.GetProjectFileByID(id);
            //Assert.IsTrue(result.ID == id);
        }
    }
}
