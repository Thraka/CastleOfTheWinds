namespace CastleOfTheWinds
{
    public abstract class GameAction
    {
        public abstract int Duration { get; }
        
        public abstract bool Repeat { get; }

        public abstract bool StartAction(Game game);

        public abstract void CompleteAction(Game game);
    }
}