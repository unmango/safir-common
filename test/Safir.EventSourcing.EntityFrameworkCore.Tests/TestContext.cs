using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    public sealed class TestContext : DbContext
    {
        private readonly DbConnection _connection;
        
        public TestContext() : base(BuildOptions())
        {
            Database.EnsureCreated();
            _connection = Database.GetDbConnection();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            
            // Limitation of SQLite in memory provider
            modelBuilder.Entity<Event>().Property(x => x.Id)
                .HasValueGenerator((p, e) => new SequentialLongValueGenerator(p, e));
            modelBuilder.Entity<Event>().Property(x => x.Position)
                .HasValueGenerator((p, e) => new SequentialIntValueGenerator(p, e));
        }

        public override void Dispose()
        {
            _connection.Dispose();
            base.Dispose();
        }

        private static DbContextOptions<TestContext> BuildOptions() =>
            new DbContextOptionsBuilder<TestContext>()
                .UseSqlite(CreateDatabase())
                .Options;

        private static DbConnection CreateDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        private class SequentialLongValueGenerator : ValueGenerator<long>
        {
            private readonly IProperty _property;
            private readonly IEntityType _entityType;
            private long _last = 0;

            public SequentialLongValueGenerator(IProperty property, IEntityType entityType)
            {
                _property = property;
                _entityType = entityType;
            }
            
            public override long Next(EntityEntry entry)
            {
                return ++_last;
            }

            public override bool GeneratesTemporaryValues => false;
        }

        private class SequentialIntValueGenerator : ValueGenerator<int>
        {
            private readonly IProperty _property;
            private readonly IEntityType _entityType;
            private int _last = 0;

            public SequentialIntValueGenerator(IProperty property, IEntityType entityType)
            {
                _property = property;
                _entityType = entityType;
            }
            
            public override int Next(EntityEntry entry)
            {
                return ++_last;
            }

            public override bool GeneratesTemporaryValues => false;
        }
    }
}
