using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace CastleOfTheWinds.Maps.Static
{
    public class RoughTrail : CastleMap
    {
        public RoughTrail() : base("A Rough Trail", 50, 43)
        {
            AddTerrain(Resources.ReadMap($"{Name}.txt"));

            AddTerrainObject((44, 32), "the smoking remains of a building", "/terrain/village_hut_left_burning.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((10, 41), "The hamlet gate", "/terrain/gate.png", isWalkable: false, width: 3, height: 1);
            AddSceneryObject((25, 1), "the entrance to an abandoned mine", "/scenery/mine.png", isWalkable: true);
            AddSceneryObject((44, 30), "A trampled garden", "/scenery/garden_broken.png", isWalkable: true);

            Terrain[(25, 1)].AddWalkTrigger((game, oldMap, _) =>
            {
                if (oldMap == this)
                {
                    game.ChangeMap("Mine Level 0", (10, 38));
                }
            });

            Terrain[(11, 41)].AddWalkTrigger((game, oldMap, _) =>
            {
                if (oldMap == this)
                {
                    game.ChangeMap("A Tiny Hamlet", (13, 1));
                }
            });

            Terrain[(25, 0)].AddCollissionTrigger((game, _, _) => game.ChangeMap("Mine Level 0", (10, 38)));
            Terrain[(11, 42)].AddCollissionTrigger((game, _, _) => game.ChangeMap("A Tiny Hamlet", (13, 1)));
            Terrain[(0, 24)].AddCollissionTrigger((game, _, _) => game.TriggerStory("Highway, Looking West"));
            Terrain[(0, 25)].AddCollissionTrigger((game, _, _) => game.TriggerStory("Highway, Looking West"));
            Terrain[(49, 24)].AddCollissionTrigger((game, _, _) => game.TriggerStory("Highway, Looking East"));
            Terrain[(49, 25)].AddCollissionTrigger((game, _, _) => game.TriggerStory("Highway, Looking East"));
            Terrain[(25, 26)].AddWalkTrigger((game, _, _) => game.TriggerStory("Neglected highway"));
            Terrain[(44, 33)].AddWalkTrigger((game, _, _) => game.TriggerStory("Burning farm"));

            foreach (var position in this.Positions())
            {
                Explored[position] = true;
            }
        }
    }
}