using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CastleOfTheWinds.Maps;
using CastleOfTheWinds.Objects;
using GoRogue;
using GoRogue.MapViews;
using Rectangle = System.Drawing.Rectangle;

namespace CastleOfTheWinds
{
    public partial class DungeonPanel : UserControl
    {
        private const int PixelsPerTile = 32;
        private CastleMap? _currentMap;

        private static readonly Dictionary<Keys, Action<Game>> KeyCommands = new()
        {
            [Keys.Shift | Keys.Up] = game => game.Sprint(Direction.UP),
            [Keys.Shift | Keys.Down] = game => game.Sprint(Direction.DOWN),
            [Keys.Shift | Keys.Left] = game => game.Sprint(Direction.LEFT),
            [Keys.Shift | Keys.Right] = game => game.Sprint(Direction.RIGHT),
            [Keys.Shift | Keys.Home] = game => game.Sprint(Direction.UP_LEFT),
            [Keys.Shift | Keys.PageUp] = game => game.Sprint(Direction.UP_RIGHT),
            [Keys.Shift | Keys.End] = game => game.Sprint(Direction.DOWN_LEFT),
            [Keys.Shift | Keys.PageDown] = game => game.Sprint(Direction.DOWN_RIGHT),
            [Keys.Up] = game => game.MoveOrAttack(Direction.UP),
            [Keys.Down] = game => game.MoveOrAttack(Direction.DOWN),
            [Keys.Left] = game => game.MoveOrAttack(Direction.LEFT),
            [Keys.Right] = game => game.MoveOrAttack(Direction.RIGHT),
            [Keys.Home] = game => game.MoveOrAttack(Direction.UP_LEFT),
            [Keys.PageUp] = game => game.MoveOrAttack(Direction.UP_RIGHT),
            [Keys.End] = game => game.MoveOrAttack(Direction.DOWN_LEFT),
            [Keys.PageDown] = game => game.MoveOrAttack(Direction.DOWN_RIGHT),
        };

        private readonly Game _game;

        public DungeonPanel(Game game)
        {
            _game = game;

            InitializeComponent();

            staticPicture.Controls.Add(dynamicPicture);
            dynamicPicture.Dock = DockStyle.Fill;
            dynamicPicture.BackColor = Color.Transparent;
            dynamicPicture.BringToFront();

            _game.StateChanged += (_, _) => ProcessStateChange();
            _game.MessageLogged += (_, _) => UpdateLog();
            _game.StoryProgressed += (_, message) => new StoryDialog(message).ShowDialog(this);

            ProcessStateChange();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (KeyCommands.TryGetValue(keyData, out var action))
            {
                action.Invoke(_game);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProcessStateChange()
        {
            RedrawMap();
            EnsurePlayerVisible();
            UpdateStats();
        }

        private void UpdateStats()
        {
            var player = _game.Player;
            var map = _game.Map;
            
            this.richTextBox1.SelectionTabs = new[] { 100 };
            this.richTextBox1.Text = $"HP\t{player.HitPoints} ({player.HitPointsMax})\nMana\t{player.Mana} ({player.ManaMax})\nSpeed\t{player.Speed}% / {player.SpeedMax}%\nTime\t{_game.GameTime:d\\d\\,hh\\:mm\\:ss}\n{map.Name}";
        }

        private void UpdateLog()
        {
            this.logTextBox.Lines = _game.Logs.ToArray();
            this.logTextBox.SelectionStart = this.logTextBox.Text.Length;
            this.logTextBox.ScrollToCaret();
        }
        
        private void RedrawMap()
        {
            var map = _game.Map;

            var bitmap = new Bitmap(PixelsPerTile * map.Width, PixelsPerTile * map.Height);
            var graphics = Graphics.FromImage(bitmap);

            if (map != _currentMap)
            {
                this.mapPanel.Width = PixelsPerTile * map.Width;
                this.mapPanel.Height = PixelsPerTile * map.Width;

                graphics.Clear(Color.White);
                PaintExposedTerrain(map, graphics, PixelsPerTile);
                this.staticPicture.Image = bitmap;

                bitmap = new Bitmap(PixelsPerTile * map.Width, PixelsPerTile * map.Height);
                graphics = Graphics.FromImage(bitmap);
                _currentMap = map;
            }

            graphics.Clear(Color.Transparent);
            PaintVisibleObjects(map, graphics, PixelsPerTile);
            this.dynamicPicture.Image = bitmap;
        }

        private void EnsurePlayerVisible()
        {
            var scaledPlayerPosition = _game.Player.Position * PixelsPerTile + this.mapPanel.Location;

            var edgeRectangle = new Rectangle(
                scaledPlayerPosition.X - PixelsPerTile * 2,
                scaledPlayerPosition.Y - PixelsPerTile * 2,
                PixelsPerTile * 5,
                PixelsPerTile * 5
            );

            edgeRectangle.Intersect(this.mapPanel.Bounds);

            var panel = (SplitterPanel)this.mapPanel.Parent;

            var shift = Size.Empty;
            if (edgeRectangle.X < panel.ClientRectangle.X)
            {
                shift = new Size(panel.ClientRectangle.X - edgeRectangle.X, shift.Height);
            }

            if (edgeRectangle.Y < panel.ClientRectangle.Y)
            {
                shift = new Size(shift.Width, panel.ClientRectangle.Y - edgeRectangle.Y);
            }

            if (panel.ClientRectangle.Right < edgeRectangle.Right)
            {
                shift = new Size(panel.ClientRectangle.Right - edgeRectangle.Right, shift.Height);
            }

            if (panel.ClientRectangle.Bottom < edgeRectangle.Bottom)
            {
                shift = new Size(shift.Width, panel.ClientRectangle.Bottom - edgeRectangle.Bottom);
            }

            if (shift != Size.Empty)
            {
                this.mapPanel.Location = Point.Add(this.mapPanel.Location, shift);
            }
        }

        private void PaintExposedTerrain(CastleMap map, Graphics graphics, int scale)
        {
            foreach (var position in map.Terrain.Positions())
            {
                if (map.Explored[position])
                {
                    if (map.Terrain[position] is not TerrainObject terrainObject)
                    {
                        continue;
                    }

                    var scaledSize = terrainObject.Size * scale;

                    var image = terrainObject.GetImage();

                    if (image != null)
                    {
                        graphics.DrawImage(image, position.X * scale, position.Y * scale, scaledSize.Width,
                            scaledSize.Height);
                    }
                }
            }
        }

        private void PaintVisibleObjects(CastleMap map, Graphics graphics, int scale)
        {
            var sizeOne = new Size(1, 1);
            var sceneryMask = map.LayerMasker.Mask(MapLayers.Scenery);
            var itemsMask = map.LayerMasker.Mask(MapLayers.Items);
            var creaturesMask = map.LayerMasker.Mask(MapLayers.Creatures);

            foreach (var position in map.Positions())
            {
                if (map.Explored[position])
                {
                    var scenery = map.GetObjects<CastleObject>(position, sceneryMask).LastOrDefault();

                    if (scenery != null)
                    {
                        var image = scenery.GetImage();
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }

                    var items = map.GetObjects<CastleObject>(position, itemsMask).ToArray();

                    if (items.Length == 1)
                    {
                        var image = items[0].GetImage();
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }
                    else if (items.Length > 1)
                    {
                        var image = Resources.ReadImage("/items/pile.png");
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }
                }

                if (map.FOV.BooleanFOV[position])
                {
                    var creature = map.GetObjects<CastleObject>(position, creaturesMask).SingleOrDefault();

                    if (creature != null)
                    {
                        var image = creature.GetImage();
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }
                }
            }
        }

        private static void PaintObject(Graphics graphics, Image image, Coord position, Size size, int scale)
        {
            graphics.DrawImage(image, position.X * scale, position.Y * scale, size.Width * scale, size.Height * scale);
        }
    }
}
