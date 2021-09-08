using GoRogue;

namespace CastleOfTheWinds.Maps
{
    public delegate void WalkTrigger(Game game, CastleMap? oldMap, Coord oldPosition);
    public delegate void CollisionTrigger(Game game, CastleMap? oldMap, Coord oldPosition);
}