using System;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows.Forms;
using Rectangle = System.Drawing.Rectangle;

namespace CastleOfTheWinds
{
    public partial class GameForm : Form
    {
        private readonly Dictionary<Keys, InputCommand> keyMap = new();
        private int scale = 32;

        public Game Game { get; }
        public Map Map { get; }
        public GameObject Player { get; }

        public GameForm(Game game)
        {
            InitializeComponent();



            Game = game;
            Map = game.Map;
            Player = game.Player;

            Player.Moved += PlayerMoved;
            Game.MessageLogged += (sender, message) => Log(message);
           
            keyMap[Keys.Up] = InputCommand.MoveUp;
            keyMap[Keys.Down] = InputCommand.MoveDown;
            keyMap[Keys.Left] = InputCommand.MoveLeft;
            keyMap[Keys.Right] = InputCommand.MoveRight;
            keyMap[Keys.Home] = InputCommand.MoveUpLeft;
            keyMap[Keys.PageDown] = InputCommand.MoveUpRight;
            keyMap[Keys.End] = InputCommand.MoveDownLeft;
            keyMap[Keys.Next] = InputCommand.MoveDownRight;

            this.picture.Top = 0;
            this.picture.Left = 0;
            this.picture.Width = scale * Map.Width;
            this.picture.Height = scale * Map.Width;

            RedrawMap();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var hasShift = keyData.HasFlag(Keys.Shift);
            var hasControl = keyData.HasFlag(Keys.Control);
            var hasAlt = keyData.HasFlag(Keys.Alt);

            var unmodifiedKey = keyData;
            if(hasShift) unmodifiedKey -= Keys.Shift;
            if(hasControl) unmodifiedKey -= Keys.Control;
            if(hasAlt) unmodifiedKey -= Keys.Alt;

            if (keyMap.TryGetValue(unmodifiedKey, out var inputCommand) && Game.ProcessCommand(inputCommand, hasShift, hasControl, hasAlt))
            {
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PlayerMoved(object? sender, ItemMovedEventArgs<IGameObject> eventArgs)
        {
            RedrawMap();
            EnsurePlayerVisible();
        }

        private void Log(string message)
        {
            this.logTextBox.AppendText($"\r\n{message}");
        }

        private void RedrawMap()
        {
            var bitmap = new Bitmap(scale * Map.Width, scale * Map.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);

            PaintExposedTerrain(graphics, scale);
            PaintVisibleObjects(graphics, scale);

            this.picture.Image = bitmap;
        }

        private void EnsurePlayerVisible()
        {
            var scaledPlayerPosition = Player.Position * scale + this.picture.Location;

            var edgeRectangle = new Rectangle(
                scaledPlayerPosition.X - scale * 2,
                scaledPlayerPosition.Y - scale * 2,
                scale * 5,
                scale * 5
            );

            edgeRectangle.Intersect(this.picture.Bounds);

            var panel = (SplitterPanel) this.picture.Parent;

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

        private void PaintExposedTerrain(Graphics graphics, int scale)
        {
            foreach (var position in Map.Terrain.Positions())
            {
                if (Map.Explored[position])
                {
                    if(Map.Terrain[position] is not TerrainObject terrainObject)
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
        private void PaintVisibleObjects(Graphics graphics, int scale)
        {
            var sizeOne = new Size(1, 1);
            var sceneryMask = Map.LayerMasker.Mask(MapLayers.Scenery);
            var itemsMask = Map.LayerMasker.Mask(MapLayers.Items);
            var creaturesMask = Map.LayerMasker.Mask(MapLayers.Creatures);

            foreach (var position in Map.Positions())
            {
                if (Map.Explored[position])
                {
                    var scenery = Map.GetObjects<CastleObject>(position, sceneryMask).LastOrDefault();

                    if(scenery != null)
                    {
                        var image = scenery.GetImage();
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }

                    var items = Map.GetObjects<CastleObject>(position, itemsMask).ToArray();

                    if (items.Length == 1)
                    {
                        var image = items[0].GetImage();
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }
                    else if(items.Length > 1)
                    {
                        var image = Bitmaps.Read("/items/pile");
                        if (image != null)
                            PaintObject(graphics, image, position, sizeOne, scale);
                    }
                }

                if (Map.FOV.BooleanFOV[position])
                {
                    var creature = Map.GetObjects<CastleObject>(position, creaturesMask).SingleOrDefault();

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
