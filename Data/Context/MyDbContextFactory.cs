using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Data.Context
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseMySql("Server=bjyszb039pnpsyqp6pjs-mysql.services.clever-cloud.com;Database=bjyszb039pnpsyqp6pjs;User ID=u0az4gbfi9hxljh3;Password=q8M7rW5I7yhyew8rW5cK;");

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}