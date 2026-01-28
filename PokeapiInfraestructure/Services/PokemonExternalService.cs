using Microsoft.Extensions.Logging;
using PokeapiDomain.Entities;
using PokeapiDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeapiInfraestructure.Services
{
    public class PokemonExternalService : IPokemonExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokemonExternalService> _logger;

        public PokemonExternalService(
            HttpClient httpClient,
            ILogger<PokemonExternalService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiResponse> GetPokemonListAsync(int offset, int limit)
        {
            try
            {
                var url = $"pokemon?offset={offset}&limit={limit}";

                _logger.LogInformation($"Calling PokeAPI: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResult = JsonSerializer.Deserialize<PokeApiListResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new ApiResponse
                {
                    Count = apiResult.Count,
                    Next = apiResult.Next,
                    Previous = apiResult.Previous,
                    Results = apiResult.Results.Select(r => new PokemonBasic
                    {
                        Name = r.Name,
                        Url = r.Url
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling PokeAPI");
                throw;
            }
        }

        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            try
            {
                var url = $"pokemon/{id}"; 

                _logger.LogInformation($"Calling PokeAPI: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiPokemon = JsonSerializer.Deserialize<PokeApiPokemon>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return MapToPokemon(apiPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pokemon {id}");
                throw;
            }
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            try
            {
                var url = $"pokemon/{name.ToLower()}"; 

                _logger.LogInformation($"Calling PokeAPI: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiPokemon = JsonSerializer.Deserialize<PokeApiPokemon>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return MapToPokemon(apiPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pokemon {name}");
                throw;
            }
        }

        private Pokemon MapToPokemon(PokeApiPokemon apiPokemon)
        {
            return new Pokemon
            {
                Id = apiPokemon.Id,
                Name = apiPokemon.Name,
                Height = apiPokemon.Height,
                Weight = apiPokemon.Weight,
                SpriteUrl = apiPokemon.Sprites?.Front_Default ?? string.Empty,
                OfficialArtwork = apiPokemon.Sprites?.Other?.OfficialArtwork?.Front_Default ?? string.Empty,
                Types = apiPokemon.Types?.Select(t => new PokemonTypeSlot
                {
                    Slot = t.Slot,
                    TypeName = t.Type.Name
                }).ToList() ?? new List<PokemonTypeSlot>(),
                Created = DateTime.UtcNow,
                LastSync = DateTime.UtcNow
            };
        }

        private class PokeApiListResponse
        {
            public int Count { get; set; }
            public string? Next { get; set; }
            public string? Previous { get; set; }
            public List<PokeApiBasic> Results { get; set; }
        }

        private class PokeApiBasic
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        private class PokeApiPokemon
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Height { get; set; }
            public int Weight { get; set; }
            public PokeApiSprites? Sprites { get; set; }
            public List<PokeApiTypeSlot>? Types { get; set; }
        }

        private class PokeApiSprites
        {
            public string? Front_Default { get; set; }
            public PokeApiOther? Other { get; set; }
        }

        private class PokeApiOther
        {
            public PokeApiOfficialArtwork? OfficialArtwork { get; set; }
        }

        private class PokeApiOfficialArtwork
        {
            public string? Front_Default { get; set; }
        }

        private class PokeApiTypeSlot
        {
            public int Slot { get; set; }
            public PokeApiType Type { get; set; }
        }

        private class PokeApiType
        {
            public string Name { get; set; }
        }
    }
}