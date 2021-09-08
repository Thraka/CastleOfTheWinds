using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace CastleOfTheWinds.Maps.Static
{
    public class RoughTrail : CastleMap
    {
        public RoughTrail() : base("A Rough Trail", 48, 41)
        {
            AddTerrain(Resources.ReadMap($"{Name}.txt"));

            AddTerrainObject((43, 31), "the smoking remains of a building", "/terrain/village_hut_left_burning.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((9, 40), "The hamlet gate", "/terrain/gate.png", isWalkable: false, width: 3, height: 1);
            AddSceneryObject((24, 0), "the entrance to an abandoned mine", "/scenery/mine.png", isWalkable: true);
            AddSceneryObject((43, 30), "A trampled garden", "/scenery/garden_broken.png", isWalkable: true);

            // Fix the gate and farm door to be walkable
            Terrain[(43, 32)].IsWalkable = true;
            Terrain[(10, 40)].IsWalkable = true;

            foreach (var position in this.Positions())
            {
                Explored[position] = true;
            }
        }

        public override bool BeforeObjectMove(Game game, CastleObject entity, Map toMap, Coord toCoordinates)
        {
            if (entity == game.Player)
            {
                if (toCoordinates == (24, -1))
                {
                    game.ChangeMap("Mine Level 0", (13, 0));
                    return true;
                }

                if (toCoordinates == (10, 41))
                {
                    game.ChangeMap("A Tiny Hamlet", (13, 0));
                    return true;
                }

                if (toCoordinates.X == -1 && toCoordinates.Y is 23 or 24)
                {
                    game.TriggerStory("Highway, Looking West");
                }

                if (toCoordinates.X == 48 && toCoordinates.Y is 23 or 24)
                {
                    game.TriggerStory("Highway, Looking East");
                }
            }


            return false;
        }

        public override void AfterObjectMove(Game game, CastleObject entity, Map fromMap, Coord fromCoordinates)
        {
            if (entity == game.Player)
            {
                if (fromMap == this && entity.Position == (10, 40))
                {
                    game.ChangeMap("A Tiny Hamlet", (13, 0));
                }

                if (fromMap == this && entity.Position == (24, 0))
                {
                    game.ChangeMap("Mine Level 0", (13, 0));
                }

                if (entity.Position == (24, 25))
                {
                    game.TriggerStory("Neglected highway");
                }

                if (entity.Position == (43, 32))
                {
                    game.TriggerStory("Burning farm");
                }
            }
        }
    }
}