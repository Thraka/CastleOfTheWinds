using GoRogue;

namespace CastleOfTheWinds
{
    public class Creature : CastleObject
    {
        public Creature(Coord position, string description, string imagePath)
            : base(MapLayers.Creatures, position, description, imagePath, parentObject: null, isStatic: false, isWalkable: false, isTransparent: true)
        {
        }
    }
}