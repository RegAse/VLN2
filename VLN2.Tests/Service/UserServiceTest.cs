using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Example.BusinessLogicTest;
using VLN2.Tests.ServiceClasses;
using VLN2.Models;

namespace VLN2.Tests.Service
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange:
            MockDatabase context = new MockDatabase();

            _service = new UserService(context);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
