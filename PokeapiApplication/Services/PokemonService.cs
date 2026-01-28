using Microsoft.Extensions.Logging;
using PokeapiApplication.Dtos;
using PokeapiApplication.Interfaces;
using PokeapiDomain.Entities;
using PokeapiDomain.Repositories;
using PokeapiDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiApplication.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonExternalService _externalService;
        private readonly IPokemonRepository _repository;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(
            IPokemonExternalService externalService,
            IPokemonRepository repository,
            ILogger<PokemonService> logger)
        {
            _externalService = externalService;
            _repository = repository;
            _logger = logger;
        }

        public async Task<PokemonListDto> GetPokemonListAsync(int offset, int limit)
        {
            try
            {
                var apiResponse = await _externalService.GetPokemonListAsync(offset, limit);

                return new PokemonListDto
                {
                    Count = apiResponse.Count,
                    Next = apiResponse.Next,
                    Previous = apiResponse.Previous,
                    Results = apiResponse.Results.Select(r => new PokemonBasicDto
                    {
                        Name = r.Name,
                        Url = r.Url
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pokemon list");
                throw;
            }
        }

        public async Task<PokemonDto?> GetPokemonByIdAsync(int id)
        {
            try
            {
                var dbPokemon = await _repository.GetByIdAsync(id);

                if (dbPokemon != null)
                {
                    return MapToDto(dbPokemon);
                }

                var pokemon = await _externalService.GetPokemonByIdAsync(id);

                if (pokemon != null)
                {
                    await SyncPokemonToDbAsync(pokemon);
                }

                return MapToDto(pokemon);
            }
            catch (HttpRequestException)
            {
                _logger.LogWarning($"External API failed for pokemon {id}, using cache");
                var dbPokemon = await _repository.GetByIdAsync(id);
                return dbPokemon != null ? MapToDto(dbPokemon) : null;
            }
        }

        public async Task<PokemonDto?> GetPokemonByNameAsync(string name)
        {
            try
            {
                var pokemon = await _externalService.GetPokemonByNameAsync(name);

                if (pokemon != null)
                {
                    await SyncPokemonToDbAsync(pokemon);
                }

                return MapToDto(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pokemon {name}");
                throw;
            }
        }

        public async Task<List<PokemonBasicDto>> GetAllPokemonNamesAsync(int limit)
        {
            try
            {
                var apiResponse = await _externalService.GetPokemonListAsync(0, limit);

                return apiResponse.Results.Select(r => new PokemonBasicDto
                {
                    Name = r.Name,
                    Url = r.Url
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pokemon names");
                throw;
            }
        }

        public async Task<List<PokemonWithSpriteDto>> SearchPokemonAsync(string searchTerm, int maxResults)
        {
            try
            {
                var allNamesResponse = await _externalService.GetPokemonListAsync(0, 1500);

                var matchingNames = allNamesResponse.Results
                    .Where(p => p.Name.Contains(searchTerm.ToLower()))
                    .Take(maxResults)
                    .ToList();

                var pokemonTasks = matchingNames.Select(async p =>
                {
                    var id = ExtractIdFromUrl(p.Url);
                    return await _externalService.GetPokemonByIdAsync(id);
                });

                var results = await Task.WhenAll(pokemonTasks);

                return results
                    .Where(p => p != null)
                    .Select(p => new PokemonWithSpriteDto
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Url = $"https://pokeapi.co/api/v2/pokemon/{p.Id}/",
                        Sprite = p.SpriteUrl
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching pokemon with term: {searchTerm}");
                throw;
            }
        }

        private async Task SyncPokemonToDbAsync(Pokemon pokemon)
        {
            try
            {
                var exists = await _repository.ExistsAsync(pokemon.Id);

                if (exists)
                {
                    await _repository.UpdateAsync(pokemon);
                }
                else
                {
                    await _repository.AddAsync(pokemon);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error syncing pokemon {pokemon.Id}");
            }
        }

        private int ExtractIdFromUrl(string url)
        {
            var parts = url.TrimEnd('/').Split('/');
            return int.Parse(parts[^1]);
        }

        private PokemonDto MapToDto(Pokemon pokemon)
        {
            return new PokemonDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                Sprites = new SpritesDto
                {
                    FrontDefault = pokemon.SpriteUrl,
                    Other = new OtherSpritesDto
                    {
                        OfficialArtwork = new OfficialArtworkDto
                        {
                            FrontDefault = pokemon.OfficialArtwork
                        }
                    }
                },
                Types = pokemon.Types.Select(t => new TypeDto
                {
                    Slot = t.Slot,
                    Type = new TypeInfoDto
                    {
                        Name = t.TypeName,
                        Url = $"https://pokeapi.co/api/v2/type/{t.TypeName}/"
                    }
                }).ToList()
            };
        }
    }
}