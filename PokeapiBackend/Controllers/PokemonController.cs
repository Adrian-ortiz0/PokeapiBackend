using Microsoft.AspNetCore.Mvc;
using PokeapiApplication.Dtos;
using PokeapiApplication.Interfaces;

namespace PokeapiBackend.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(
            IPokemonService pokemonService,
            ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<PokemonListDto>> GetPokemonList(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20)
        {
            try
            {
                var result = await _pokemonService.GetPokemonListAsync(offset, limit);
                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pokemon list");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PokemonDto>> GetPokemonById(int id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonByIdAsync(id);

                if (pokemon == null)
                    return NotFound(new { message = $"Pokemon {id} not found" });

                return Ok(pokemon);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pokemon {id}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<PokemonDto>> GetPokemonByName(string name)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonByNameAsync(name);

                if (pokemon == null)
                    return NotFound(new { message = $"Pokemon '{name}' not found" });

                return Ok(pokemon);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pokemon {name}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("names")]
        public async Task<ActionResult<List<PokemonBasicDto>>> GetAllNames([FromQuery] int limit = 1500)
        {
            try
            {
                var names = await _pokemonService.GetAllPokemonNamesAsync(limit);
                return Ok(names);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pokemon names");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<PokemonWithSpriteDto>>> SearchPokemon(
            [FromQuery] string term,
            [FromQuery] int maxResults = 20)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { message = "Search term is required" });

            try
            {
                var results = await _pokemonService.SearchPokemonAsync(term, maxResults);
                return Ok(results);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching pokemon");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
