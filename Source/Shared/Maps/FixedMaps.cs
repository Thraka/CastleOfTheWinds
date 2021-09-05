using System;
using System.Drawing;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;

namespace CastleOfTheWinds
{
    internal class FixedMaps
    {
        private static readonly Size OneTile = new (1, 1);

        public static CastleMap Village { get; } = BuildVillage();

        private static CastleMap BuildVillage()
        {
            var terrain = @"
               x | 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6
               --+------------------------------------------------------
               0 | # # # # # # # # # # . . . + . . . . # # # # # # # # #
               1 | # # # # # # # # # # . . . + . . . . # # # # # # # # #
               2 | # # # # # # # # # # . . . + . . . . # # # # # # # # #
               3 | # # # # # # # # # # . . . + . . . . # # # # # # # # #
               4 | # # # # # # # # # # . . . + . . . . # # # # # # # # #
               5 | # # # # # . . . . . a + + + . . . . . . . # # # # # #
               6 | # # # # # . . . . a + b . + . . a + + . . # # # # # #
               7 | # # # # # . . + + + b . . + . a + b . . . # # # # # #
               8 | # # # # # . . . . . . . . + + + b . . . . . . # # # #
               9 | # # # # # . . . . . . . . + . . . . . . . . . # # # #
               0 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               1 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               2 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               3 | # # # # # # . . . . + + + + + + + . . . . . . # # # #
               4 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               5 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               6 | # # # # # # . . . . . . . + . . . . . . . . . # # # #
               7 | # # # # # # . . . . . . + + + . . . . . # # # # # # #
               8 | # # # # # # . . . . + + + . + + + . . . # # # # # # #
               9 | # # # # # # . . . . . . + + + . . . . . # # # # # # #
               0 | # # # # # # . . . . . . . + . . . . . . # # # # # # #
               1 | # # # # # # . . . . . . . + . . . . . . # # # # # # #
               2 | # # # # # # # # . . . . . + . . . # # # # # # # # # #
               3 | # # # # # # # # . . . . . . . . . # # # # # # # # # #
               4 | # # # # # # # # . . . . . . . . . # # # # # # # # # #
               5 | # # # # # # # # . . . . . . . . . # # # # # # # # # #
               6 | # # # # # # # # . . . . . . . . . # # # # # # # # # #
               7 | # # # # # # # # # # # # # # # # # # # # # # # # # # #
            ";

            var map = BuildMap("A Tiny Hamlet", terrain);

            AddTerrainObject(map, (5, 6), "A farm", "/terrain/village_hut_right", isWalkable: false, width: 3, height: 3);
            AddTerrainObject(map, (8, 17), "Bjorn the Blacksmith", "/terrain/village_hut_right", isWalkable: false, width: 3, height: 3);
            AddTerrainObject(map, (9, 13), "The home of the village sage", "/terrain/village_sage", isWalkable: false, width: 2, height: 2);
            AddTerrainObject(map, (11, 22), "A shrine to Odin", "/terrain/village_shrine", isWalkable: false, width: 5, height: 5);
            AddTerrainObject(map, (12, 0), "The hamlet gate", "/terrain/gate", isWalkable: false, width: 3, height: 1);
            AddTerrainObject(map, (16, 12), "Olaf's Junk Store", "/terrain/village_hut_left", isWalkable: false, width: 3, height: 3);
            AddTerrainObject(map, (16, 17), "Gunhild's General Store", "/terrain/village_hut_left", isWalkable: false, width: 3, height: 3);
            AddTerrainObject(map, (18, 5), "A farm", "/terrain/village_hut_left", isWalkable: false, width: 3, height: 3);

            AddSceneryObject(map, (7, 5), "A vegetable garden", "/scenery/garden", isWalkable: true);
            AddSceneryObject(map, (7, 9), "A wagon", "/scenery/wagon", isWalkable: true);
            AddSceneryObject(map, (11, 14), "A sign that says:\nSnorri the Sage", "/scenery/sign", isWalkable: true);
            AddSceneryObject(map, (11, 17), "A  sign that says:\nBjorn the Blacksmith", "/scenery/sign", isWalkable: true);
            AddSceneryObject(map, (13, 18), "the village well", "/scenery/well", isWalkable: false);
            AddSceneryObject(map, (14, 21), "A sign that says:\nShrine of Odin", "/scenery/sign", isWalkable: true);
            AddSceneryObject(map, (15, 12), "A sign that says:\nOlaf's Junk Store", "/scenery/sign", isWalkable: true);
            AddSceneryObject(map, (15, 17), "A sign that says:\nGunhild's General Store", "/scenery/sign", isWalkable: true);
            AddSceneryObject(map, (17, 4), "A vegetable garden", "/scenery/garden", isWalkable: true);
            
            return map;
        }

        private static CastleMap BuildMap(string name, string terrain)
        {
            var rows = terrain.Trim().Split('\n')
                .Skip(2) // Trim off the horizontal ruler
                .Select(x => x.Trim().Replace(" ", string.Empty).Substring(2)) // Trim off the vertical ruler
                .ToArray();

            var width = rows[0].Length;
            var height = rows.Length;

            var map = new CastleMap(name, width, height);

            for (var y = 0; y < height; y++)
            {
                var row = rows[y];
                if (row.Length != width)
                {
                    throw new ArgumentException($"Expected row {y} to be {width} wide", nameof(terrain));
                }

                for (var x = 0; x < width; x++)
                {
                    var position = new Coord(x, y);

                    var key = row[x];

                    var terrainObject = key switch
                    {
                        '#' => new TerrainObject(position, "farmland", "/terrain/farmland", false, OneTile),
                        '.' => new TerrainObject(position, "grass", "/terrain/grass", true, OneTile),
                        '+' => new TerrainObject(position, "road", "/terrain/road", true, OneTile),
                        'a' => new TerrainObject(position, "road", "/terrain/grass_road_br", true, OneTile),
                        'b' => new TerrainObject(position, "road", "/terrain/grass_road_tl", true, OneTile),
                        _ => throw new ArgumentException($"Unknown terrain character {key} at {position}", nameof(terrain))
                    };

                    map.SetTerrain(terrainObject);
                    map.Explored[position] = true;
                }
            }


            return map;
        }

        private static void AddSceneryObject(Map map, Coord position, string description, string imagePath, bool isWalkable)
        {
            map.AddEntity(new CastleObject(MapLayers.Scenery, position, description, imagePath, isWalkable: isWalkable));
        }

        private static void AddTerrainObject(Map map, Coord position, string description, string imagePath, bool isWalkable, int width, int height)
        {
            var terrainObject = new TerrainObject(position, description, imagePath, isWalkable, new Size(width, height));

            map.SetTerrain(terrainObject);

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
                    map.SetTerrain(extension);
                }
            }
        }
    }

    public class CastleMap : Map
    {
        public CastleMap(string name, int width, int height)
            : base(width, height, numberOfEntityLayers: 3, Distance.CHEBYSHEV)
        {
            Name = name;
        }

        public string Name { get; }
    }
}