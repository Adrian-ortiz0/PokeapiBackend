using PokeapiDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiDomain.Repositories
{
    public interface IPokemonRepository
    {
        Task<Pokemon?> GetByIdAsync(int id);
        Task<List<Pokemon>> GetAllAsync();
        Task<Pokemon?> AddAsync(Pokemon pokemon);
        Task<Pokemon?> UpdateAsync(Pokemon pokemon);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
