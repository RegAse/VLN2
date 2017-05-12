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
            var f2 = new ProgrammingLanguage
            {
                ID = 2,
                Name = "HTML",
                FileExtension = "html"
            };
            var f3 = new ProgrammingLanguage
            {
                ID = 3,
                Name = "C#",
                FileExtension = "cs"
            };
            context.ProgrammingLanguages.Add(f1);
            context.ProgrammingLanguages.Add(f2);
            context.ProgrammingLanguages.Add(f3);

            _service = new ProgrammingLanguageService(context);
        }

        [TestMethod]
        public void GetProgrammingLanguageByExtensionTest()
        {
            // Act: 
            var result = _service.GetProgrammingLanguageByExtension("js");

            // Assert:
            Assert.IsTrue((result.Name == "Javascript"));

            // Act: 
            var result1 = _service.GetProgrammingLanguageByExtension("cs");

            // Assert:
            Assert.IsTrue((result1.Name == "C#"));

            // Act: 
            var result2 = _service.GetProgrammingLanguageByExtension("html");

            // Assert:
            Assert.IsTrue((result2.Name == "HTML"));
        }
    }
}
