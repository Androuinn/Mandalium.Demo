using Mandalium.Demo.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mandalium.Demo.Models.Mapping
{
    public class BlogMapping : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Headline).IsRequired().HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(p => p.SubHeadline).IsRequired().HasColumnType("nvarchar").HasMaxLength(500);
            builder.Property(p => p.CodeArea).IsRequired().HasColumnType("nvarchar(MAX)");
            builder.Property(p => p.ImageUrl).HasColumnType("nvarchar").HasMaxLength(500);
            builder.Property(p => p.CreatedBy).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasColumnType("nvarchar").HasMaxLength(50);

            builder.HasOne(p => p.Topic);
            builder.HasMany(p => p.Comments).WithOne(p => p.Blog).HasForeignKey(x => x.BlogId);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
