using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Mars.Rover.Presentation.API.Extensions
{
    public static class Swagger
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("MarsRover", new OpenApiInfo
                {
                    Title = "Mars Rover API",
                    Version = "0.0.1",
                    Description = "Mars Rover Web API Documentation",
                    //Contact = new Contact
                    //{
                    //    Name = "Swagger Implementation of Kemal Akoğlu",
                    //    //Url = "http://doco.com.tr",
                    //    Email = "kemal.akoglu@doco.com.tr"
                    //}
                });
            });
        }
    }
}