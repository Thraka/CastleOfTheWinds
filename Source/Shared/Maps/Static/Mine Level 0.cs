namespace CastleOfTheWinds.Maps.Static
{
    public class MineLevel0 : CastleMap
    {
        public MineLevel0() : base("Mine Level 0", 25, 40)
        {
            AddTerrain(Resources.ReadMap($"{Name}.txt"), allExplored: false);
        }
    }
}