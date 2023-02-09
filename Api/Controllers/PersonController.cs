using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/people")]
    [ApiController]
    ///<summary>
    /// Esto es un controlador de personas
    /// </summary>
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        [SwaggerOperation(
            Summary = "Obtiene personas",
            Description = "Este metodo trae todas las personas",
            OperationId = "Personas_Get",
            Tags = new[] { "Personas" }
        )]
        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            var persons = _personRepository.GetAll();
            if (persons.Count != 0) return Ok(_personRepository.GetAll());
            return StatusCode(204, "La lista de personas esta vacia");
        }

        /// <summary>
        ///  Este metodo trae una persona segun su id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            var person = _personRepository.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        /// <summary>
        ///  Este metodo trae una persona aleatoria
        /// </summary>
        /// <returns></returns>
        [HttpGet("shuffle")]
        public ActionResult<Person> GetShuffle()
        {
            var person = _personRepository.GetShuffle();
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        /// <summary>
        ///  Este metodo agrega una persona
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Person> Post(Person person)
        {
            _personRepository.Add(person);
            return CreatedAtAction(nameof(Get), new { id = person.PersonId }, person);
        }

        /// <summary>
        ///  Este metodo actualiza una persona
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<Person> Put(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }
            _personRepository.Update(person);
            return NoContent();
        }

        /// <summary>
        ///  Este metodo elimina una persona
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<Person> Delete(int id)
        {
            var person = _personRepository.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            _personRepository.Delete(id);
            return NoContent();
        }
    }
}
