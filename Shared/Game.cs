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
            Map = FixedMaps.Village;
            Player = new CastleObject(MapLayers.Creatures, (13, 17), description: "you", imagePath: "/creatures/human_male", parentObject: null, isStatic: false, isWalkable: false, isTransparent: true);
            if (!Map.AddEntity(Player))
            {
                var existing = Map.GetEntities<IGameObject>(Player.Position);
            }
            Map.CalculateFOV(Player.Position, 1);

            Logs = new List<string>();

            Player.Moved += (s, e) => Map.CalculateFOV(Player.Position, 1);
        }

        public Map Map { get; private set; }

        public GameObject Player { get; private set; }

        public List<string> Logs { get; private set; } = new();

        public event EventHandler<string> MessageLogged;

        public bool ProcessCommand(InputCommand inputCommand, bool hasShift, bool hasControl, bool hasAlt)
        {
            switch(inputCommand)
            {
                case InputCommand.MoveUp:
                    MovePlayer(Direction.UP, hasShift);
                    return true;
                case InputCommand.MoveDown:
                    MovePlayer(Direction.DOWN, hasShift);
                    return true;
                case InputCommand.MoveLeft:
                    MovePlayer(Direction.LEFT, hasShift);
                    return true;
                case InputCommand.MoveRight:
                    MovePlayer(Direction.RIGHT, hasShift);
                    return true;
                case InputCommand.MoveUpLeft:
                    MovePlayer(Direction.UP_LEFT, hasShift);
                    return true;
                case InputCommand.MoveUpRight:
                    MovePlayer(Direction.UP_RIGHT, hasShift);
                    return true;
                case InputCommand.MoveDownLeft:
                    MovePlayer(Direction.DOWN_LEFT, hasShift);
                    return true;
                case InputCommand.MoveDownRight:
                    MovePlayer(Direction.DOWN_RIGHT, hasShift);
                    return true;
                default:
                    return false;
            }
        }

        public void MovePlayer(Direction direction, bool sprint)
        {
            bool collision;

            while(true)
            {
                collision = !Player.MoveIn(direction);
                
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
