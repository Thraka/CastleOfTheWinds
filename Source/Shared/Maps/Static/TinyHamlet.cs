using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace CastleOfTheWinds.Maps.Static
{
    public class TinyHamlet : CastleMap
    {
        public TinyHamlet() : base("A Tiny Hamlet", 27, 28)
        {
            AddTerrain(Resources.ReadMap($"{Name}.txt"));

            AddTerrainObject((5, 6), "A farm", "/terrain/village_hut_right.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((8, 17), "Bjorn the Blacksmith", "/terrain/village_hut_right.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((9, 13), "The home of the village sage", "/terrain/village_sage.png", isWalkable: false, width: 2, height: 2);
            AddTerrainObject((11, 22), "A shrine to Odin", "/terrain/village_shrine.png", isWalkable: false, width: 5, height: 5);
            AddTerrainObject((12, 0), "The hamlet gate", "/terrain/gate.png", isWalkable: false, width: 3, height: 1);
            AddTerrainObject((16, 12), "Olaf's Junk Store", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((16, 17), "Gunhild's General Store", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            AddTerrainObject((18, 5), "A farm", "/terrain/village_hut_left.png", isWalkable: false, width: 3, height: 3);
            
            AddSceneryObject((7, 5), "A vegetable garden", "/scenery/garden.png", isWalkable: true);
            AddSceneryObject((7, 9), "A wagon", "/scenery/wagon.png", isWalkable: true);
            AddSceneryObject((11, 14), "A sign that says:\nSnorri the Sage", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((11, 17), "A  sign that says:\nBjorn the Blacksmith", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((13, 18), "the village well", "/scenery/well.png", isWalkable: false);
            AddSceneryObject((14, 21), "A sign that says:\nShrine of Odin", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((15, 12), "A sign that says:\nOlaf's Junk Store", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((15, 17), "A sign that says:\nGunhild's General Store", "/scenery/sign.png", isWalkable: true);
            AddSceneryObject((17, 4), "A vegetable garden", "/scenery/garden.png", isWalkable: true);

            // Fix the shrine entrance and farm doors to be walkable
            Terrain[(18, 6)].IsWalkable = true;
            Terrain[(7, 7)].IsWalkable = true;
            Terrain[(13, 22)].IsWalkable = true;

            foreach (var position in this.Positions())
            {
                Explored[position] = true;
            }
        }

        public override bool BeforeObjectMove(Game game, CastleObject entity, Map toMap, Coord toCoordinates)
        {
            if (entity != game.Player)
            {
                return true;
            }

            if (toCoordinates == (13, -1))
            {
                game.ChangeMap("A Rough Trail", (10, 40));
                return false;
            }

            if (toCoordinates == (10, 13))
            {
                game.LoadStore("Hamlet Sage");
                return false;
            }

            if (toCoordinates == (16, 13))
            {
                game.LoadStore("Hamlet Junk Store");
                return false;
            }

            if (toCoordinates == (10, 18))
            {
                game.LoadStore("Hamlet Blacksmith");
                return false;
            }

            if (toCoordinates == (16, 18))
            {
                game.LoadStore("Hamlet General Store");
                return false;
            }

            return true;
        }

        public override void AfterObjectMove(Game game, CastleObject entity, Map fromMap, Coord fromCoordinates)
        {
            if (entity != game.Player)
            {
                return;
            }

            if (entity.Position == (13, -1))
            {
                game.ChangeMap("A Rough Trail", (10, 40));
            }

            if (entity.Position == (18, 6))
            {
                game.TriggerStory("Locked farm door");
            }

            if (entity.Position == (7, 7))
            {
                game.TriggerStory("Locked farm door");
            }

            if (entity.Position == (13, 22))
            {
                game.LoadStore("Hamlet Shrine");
                entity.MoveIn(Direction.UP);
            }
        }
    }
}