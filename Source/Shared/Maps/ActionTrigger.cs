using System;

namespace CastleOfTheWinds.Maps
{
    internal class ActionTrigger
    {
        public Func<Game, bool> Action { get; }

        public ActionTrigger(Action<Game> action)
        {
            Action = game =>
            {
                action(game);
                return true;
            };
        }
    
        public ActionTrigger(Func<Game, bool> action)
        {
            Action = action;
        }
    }
}