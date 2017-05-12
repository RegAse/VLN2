using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VLN2.Services;

namespace VLN2.Tests.Service
{
    [TestClass]
    public class ProjectServiceTest
    {
        ProjectService _service = new ProjectService();
        [TestMethod]
        public void TestGetProjectByID()
        {
            //Arrange:
            int id = 32;
            //Act:
            var result = _service.GetProjectByID(id);
            //Assert:
            Assert.AreEqual(id, result.ID);
            
        }
    }
}
