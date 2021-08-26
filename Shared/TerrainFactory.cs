using GoRogue.GameFramework;
using GoRogue;

namespace CastleOfTheWinds
{
    // Factory class for terrain.  Things to note here, are that both terrain GameObjects
    // are placed on layer 0, and have their isStatic set to true (indicating that the object cannot move).
    // If these things are incorrectly set, an exception will be raised when we try to add them to the map
    // using SetTerrain later
    static class TerrainFactory
    {
        // Note also that both objects have the parentObject flag set to null.  This is because they have no parent,
        // as we are not inheriting from GameObject, nor are we using a GameObject instance as a backing field.  If
        // either of these things were true, the parameter would be "this" instead.
        public static GameObject Wall(Coord position) => new GameObject(position, layer: 0, parentObject: null, isStatic: true, isWalkable: false, isTransparent: false);


        public static GameObject Floor(Coord position) => new GameObject(position, layer: 0, parentObject: null, isStatic: true, isWalkable: true, isTransparent: true);
    }
}
