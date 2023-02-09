using Core.PersonRepository.Interfaces;
using Data.Context;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.PersonRepository.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MyDbContext _context;
        public PersonRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task Add(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
             _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }

        public async Task<Person> Get(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task<List<Person>> GetAll()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person> GetShuffle()
        {
            int count = _context.Persons.Count();
            int randomId = new Random().Next(1, count + 1);
            Person randomPerson = await _context.Persons.FindAsync(randomId);
            return randomPerson;
        }

        public async Task Update(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
