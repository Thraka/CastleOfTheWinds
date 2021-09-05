using GoRogue;

namespace CastleOfTheWinds
{
    public class Creature : CastleObject
    {
        public Creature(Coord position, string description, string imagePath)
            : base(MapLayers.Creatures, position, description, imagePath, parentObject: null, isStatic: false, isWalkable: false, isTransparent: true)
        {
        }

        public int HitPointsMax { get; set; }

        public int ManaMax { get; set; }

        public int HitPoints { get; set; }

        public int Mana { get; set; }
        public int Speed { get; set; }
        public int SpeedMax { get; set; }
    }
}