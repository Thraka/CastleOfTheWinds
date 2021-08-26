using GoRogue.GameFramework;
using GoRogue;

namespace CastleOfTheWinds
{
    // Factory class for "entities", eg. everything not terrain
    static class EntityFactory
    {
        // Similar to above.  For this object, we need to make sure to set the isWalkable to false, as the player collides with other things!
        public static GameObject Player(Coord position) => new GameObject(position, layer: 1, parentObject: null, isStatic: false, isWalkable: false, isTransparent: true);
    }
}
