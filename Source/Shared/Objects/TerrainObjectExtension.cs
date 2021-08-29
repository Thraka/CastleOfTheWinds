using GoRogue;

using System.Drawing;

namespace CastleOfTheWinds
{
    public class TerrainObjectExtension : CastleObject
    {
        public TerrainObjectExtension(Coord position, TerrainObject parent)
            : base(parent.Layer, position, parent.Description, isStatic: parent.IsStatic, isWalkable: parent.IsWalkable, isTransparent: parent.IsTransparent)
        {
            ParentTerrainObject = parent;
        }

        public TerrainObject ParentTerrainObject { get; }

        public override Image? GetImage() => null;
    }
}