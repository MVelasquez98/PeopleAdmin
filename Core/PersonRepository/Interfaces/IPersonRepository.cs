using Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.PersonRepository.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> Get(int id);
        Task<Person> GetShuffle();
        Task<List<Person>> GetAll();
        Task Add(Person person);
        Task Update(Person person);
        Task Delete(int id);
    }
}
