using GoRogue.GameFramework;

namespace CastleOfTheWinds.Maps.Static
{
    public static class Extensions
    {
        public static T SetWalkable<T>(this T gameObject, bool walkable = true) where T : IGameObject
        {
            gameObject.IsWalkable = walkable;
            return gameObject;
        }

        public static T AddWalkTrigger<T>(this T terrainObject, WalkTrigger trigger) where T : IGameObject
        {
            terrainObject.IsWalkable = true;
            terrainObject.AddComponent(trigger);
            return terrainObject;
        }

        public static T AddCollissionTrigger<T>(this T terrainObject, CollisionTrigger trigger) where T : IGameObject
        {
            terrainObject.AddComponent(trigger);
            return terrainObject;
        }
    }
}