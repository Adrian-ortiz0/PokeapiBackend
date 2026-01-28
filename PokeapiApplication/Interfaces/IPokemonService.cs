using PokeapiApplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiApplication.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonListDto> GetPokemonListAsync(int offset, int limit);
        Task<PokemonDto?> GetPokemonByIdAsync(int id);
        Task<PokemonDto?> GetPokemonByNameAsync(string name);
        Task<List<PokemonBasicDto>> GetAllPokemonNamesAsync(int limit);
        Task<List<PokemonWithSpriteDto>> SearchPokemonAsync(string searchTerm, int maxResults);
    }
}
