using Api.Controllers;
using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void Get_ShouldReturnAllPersons()
        {
            // Arrange
            _repository.Setup(repo => repo.GetAll()).Returns(_persons);

            // Act
            var result = _controller.Get().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var returnedPersons = (result as OkObjectResult).Value as List<Person>;
            Assert.IsNotNull(returnedPersons);
            Assert.AreEqual(2, returnedPersons.Count);

        }
        [TestMethod]
        public void Get_ShouldReturnPersonById()
        {
            // Arrange
            _repository.Setup(repo => repo.Get(1)).Returns(_persons[0]);

            // Act
            var result = _controller.Get(1).Result;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedPerson = okResult.Value as Person;
            Assert.IsNotNull(returnedPerson);
            Assert.AreEqual(1, returnedPerson.PersonId);
            Assert.AreEqual("Matias", returnedPerson.Name);
            Assert.AreEqual("Velasquez", returnedPerson.Surname);
            Assert.AreEqual(24, returnedPerson.Age);
        }

        [TestMethod]
        public void Post_ShouldAddPerson()
        {
            // Arrange
            var person = _persons[0];
            _repository.Setup(repo => repo.Add(person)).Callback(() => { });

            // Act
            var result = _controller.Post(person).Result;

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnedPerson = createdResult.Value as Person;
            Assert.IsNotNull(returnedPerson);
            Assert.AreEqual(1, returnedPerson.PersonId);
            Assert.AreEqual("Matias", returnedPerson.Name);
            Assert.AreEqual("Velasquez", returnedPerson.Surname);
            Assert.AreEqual(24, returnedPerson.Age);
        }

        [TestMethod]
        public void Put_ShouldUpdatePerson()
        {
            // Arrange
            var personToUpdate = _persons[0];
            _repository.Setup(repo => repo.Update(It.IsAny<Person>())).Callback((Person p) =>
            {
                personToUpdate = p;
            });

            // Act
            var result = _controller.Put(1, new Person { PersonId = 1, Name = "Matias Alejandro" }).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _repository.Verify(repo => repo.Update(It.IsAny<Person>()), Times.Once());
            Assert.AreEqual("Matias Alejandro", personToUpdate.Name);
        }

        [TestMethod]
        public void Delete_ShouldRemovePerson()
        {
            // Arrange
            var personToDelete = _persons[0];
            //necesito mock del get porque lo utilizo en el delete para saber si existe
            _repository.Setup(repo => repo.Get(It.IsAny<int>())).Returns((int id) =>
            {
                return _persons.FirstOrDefault(p => p.PersonId == id);
            });
            _repository.Setup(repo => repo.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                personToDelete = _persons.FirstOrDefault(p => p.PersonId == id);
            });

            // Act
            var result = _controller.Delete(1).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _repository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public void GetShuffle_ShouldReturnRandomPerson()
        {
            // Arrange
            var persons = new List<Person>()
    {
        new Person { PersonId = 1, Name = "Person 1" },
        new Person { PersonId = 2, Name = "Person 2" },
        new Person { PersonId = 3, Name = "Person 3" }
    };
            _repository.Setup(repo => repo.GetShuffle()).Returns(() =>
            {
                int randomIndex = new Random().Next(persons.Count);
                return persons[randomIndex];
            });

            // Act
            var result = _controller.GetShuffle().Result as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(Person));
        }
    }
}
