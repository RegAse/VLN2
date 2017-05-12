using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Example.BusinessLogicTest;

namespace VLN2.Tests.Service
{
    [TestClass]
    public class ProjectServiceTest
    {
        //ProjectService _service = new ProjectService();
        private ProjectServiceTest _service;

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
            context.Projects.Add(f1);
            _service = new ProjectServiceTest(context);
        }

        [TestMethod]
        public void TestGetProjectByID()
        {
            int id = 1;
            //var result = _service.TestGetProjectByID(id);
        }
    }
}
