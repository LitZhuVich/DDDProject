using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DDD.Infrastructure
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DDDProject;Integrated Security=True;Trust Server Certificate=True");
            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
