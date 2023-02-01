using System.ComponentModel.DataAnnotations;
namespace Data.Model
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public float Age { get; set; }

        public Person(string name, string surname, float age)
        {
            this.Name = name;
            this.Surname = surname;
            this.Age = age;
        }
        public Person()
        {
        }
    }
}