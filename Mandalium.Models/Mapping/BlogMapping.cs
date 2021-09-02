using Mandalium.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Models.Mapping
{
    public class BlogMapping : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Headline).IsRequired().HasColumnType("nvarchar(200)").HasMaxLength(200);
            builder.Property(p => p.SubHeadline).IsRequired().HasColumnType("nvarchar(500)").HasMaxLength(500);
            builder.Property(p => p.CodeArea).IsRequired().HasColumnType("nvarchar(MAX)");
            builder.Property(p => p.ImageUrl).HasColumnType("nvarchar(500)").HasMaxLength(500);
            builder.Property(p => p.CreatedBy).HasColumnType("nvarchar(50)").HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasColumnType("nvarchar(50)").HasMaxLength(50);

            builder.HasOne(p => p.Topic);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
