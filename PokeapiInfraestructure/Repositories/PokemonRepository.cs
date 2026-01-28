using Microsoft.EntityFrameworkCore;
using PokeapiDomain.Entities;
using PokeapiDomain.Repositories;
using PokeapiInfraestructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiInfraestructure.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pokemon?> GetByIdAsync(int id)
        {
            return await _context.Pokemons.FindAsync(id);
        }

        public async Task<List<Pokemon>> GetAllAsync()
        {
            return await _context.Pokemons.ToListAsync();
        }

        public async Task<Pokemon?> AddAsync(Pokemon pokemon)
        {
            await _context.Pokemons.AddAsync(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }

        public async Task<Pokemon?> UpdateAsync(Pokemon pokemon)
        {
            _context.Pokemons.Update(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null) return false;

            _context.Pokemons.Remove(pokemon);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pokemons.AnyAsync(p => p.Id == id);
        }
    }
}
