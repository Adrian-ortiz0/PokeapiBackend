using PokeapiDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiDomain.Services
{
    public interface IPokemonExternalService
    {
        Task<ApiResponse> GetPokemonListAsync(int offset, int limit);
        Task<Pokemon> GetPokemonByIdAsync(int id);
        Task<Pokemon> GetPokemonByNameAsync(string name);
    }

    public class ApiResponse
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokemonBasic> Results { get; set; } = new();
    }

    public class PokemonBasic
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
