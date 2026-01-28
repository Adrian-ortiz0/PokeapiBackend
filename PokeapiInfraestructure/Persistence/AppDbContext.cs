using Microsoft.EntityFrameworkCore;
using PokeapiDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeapiInfraestructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.ToTable("Pokemons");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Height).IsRequired();
                entity.Property(e => e.Weight).IsRequired();
                entity.Property(e => e.SpriteUrl).HasMaxLength(500);
                entity.Property(e => e.OfficialArtwork).HasMaxLength(500);

                entity.Property(e => e.Types)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<PokemonTypeSlot>>(v, (JsonSerializerOptions)null) ?? new List<PokemonTypeSlot>())
                    .HasColumnType("json");

                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.LastSync);
            });
        }
    }
}

