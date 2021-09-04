using GoRogue.GameFramework;
using GoRogue;
using System;
using System.Collections.Generic;

namespace CastleOfTheWinds
{
    public class Game
    {
        public Game()
        {
            Logs = new List<string>();
            Map = FixedMaps.Village;
            Player = new Creature(new Coord(13, 17), description: "you", imagePath: "/creatures/human_male");

            Map.AddEntity(Player);
        }

        public Map Map { get; private set; }

        public Creature Player { get; private set; }

        public List<string> Logs { get; private set; }

        public event EventHandler<string> MessageLogged;

        public event EventHandler MapUpdated;

        public void ProcessTurn(PlayerCommand playerCommand)
        {
            Action playerAction = playerCommand switch
            {
                PlayerCommand.MoveUp => () => MoveOrAttack(Direction.UP),
                PlayerCommand.MoveDown => () => MoveOrAttack(Direction.DOWN),
                PlayerCommand.MoveLeft => () => MoveOrAttack(Direction.LEFT),
                PlayerCommand.MoveRight => () => MoveOrAttack(Direction.RIGHT),
                PlayerCommand.MoveUpLeft => () => MoveOrAttack(Direction.UP_LEFT),
                PlayerCommand.MoveUpRight => () => MoveOrAttack(Direction.UP_RIGHT),
                PlayerCommand.MoveDownLeft => () => MoveOrAttack(Direction.DOWN_LEFT, hasShift),
                PlayerCommand.MoveDownRight => () => MoveOrAttack(Direction.DOWN_RIGHT, hasShift),
                _ => throw new ArgumentException("Unrecognized PlayerCommand")
            };

            if (playerAction == null)
            {
                return false;
            }

            ProcessTurn(playerAction);
        }

        private void ProcessTurn(Action playerAction)
        {
            UpdateMap();
        }

        private void UpdateMap()
        {
            Map.CalculateFOV(Player.Position, 1);
            MapUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void MoveOrAttack(Direction direction, bool sprint)
        {
            bool collision;

            while(true)
            {
                collision = !Player.MoveIn(direction);

                if (collision)
                {
                    break
                }
                
                if(collision || !sprint)
                {
                    break;
                }
            }       
            
            if(collision)
            {
                Log("Collision");
            }            
        }

        private void Log(string message)
        {
            Logs.Add(message);
            MessageLogged?.Invoke(this, message);
        }
    }
}
