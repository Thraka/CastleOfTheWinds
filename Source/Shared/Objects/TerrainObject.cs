using System.Collections.Generic;
using System.Drawing;
using GoRogue;

namespace CastleOfTheWinds.Objects
{
    public class TerrainObject : CastleObject
    {
        public TerrainObject(Coord position, string description, string imagePath, bool isWalkable, Size size)
            : base(MapLayers.Terrain, position, description, imagePath, isStatic: true, isWalkable: isWalkable)
        {
            Size = size;
        }

        public Size Size { get; }

        public List<TerrainObjectExtension> Extensions { get; } = new();
    }
}