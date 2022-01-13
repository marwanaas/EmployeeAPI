using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeWebAPI.Models;
using EmployeeWebAPI.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EmployeeWebAPI
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
            services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Employee API",
                    Description = "Employee Management API",
                    TermsOfService = new Uri("https://NomSociété.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Malek",
                        Email = "malek.zribi@gmail.com",
                        Url = new Uri("https://twitter.com/malek"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Nom Société Open License",
                        Url = new Uri("https://NomSociété.com"),
                    }
                });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This middleware serves generated Swagger document as a JSON endpoint          	
            
            app.UseSwagger();

            // This middleware serves the Swagger documentation UI 
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
                c.RoutePrefix = string.Empty;

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

