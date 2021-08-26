using GoRogue.GameFramework;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using System;
using System.Collections.Generic;

namespace CastleOfTheWinds
{
    public class Game
    {
        public Map Map { get; private set; }

        public GameObject Player { get; private set; }

        public List<string> Logs { get; private set; }

        public event EventHandler<string> MessageLogged;

        public void Initialize()
        {
            // We'll use GoRogue map generation to generate a simple rectangle map, with walls
            // around the edges and floor everywhere else, then translate to use our GameObjects.
            var terrainMap = new ArrayMap<bool>(80, 50);

            QuickGenerators.GenerateRectangleMap(terrainMap);

            Map = new Map(
                width: terrainMap.Width,
                height: terrainMap.Height,
                numberOfEntityLayers: 1,
                distanceMeasurement: Distance.CHEBYSHEV);

            foreach (var pos in terrainMap.Positions())
                if (terrainMap[pos]) // Floor
                    Map.SetTerrain(TerrainFactory.Floor(pos));
                else // Wall
                    Map.SetTerrain(TerrainFactory.Wall(pos));

            // Create the player at position (1, 1) - just inside the outer walls
            Player = EntityFactory.Player((1, 1));

            Map.AddEntity(Player);

            Logs = new List<string>();

            Player.Moved += (s,e) => Map.CalculateFOV(Player.Position, 1);
        }

        internal bool ProcessCommand(InputCommand inputCommand, bool hasShift, bool hasControl, bool hasAlt)
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

        private void MovePlayer(Direction direction, bool sprint)
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
