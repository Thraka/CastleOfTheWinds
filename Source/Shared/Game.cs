using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using CastleOfTheWinds.Maps;
using CastleOfTheWinds.Maps.Static;
using CastleOfTheWinds.Objects;
using GoRogue.GameFramework;
using GoRogue.MapViews;

namespace CastleOfTheWinds
{
    public class Game
    {
        private static readonly TimeSpan TickDuration = TimeSpan.FromSeconds(2.5);
        private int _currentTick;
        private readonly Dictionary<string, CastleMap> _maps;
        private readonly Dictionary<string, StoryScene> _storyScenes;
        private readonly List<string> _triggeredScenes;

        public Game()
        {
            _triggeredScenes = new();
            _storyScenes = new()
            {
                ["Locked farm door"] = new StoryScene
                {
                    OnlyPlayOnce = false,
                    PlayOnLog = true,
                    Pages = new[] { "The door to the farm is locked." }
                },
                ["Highway, Looking East"] = new StoryScene
                {
                    OnlyPlayOnce = true,
                    PlayOnLog = true,
                    Pages = new[] { "You look at the highway vanishing in the distance, and feel no need to continue." }
                },
                ["Highway, Looking West"] = new StoryScene
                {
                    OnlyPlayOnce = true,
                    PlayOnLog = true,
                    Pages = new[] { "You look at the highway vanishing in the distance, and feel no need to continue." }
                },
                ["Enter a rough trail"] = new StoryScene
                {
                    OnlyPlayOnce = true,
                    PlayOnLog = true,
                    Pages = new[] { "You see a badly neglected highway, and a trail leading into the mountains." }
                },
                ["Burning farm"] = new StoryScene
                {
                    OnlyPlayOnce = true,
                    PlayOnLog = false,
                    Pages = new[]{ Resources.ReadText("Burning farm.txt") }
                },
                ["Neglected highway"] = new StoryScene
                {
                    OnlyPlayOnce = true,
                    PlayOnLog = true,
                    Pages = new [] { "You see a badly neglected highway, and a trail leading into the mountains." }
                },
            };

            Logs = new List<string>();

            Player = new Creature((1, 1), description: "you", imagePath: "/creatures/human_male.png")
            {
                Mana = 7,
                ManaMax = 7,
                HitPoints = 50,
                HitPointsMax = 50,
                Speed = 100,
                SpeedMax = 200
            };

            Player.Moved += HandlePlayerMoved;

            _maps = new CastleMap[]
                {
                    new TinyHamlet(),
                    new RoughTrail(),
                    new MineLevel0()
                }
                .ToDictionary(x => x.Name);

            Map = ChangeMap("A Tiny Hamlet", (13, 18));
        }

        private void HandlePlayerMoved(object? sender, ItemMovedEventArgs<IGameObject> eventArgs)
        {
            var map = Player.CurrentMap;

            Trace(Player.Position.ToString());

            if (map == null)
            {
                return;
            }

            map.CalculateFOV(Player.Position, 1);
        }

        public CastleMap Map { get; private set; }

        public Creature Player { get; }

        public List<string> Logs { get; }

        public TimeSpan GameTime => TickDuration * _currentTick;

        public event EventHandler<string>? MessageLogged;

        public event EventHandler<string>? StoryProgressed;

        public event EventHandler? StateChanged;

        public bool MoveOrAttack(Direction direction)
        {
            if (MoveAndTick(direction))
            {
                return true;
            }

            if (!Map.Contains(Player.Position + direction))
            {
                return false;
            }

            if (AttackAndTick(direction))
            {
                return true;
            }

            // we can't move that way, and we can't attack something in that direction. Why are we blocked?
            var blockingObject = Map.GetObjects<CastleObject>(Player.Position + direction)
                .Reverse()
                .FirstOrDefault(x => !x.IsWalkable);

            if (blockingObject != null)
            {
                Trace($"Blocked by {blockingObject.Description}");
            }
            else
            {
                Error("Unexpected inability to MoveOrAttack");
            }
            
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

                    if (!targetTerrain.IsWalkable)
                    {
                        Trace($"Blocked by {targetTerrain.Description}");
                    }
                    else
                    {
                        Error("Unexpected inability to Attack after sprint");
                    }
                }
            }

            // sprinting is only successful if there is some movement involved
            return moved;
        }

        internal CastleMap ChangeMap(string mapName, Coord position)
        {
            if (!_maps.TryGetValue(mapName, out var map))
            {
                throw new Exception($"Unknown map name {mapName}");
            }

            MoveEntity(Player, map, position);
            
            Map = map;

            Map.CalculateFOV(Player.Position, 1);

            TriggerStateChanged();

            return map;
        }

        internal void TriggerStory(string sceneName)
        {
            if (!_storyScenes.TryGetValue(sceneName, out var scene))
            {
                Error($"Unknown story scene {sceneName}");
            }

            if (_triggeredScenes.Contains(sceneName) && scene!.OnlyPlayOnce)
            {
                return;
            }

            foreach (var storyPage in scene!.Pages)
            {
                if (scene.PlayOnLog)
                {
                    Log(storyPage);
                }
                else
                {
                    StoryProgressed?.Invoke(this, storyPage);
                }
            }

            _triggeredScenes.Add(sceneName);
        }

        internal void LoadStore(string storeName)
        {
            Log($"Entering store {storeName}");
        }

        private bool MoveAndTick(Direction direction)
        {
            if (!MoveEntity(Player, Map, Player.Position + direction))
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

        private bool MoveEntity(CastleObject entity, CastleMap toMap, Coord toCoordinates)
        {
            var fromMap = (CastleMap)entity.CurrentMap;
            var fromCoordinates = entity.Position;

            if (!toMap.Contains(toCoordinates))
            {
                return false;
            }

            if (!toMap.WalkabilityView[toCoordinates])
            {

                if (entity == Player)
                {
                    var collisionTrigger = toMap.Terrain[toCoordinates].GetComponent<CollisionTrigger>();

                    if (collisionTrigger != null)
                    {
                        collisionTrigger.Invoke(this, fromMap, fromCoordinates);
                        return true;
                    }
                }

                return false;
            }

            if (toMap != fromMap)
            {
                // before removing the entity from its current map, make sure it can be moved to the new map
                fromMap?.RemoveEntity(entity);
                entity.Position = toCoordinates;
                toMap.AddEntity(entity);
            }
            else
            {
                entity.Position = toCoordinates;
            }

            TriggerStateChanged();

            if (entity == Player)
            {
                toMap.CalculateFOV(Player.Position, 1);
                var walkTrigger = toMap.Terrain[toCoordinates].GetComponent<WalkTrigger>();
                walkTrigger?.Invoke(this, fromMap, fromCoordinates);
            }

            return true;
        }

        private void TriggerStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Error(string message) => Log($"Error: {message}");

        private void Trace(string message) => Log($"Trace: {message}");

        private void Log(string message)
        {
            Logs.Add(message);
            MessageLogged?.Invoke(this, message);
        }
    }

    internal class StoryScene
    {
        public bool OnlyPlayOnce { get; set; }
        public bool PlayOnLog { get; set; }
        public string[] Pages { get; set; } = Array.Empty<string>();
    }
}
