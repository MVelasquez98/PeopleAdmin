using Api.Controllers;
using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAdminTest.Api.Controller
{
    [TestClass]
    public class PersonControllerTest
    {
        private readonly PersonController _controller;
        private readonly Mock<IPersonRepository> _repository;
        private List<Person> _persons;

        [TestInitialize]
        public void Setup()
        {
            _persons = new List<Person>(){
            new Person {PersonId = 1, Name = "Matias", Surname="Velasquez", Age=24},
            new Person {PersonId = 2, Name = "Hector", Surname="Velasquez", Age=50}};
        }
        public PersonControllerTest()
        {
            _repository = new Mock<IPersonRepository>();
            _controller = new PersonController(_repository.Object);
        }

        [TestMethod]
        public async Task Get_ShouldReturnAllPersons()
        {
            // Arrange
            _repository.Setup(repo => repo.GetAll()).ReturnsAsync(_persons);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result.Result;
            var returnedPersons = (List<Person>)okResult.Value;
            Assert.IsNotNull(returnedPersons);
            Assert.AreEqual(2, returnedPersons.Count);

        }
        [TestMethod]
        public async Task Get_ShouldReturnPersonById()
        {
            // Arrange
            _repository.Setup(repo => repo.Get(1)).ReturnsAsync(_persons[0]);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(okResult);
            var returnedPerson = okResult.Value as Person;
            Assert.IsNotNull(returnedPerson);
            Assert.AreEqual(1, returnedPerson.PersonId);
            Assert.AreEqual("Matias", returnedPerson.Name);
            Assert.AreEqual("Velasquez", returnedPerson.Surname);
            Assert.AreEqual(24, returnedPerson.Age);
        }

        [TestMethod]
        public async Task Post_ShouldAddPerson()
        {
            // Arrange
            var person = _persons[0];
            _repository.Setup(repo => repo.Add(person)).Callback(() => { });

            // Act
            var result = await _controller.Post(person);

            // Assert
            var createdResult = (CreatedAtActionResult)result.Result;
            Assert.IsNotNull(createdResult);
            var returnedPerson = createdResult.Value as Person;
            Assert.IsNotNull(returnedPerson);
            Assert.AreEqual(1, returnedPerson.PersonId);
            Assert.AreEqual("Matias", returnedPerson.Name);
            Assert.AreEqual("Velasquez", returnedPerson.Surname);
            Assert.AreEqual(24, returnedPerson.Age);
        }

        [TestMethod]
        public async Task Put_ShouldUpdatePerson()
        {
            // Arrange
            var personToUpdate = _persons[0];
            _repository.Setup(repo => repo.Update(It.IsAny<Person>())).Callback((Person p) =>
            {
                personToUpdate = p;
            });

            // Act
            var result = await _controller.Put(1, new Person { PersonId = 1, Name = "Matias Alejandro" });

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _repository.Verify(repo => repo.Update(It.IsAny<Person>()), Times.Once());
            Assert.AreEqual("Matias Alejandro", personToUpdate.Name);
        }

        [TestMethod]
        public async Task Delete_ShouldRemovePerson()
        {
            // Arrange
            var personToDelete = _persons[0];
            //necesito mock del get porque lo utilizo en el delete para saber si existe
            _repository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return _persons.FirstOrDefault(p => p.PersonId == id);
            });
            _repository.Setup(repo => repo.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                personToDelete = _persons.FirstOrDefault(p => p.PersonId == id);
            });

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _repository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public async Task GetShuffle_ShouldReturnRandomPerson()
        {
            // Arrange
            var persons = new List<Person>()
    {
        new Person { PersonId = 1, Name = "Person 1" },
        new Person { PersonId = 2, Name = "Person 2" },
        new Person { PersonId = 3, Name = "Person 3" }
    };
            _repository.Setup(repo => repo.GetShuffle()).ReturnsAsync(() =>
            {
                int randomIndex = new Random().Next(persons.Count);
                return persons[randomIndex];
            });

            // Act
            var result = await _controller.GetShuffle();
            var okResult = (OkObjectResult)result.Result;
            // Assert
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
            Assert.IsInstanceOfType(okResult.Value, typeof(Person));
        }
    }
}
