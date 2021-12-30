using App_Code.Middlewares;
using Mandalium.API.App_Code.Swagger;
using Mandalium.API.AutoMapperProfiles;
using Mandalium.Core.Abstractions.Interfaces;
using Mandalium.Core.Context;
using Mandalium.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mandalium.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddDbContext(services);
            AddControllers(services);
            AddUnitOfWork(services);
            AddSwagger(services);
            AddMemoryCache(services);
            AddAutoMapper(services);
        }

      

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mandalium API");
                c.DisplayRequestDuration();
            });

            app.UseRequestAuthenticationMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseRequestCultureMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(d => d.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly("Mandalium.Demo.Core")));
        }

        public void AddControllers(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddRequiredHeaderParameter>();
            });
        }

        public void AddMemoryCache(IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        private void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config => { config.AddProfile<DefaultProfile>(); });
        }
    }
}
