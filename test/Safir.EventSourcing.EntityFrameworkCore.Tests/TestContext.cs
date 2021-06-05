using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    public class TestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=:memory:");
        }
    }
}
