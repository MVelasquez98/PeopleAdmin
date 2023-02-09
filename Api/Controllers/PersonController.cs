using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<Person>>> Get()
        {
            var persons = await _personRepository.GetAll();
            if (persons.Count != 0) return Ok(persons);
            return StatusCode(204, "La lista de personas esta vacia");
        }

        /// <summary>
        ///  Este metodo trae una persona segun su id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _personRepository.Get(id);
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
        public async Task<ActionResult<Person>> GetShuffle()
        {
            var person = await _personRepository.GetShuffle();
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
        public async Task<ActionResult<Person>> Post(Person person)
        {
            await _personRepository.Add(person);
            return CreatedAtAction(nameof(Get), new { id = person.PersonId }, person);
        }

        /// <summary>
        ///  Este metodo actualiza una persona
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }
            await _personRepository.Update(person);
            return NoContent();
        }

        /// <summary>
        ///  Este metodo elimina una persona
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await _personRepository.Get(id);
            if (person == null)
            {
                return NotFound();
            }
            await _personRepository.Delete(id);
            return NoContent();
        }
    }
}
