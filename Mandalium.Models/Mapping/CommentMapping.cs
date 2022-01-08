using Mandalium.Demo.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mandalium.Demo.Models.Mapping
{
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.CodeArea).IsRequired().HasColumnType("nvarchar").HasMaxLength(4000);
            builder.Property(p => p.CreatedBy).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasColumnType("nvarchar").HasMaxLength(50);

            builder.HasOne(p => p.Blog);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
