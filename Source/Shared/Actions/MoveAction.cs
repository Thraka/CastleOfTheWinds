using GoRogue;

namespace CastleOfTheWinds
{
    public class MoveAction : GameAction
    {
        private readonly Creature _creature;
        private readonly Direction _direction;

        public MoveAction(Creature creature, Direction direction, bool sprint)
        {
            _creature = creature;
            _direction = direction;
            Repeat = sprint;
        }
        
        public override int Duration => 1;

        public override bool Repeat { get; }

        public override bool StartAction(Game game)
        {
            return _creature.MoveIn(_direction);
        }

        public override void CompleteAction(Game game)
        {
        }
    }

    public class SearchAction : GameAction
    {
        private readonly Coord _position;

        public SearchAction(Coord position)
        {
            _position = position;
        }

        public override int Duration => 1;

        public override bool Repeat => false;

        public override bool StartAction(Game game)
        {
        }

        public override void CompleteAction(Game game)
        {
        }
    }
}