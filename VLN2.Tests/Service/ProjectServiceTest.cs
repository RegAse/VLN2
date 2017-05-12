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
            int id = 1;
            var result = _service.GetProjectByID(32);
            System.Diagnostics.Debug.WriteLine(result.ID);
            Assert.AreEqual(1, result);
            
        }
    }
}
