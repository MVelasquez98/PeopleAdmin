using Core.PersonRepository.Interfaces;
using Data.Context;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Core.PersonRepository.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MyDbContext _context;
        public PersonRepository(MyDbContext context)
        {
            _context = context;
        }
        public void Add(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var person = _context.Persons.Find(id);
            _context.Persons.Remove(person);
            _context.SaveChanges();
        }

        public Person Get(int id)
        {
            return _context.Persons.Find(id);
        }

        public List<Person> GetAll()
        {
            return _context.Persons.ToList();
        }

        public Person GetShuffle()
        {
            int count = _context.Persons.Count();
            int randomId = new Random().Next(1, count + 1);
            Person randomPerson = _context.Persons.Find(randomId);
            return randomPerson;
        }

        public void Update(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
