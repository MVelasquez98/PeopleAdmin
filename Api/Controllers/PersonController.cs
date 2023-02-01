using Core.PersonRepository.Interfaces;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public ActionResult<List<Person>> Get()
        {
            var persons = _personRepository.GetAll();
            if (persons.Count != 0) return Ok(_personRepository.GetAll());
            return StatusCode(204, "La lista de personas esta vacia");
        }

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

        [HttpPost]
        public ActionResult<Person> Post(Person person)
        {
            _personRepository.Add(person);
            return CreatedAtAction(nameof(Get), new { id = person.PersonId }, person);
        }

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
