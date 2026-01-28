using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeapiDomain.Repositories;
using PokeapiDomain.Services;
using PokeapiInfraestructure.Persistence;
using PokeapiInfraestructure.Repositories;
using PokeapiInfraestructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiInfraestructure.DependecyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
                ));

            services.AddScoped<IPokemonRepository, PokemonRepository>();

            services.AddHttpClient<IPokemonExternalService, PokemonExternalService>(client =>
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}