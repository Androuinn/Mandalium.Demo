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
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.CodeArea).IsRequired().HasColumnType("nvarchar(4000)").HasMaxLength(4000);
            builder.Property(p => p.CreatedBy).HasColumnType("nvarchar(50)").HasMaxLength(50);
            builder.Property(p => p.ModifiedBy).HasColumnType("nvarchar(50)").HasMaxLength(50);

            builder.HasOne(p => p.Blog);
            builder.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}
