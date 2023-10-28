using AutoFilterer.Swagger;
using GenericRepository.Demo.Context;
using MarkdownDocumenting.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository.Generic;
using System.Reflection;

namespace GenericRepository.Demo
{
    public static class ServiceExtensions
    {
        public static void ProjectSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddEndpointsApiExplorer();

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);

            // registering generic repository
            services.ApplyGenericRepository<ApiDbContext>();

            // swagger documentation setup
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("GenericRepository", new OpenApiInfo()
                {
                    Title = "Generic Repository",
                    Version = "1.0.0",
                    Description = "Generic repository with some great features e.g. CRUD, count, where, include, filtering, sorting, hard n soft delete. etc.",
                    Contact = new OpenApiContact()
                    {
                        Name = "Kashif Javed",
                        Email = "kashifj521@hotmail.com",
                        Url = new Uri("https://github.com/kashifjaved1/GenericRepository.NET7")
                    }
                });
                options.UseAutoFiltererParameters();

                //getting documentation xml / md filename
                //var docFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var docFile = $"{Assembly.GetEntryAssembly().GetName().Name}.md";

                // setting documentation file path
                var filePath = Path.Combine(AppContext.BaseDirectory, docFile);

                if (File.Exists((filePath)))
                {
                    //options.IncludeXmlComments(filePath);
                }
                options.DescribeAllParametersInCamelCase();
            });

            // finally adding documentation.
            services.AddDocumentation(); // Add this for default configuration.
        }
    }
}
