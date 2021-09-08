using System;
using GoRogue;
using GoRogue.MapViews;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CastleOfTheWinds.Maps;
using CastleOfTheWinds.Objects;
using Rectangle = System.Drawing.Rectangle;

namespace CastleOfTheWinds
{
    public partial class GameForm : Form
    {
        private const int PixelsPerTile = 32;
        
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

        public GameForm(Game game)
        {
            InitializeComponent();

            this.picture.Top = 0;
            this.picture.Left = 0;

            Game = game;
            Player = game.Player;

            Game.StateChanged += (_, _) => ProcessStateChange();
            Game.MessageLogged += (_, _) => UpdateLog();
            Game.StoryProgressed += (_, message) => new StoryDialog(message).ShowDialog(this);

            ProcessStateChange();
        }

        public Game Game { get; }

        public Creature Player { get; }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (KeyCommands.TryGetValue(keyData, out var action))
            {
                action.Invoke(Game);
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
            this.richTextBox1.SelectionTabs = new[] { 100 };
            this.richTextBox1.Text = $"HP\t{Player.HitPoints} ({Player.HitPointsMax})\nMana\t{Player.Mana} ({Player.ManaMax})\nSpeed\t{Player.Speed}% / {Player.SpeedMax}%\nTime\t{Game.GameTime:d\\d\\,hh\\:mm\\:ss}\n{Game.Map.Name}";
        }

        private void UpdateLog()
        {
            this.logTextBox.Lines = Game.Logs.ToArray();
            this.logTextBox.SelectionStart = this.logTextBox.Text.Length;
            this.logTextBox.ScrollToCaret();
        }

        private void RedrawMap()
        {
            var map = Game.Map;
            this.picture.Width = PixelsPerTile * map.Width;
            this.picture.Height = PixelsPerTile * map.Width;
            var bitmap = new Bitmap(PixelsPerTile * map.Width, PixelsPerTile * map.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);

            PaintExposedTerrain(map, graphics, PixelsPerTile);
            PaintVisibleObjects(map, graphics, PixelsPerTile);

            this.picture.Image = bitmap;
        }

        private void EnsurePlayerVisible()
        {
            var scaledPlayerPosition = Player.Position * PixelsPerTile + this.picture.Location;

            var edgeRectangle = new Rectangle(
                scaledPlayerPosition.X - PixelsPerTile * 2,
                scaledPlayerPosition.Y - PixelsPerTile * 2,
                PixelsPerTile * 5,
                PixelsPerTile * 5
            );

            edgeRectangle.Intersect(this.picture.Bounds);

            var panel = (SplitterPanel)this.picture.Parent;

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
                this.picture.Location = Point.Add(this.picture.Location, shift);
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

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }
    }
}
