using Data.Model;
using System.Collections.Generic;

namespace Core.PersonRepository.Interfaces
{
    public interface IPersonRepository
    {
        Person Get(int id);
        Person GetShuffle();
        List<Person> GetAll();
        void Add(Person person);
        void Update(Person person);
        void Delete(int id);
    }
}
