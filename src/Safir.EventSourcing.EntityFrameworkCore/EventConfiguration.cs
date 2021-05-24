using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}
