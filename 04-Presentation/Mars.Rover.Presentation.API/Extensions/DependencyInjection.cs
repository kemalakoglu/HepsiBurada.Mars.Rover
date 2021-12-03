using Mars.Rover.Application.Contract.Services;
using Mars.Rover.Application.Service;
using Mars.Rover.Application.UnitOfWork;
using Mars.Rover.Core.Contract;
using Mars.Rover.Domain.Aggregate.Navigate;
using Mars.Rover.Domain.Context.Context;
using Mars.Rover.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mars.Rover.Presentation.API.Extensions
{
    public static class DependencyInjection
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];
            services.AddDbContext<MarsRoverContext>(o => o.UseSqlServer(connectionString));

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<Context>()
            //    .AddDefaultTokenProviders();
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDomainService(this IServiceCollection services)
        {
            services.AddSingleton<INavigateService, NavigateService>();
        }

        public static void ConfigureApplicationService(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationService, ApplicationService>();
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddSingleton<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}