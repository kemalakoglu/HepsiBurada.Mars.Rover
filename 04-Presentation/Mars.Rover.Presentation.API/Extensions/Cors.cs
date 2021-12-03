﻿using Microsoft.Extensions.DependencyInjection;

namespace Mars.Rover.Presentation.API.Extensions
{
    public static class Cors
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
               options.AddPolicy("CorsPolicy",
               builder => builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
            });
        }
    }
}