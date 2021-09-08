using System;
using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.MapViews;

namespace CastleOfTheWinds.Maps.Static
{
    public class TinyHamlet : CastleMap
    {
        public TinyHamlet() : base("A Tiny Hamlet", 27, 29)
        {
            AddTerrain(Resources.ReadMap($"{Name}.txt"));

            AddTerrainObject((12, 1), "The hamlet gate", "/terrain/gate.png", isWalkable: false, width: 3, height: 1);
            AddTerrainObject((18, 6), "A farm", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((5, 7), "A farm", "/terrain/village_hut_right.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((16, 13), "Olaf's Junk Store", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((9, 14), "The home of the village sage", "/terrain/village_sage.png", isWalkable: false, width: 2, height: 2);
            AddTerrainObject((8, 18), "Bjorn the Blacksmith", "/terrain/village_hut_right.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((16, 18), "Gunhild's General Store", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((11, 23), "A shrine to Odin", "/terrain/village_shrine.png", isWalkable: false, width: 5, height: 5);

            AddSceneryObject((17, 5), "A vegetable garden", "/scenery/garden.png", isWalkable: true);
            AddSceneryObject((7, 6), "A vegetable garden", "/scenery/garden.png", isWalkable: true);
            AddSceneryObject((7, 10), "A wagon", "/scenery/wagon.png", isWalkable: true);
            AddSceneryObject((15, 13), "A sign that says:\nOlaf's Junk Store", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((11, 15), "A sign that says:\nSnorri the Sage", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((11, 18), "A  sign that says:\nBjorn the Blacksmith", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((15, 18), "A sign that says:\nGunhild's General Store", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((13, 19), "the village well", "/scenery/well.png", isWalkable: false);
            AddSceneryObject((14, 22), "A sign that says:\nShrine of Odin", "/scenery/sign.png", isWalkable: true);

            // Fix the shrine entrance, gate and farm doors to be walkable
            Terrain[(13, 0)].AddCollissionTrigger((game, _, _) => game.ChangeMap("A Rough Trail", (11, 41)));

            Terrain[(13, 1)].AddWalkTrigger((game, oldMap, _) =>
            {
                if (oldMap == this)
                {
                    game.ChangeMap("A Rough Trail", (11, 41));
                }
            });

            Terrain[(18, 7)].AddWalkTrigger((game, _, _) => game.TriggerStory("Locked farm door"));
            Terrain[(7, 8)].AddWalkTrigger((game, _, _) => game.TriggerStory("Locked farm door"));

            Terrain[(13, 23)].AddWalkTrigger((game, _, oldPosition) =>
            {
                game.LoadStore("Hamlet Shrine");
                game.Player.Position = oldPosition;
            });

            Terrain[(10, 14)].AddCollissionTrigger((game, _, _) => game.LoadStore("Hamlet Sage"));
            Terrain[(16, 14)].AddCollissionTrigger((game, _, _) => game.LoadStore("Hamlet Junk Store"));
            Terrain[(10, 19)].AddCollissionTrigger((game, _, _) => game.LoadStore("Hamlet Blacksmith"));
            Terrain[(16, 19)].AddCollissionTrigger((game, _, _) => game.LoadStore("Hamlet General Store"));

            foreach (var position in this.Positions())
            {
                Explored[position] = true;
            }
        }
    }
}