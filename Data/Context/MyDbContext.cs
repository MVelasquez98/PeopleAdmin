using Microsoft.EntityFrameworkCore;
using Data.Model;
namespace Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}