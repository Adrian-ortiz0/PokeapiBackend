using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiApplication.Dtos
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public SpritesDto Sprites { get; set; }
        public List<TypeDto> Types { get; set; }
    }

    public class SpritesDto
    {
        public string FrontDefault { get; set; }
        public OtherSpritesDto Other { get; set; }
    }

    public class OtherSpritesDto
    {
        public OfficialArtworkDto OfficialArtwork { get; set; }
    }

    public class OfficialArtworkDto
    {
        public string FrontDefault { get; set; }
    }

    public class TypeDto
    {
        public int Slot { get; set; }
        public TypeInfoDto Type { get; set; }
    }

    public class TypeInfoDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PokemonListDto
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokemonBasicDto> Results { get; set; }
    }

    public class PokemonBasicDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PokemonWithSpriteDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string? Sprite { get; set; }
    }
}
