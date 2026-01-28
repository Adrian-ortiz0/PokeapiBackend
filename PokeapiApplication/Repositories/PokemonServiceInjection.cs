using Microsoft.Extensions.DependencyInjection;
using PokeapiApplication.Interfaces;
using PokeapiApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiApplication.Repositories
{
    public static class PokemonServiceInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();
            return services;
        }
    }
}
