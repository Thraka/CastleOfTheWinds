using System;
using System.Drawing;
using System.Linq;
using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.GameFramework;

namespace CastleOfTheWinds.Maps
{
    public abstract class CastleMap : Map
    {
        public static readonly Size OneTile = new(1, 1);
        
        public CastleMap(string name, int width, int height)
            : base(width, height, numberOfEntityLayers: 3, Distance.CHEBYSHEV)
        {
            Name = name;
        }

        public string Name { get; }

        public virtual bool BeforeObjectMove(Game game, CastleObject entity, Map toMap, Coord toCoordinates)
        {
            return true;
        }

        public virtual void AfterObjectMove(Game game, CastleObject entity, Map fromMap, Coord fromCoordinates)
        {
        }

        protected void AddTerrain(string terrain)
        {
            var rows = terrain.Trim().Split('\n')
                .Skip(3) // Trim off the horizontal ruler
                .Select(x => x.Trim().Replace(" ", string.Empty).Substring(3)) // Trim off the vertical ruler
                .ToArray();

            for (var y = 0; y < Height; y++)
            {
                var row = rows[y];
                if (row.Length != Width)
                {
                    throw new ArgumentException($"Expected row {y} to be {Width} wide", nameof(terrain));
                }

                for (var x = 0; x < Width; x++)
                {
                    var position = new Coord(x, y);

                    var key = row[x];

                    var terrainObject = key switch
                    {
                        '#' => new TerrainObject(position, "farmland", "/terrain/farmland.png", false, OneTile),
                        '.' => new TerrainObject(position, "grass", "/terrain/grass.png", true, OneTile),
                        '+' => new TerrainObject(position, "road", "/terrain/road.png", true, OneTile),
                        'a' => new TerrainObject(position, "road", "/terrain/grass_road_br.png", true, OneTile),
                        'b' => new TerrainObject(position, "road", "/terrain/grass_road_tl.png", true, OneTile),
                        'C' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_bl.png", false, OneTile),
                        'D' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_br.png", false, OneTile),
                        'E' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_bottom.png", false, OneTile),
                        'F' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_tl.png", false, OneTile),
                        'H' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_left.png", false, OneTile),
                        'I' => new TerrainObject(position, "mountain", "/terrain/snow_mountain_right.png", false, OneTile),
                        'S' => new TerrainObject(position, "mountain", "/terrain/snow.png", false, OneTile),
                        'W' => new TerrainObject(position, "mountain", "/terrain/mountain_grass_bottom.png", false, OneTile),
                        'X' => new TerrainObject(position, "mountain", "/terrain/mountain.png", false, OneTile),
                        'Y' => new TerrainObject(position, "mountain", "/terrain/mountain_grass_br.png", false, OneTile),
                        'Z' => new TerrainObject(position, "mountain", "/terrain/mountain_grass_bl.png", false, OneTile),
                        _ => throw new ArgumentException($"Unknown terrain character {key} at {position}", nameof(terrain))
                    };

                    SetTerrain(terrainObject);
                }
            }
        }

        protected void AddSceneryObject(Coord position, string description, string imagePath, bool isWalkable)
        {
            AddEntity(new CastleObject(MapLayers.Scenery, position, description, imagePath, isWalkable: isWalkable));
        }

        protected void AddTerrainObject(Coord position, string description, string imagePath, bool isWalkable, int width, int height)
        {
            var terrainObject = new TerrainObject(position, description, imagePath, isWalkable, new Size(width, height));

            SetTerrain(terrainObject);

            for (var x = position.X; x < position.X + width; x++)
            {
                for (var y = position.Y; y < position.Y + height; y++)
                {
                    Coord childPosition = (x, y);
                    if (childPosition == position)
                    {
                        continue;
                    }

                    var extension = new TerrainObjectExtension(childPosition, terrainObject);
                    terrainObject.Extensions.Add(extension);
                    SetTerrain(extension);
                }
            }
        }
    }
}