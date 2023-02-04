using Api.Controllers;
using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleAdminTest.Api.Controller
{
    [TestClass]
    public class PersonControllerTest
    {
        private readonly PersonController _controller;
        private readonly Mock<IPersonRepository> _repository;

        public PersonControllerTest()
        {
            _repository = new Mock<IPersonRepository>();
            _controller = new PersonController(_repository.Object);
        }

        [TestMethod]
        public async Task Get_ShouldReturnAllPersons()
        {
            // Arrange
            var persons = new List<Person>
        {
            new Person {PersonId = 1, Name = "Matias", Surname="Velasquez", Age=24},
            new Person {PersonId = 2, Name = "Hector", Surname="Velasquez", Age=50}
       };
            _repository.Setup(repo => repo.GetAll()).Returns(persons);

            // Act
            var result = _controller.Get().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var returnedPersons = (result as OkObjectResult).Value as List<Person>;
            Assert.IsNotNull(returnedPersons);
            Assert.AreEqual(2, returnedPersons.Count);

        }
    }
}
