using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.MapViews;

namespace CastleOfTheWinds
{
    public class Game
    {
        private static readonly TimeSpan TickDuration = TimeSpan.FromSeconds(2.5);
        private int _currentTick;

        public Game()
        {
            Logs = new List<string>();

            Map = FixedMaps.Village;

            Player = new Creature((13, 17), description: "you", imagePath: "/creatures/human_male")
            {
                Mana = 7,
                ManaMax = 7,
                HitPoints = 50,
                HitPointsMax = 50,
                Speed = 100,
                SpeedMax = 200
            };

            Map.AddEntity(Player);
            Map.CalculateFOV(Player.Position, 1);
        }

        public CastleMap Map { get; }

        public Creature Player { get; }

        public List<string> Logs { get; }

        public TimeSpan GameTime => TickDuration * _currentTick;

        public event EventHandler<string>? MessageLogged;

        public event EventHandler? StateChanged;


        public bool MoveOrAttack(Direction direction)
        {
            if (!Map.Contains(Player.Position + direction))
            {
                // moving outside the map
                return false;
            }
            
            if (MoveAndTick(direction))
            {
                return true;
            }

            if (AttackAndTick(direction))
            {
                return true;
            }
            
            // we can't move that way, and we can't attack something in that direction. Why are we blocked?
            var blockingObject = Map.GetObjects<CastleObject>(Player.Position + direction)
                .Reverse()
                .FirstOrDefault(x => !x.IsWalkable);
            
            Log(blockingObject != null
                ? $"Blocked by {blockingObject.Description}"
                : "Error: Unexpected inability to MoveOrAttack");

            return false;
        }

        public bool Sprint(Direction direction)
        {
            var moved = false;

            while (MoveAndTick(direction))
            {
                moved = true;
                // We were able to move to a new tile.
                // See if we need to stop here for some reason.
                //
                // if(specialTile) {
                //   return true;
                // }
            }

            if (moved)
            {
                // collided with something
                // Try to kill it :)
                if (!AttackAndTick(direction))
                {
                    // nothing there. Sanity check blocking terrain...
                    // we can't move that way, and we can't attack something in that direction. Why are we blocked?
                    var targetTerrain = Map.GetTerrain<CastleObject>(Player.Position + direction);

                    Log(!targetTerrain.IsWalkable
                        ? $"Blocked by {targetTerrain.Description}"
                        : "Error: Unexpected inability to MoveOrAttack");
                }
            }

            // sprinting is only successful if there is some movement involved
            return moved;
        }


        private bool MoveAndTick(Direction direction)
        {
            if (!Player.MoveIn(direction)) 
                return false;

            ProcessTick();
            return true;
        }

        private bool AttackAndTick(Direction direction)
        {
            if (!Map.Contains(Player.Position + direction))
            {
                // Attacking outside the map
                return false;
            }

            var targetCreature = Map.GetObjects<Creature>(Player.Position + direction)
                .FirstOrDefault();

            if (targetCreature == null)
                return false;

            // Do some damage, maybe kill

            ProcessTick();
            return true;
        }

        private void ProcessTick()
        {
            // TickCreatures();

            _currentTick += 1;
            Map.CalculateFOV(Player.Position, 1);
            StateChanged?.Invoke(this, EventArgs.Empty);
        }


        private void Log(string message)
        {
            Logs.Add(message);
            MessageLogged?.Invoke(this, message);
        }
    }
}
