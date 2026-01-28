using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeapiDomain.Entities
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string SpriteUrl { get; set; }
        public string OfficialArtwork { get; set; }
        public List<PokemonTypeSlot> Types { get; set; } = new();
        public DateTime Created { get; set; }
        public DateTime LastSync { get; set; }
    }

    public class PokemonTypeSlot
    {
        public int Slot { get; set; }
        public string TypeName { get; set; }
    }
}
