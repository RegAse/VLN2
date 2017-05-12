using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Example.BusinessLogicTest;
using VLN2.Models;

namespace VLN2.Tests.Service
{
    [TestClass]
    public class ProgrammingLanguageServiceTest
    {
        private ProgrammingLanguageService _service;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange:
            MockDatabase context = new MockDatabase();
            var f1 = new ProgrammingLanguage
            {
                ID = 1,
                Name = "Javascript",
                FileExtension = "js"
            };
            context.ProgrammingLanguages.Add(f1);
            _service = new ProgrammingLanguageService(context);
        }

        [TestMethod]
        public void GetProgrammingLanguageByExtensionTest()
        {
            // Act: 
            var result = _service.GetProgrammingLanguageByExtension("js");

            // Assert:
            Assert.IsTrue((result.Name == "Javascript"));
        }
    }
}
