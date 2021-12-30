using Mandalium.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mandalium.Models.Mapping
{
    public class TopicMapping : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(p => p.CreatedBy).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
