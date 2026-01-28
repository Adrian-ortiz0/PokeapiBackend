CREATE DATABASE IF NOT EXISTS pokemon_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE pokemon_db;

CREATE TABLE IF NOT EXISTS Pokemons (
    Id INT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Height INT NOT NULL,
    Weight INT NOT NULL,
    SpriteUrl VARCHAR(500),
    OfficialArtwork VARCHAR(500),
    Types JSON COMMENT 'Array of {Slot: int, TypeName: string}',
    Created DATETIME,
    LastSync DATETIME,
    INDEX idx_name (Name),
    INDEX idx_lastsync (LastSync)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;