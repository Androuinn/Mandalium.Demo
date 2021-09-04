
using Mandalium.Models.DomainModels;
using Mandalium.Models.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Mandalium.Core.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<SystemAuthenticationKey> SystemAuthenticationKeys { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("Mandalium.Demo"));

            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }
        }
    }
}