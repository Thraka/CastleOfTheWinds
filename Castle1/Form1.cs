using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CastleOfTheWinds;
using GoRogue;
using GoRogue.GameFramework;

namespace Castle1
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<Keys, InputCommand> keyMap = new();

        public Game Game { get; }
        public Map Map { get; }
        public GameObject Player { get; }

        public Form1(Game game)
        {
            InitializeComponent();

            Game = game;
            Map = game.Map;
            Player = game.Player;

            Map.ObjectMoved += Map_ObjectMoved;
            Game.MessageLogged += (sender, message) => Log(message);
           
            keyMap[Keys.Up] = InputCommand.MoveUp;
            keyMap[Keys.Down] = InputCommand.MoveDown;
            keyMap[Keys.Left] = InputCommand.MoveLeft;
            keyMap[Keys.Right] = InputCommand.MoveRight;
            keyMap[Keys.Home] = InputCommand.MoveUpLeft;
            keyMap[Keys.PageDown] = InputCommand.MoveUpRight;
            keyMap[Keys.End] = InputCommand.MoveDownLeft;
            keyMap[Keys.Next] = InputCommand.MoveDownRight;
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

        private void Map_ObjectMoved(object? sender, ItemMovedEventArgs<IGameObject> eventArgs)
        {
            Log($"ObjectMoved: {eventArgs.Item.ID} from {eventArgs.OldPosition} to {eventArgs.NewPosition}");
        }

        private void Log(string message)
        {
            this.logTextBox.AppendText($"\r\n{message}");
        }

        private void RedrawMap()
        {
            var scale = 32;
            this.picture.Width = scale * Map.Width;
            this.picture.Height = scale * Map.Height;
            Map.CalculateFOV(Player.Position, 1);

            var wallImage = Bitmap.FromStream(new MemoryStream(Bitmaps._2));

            var graphics = this.picture.CreateGraphics();
            graphics.Clear(Color.White);
            graphics.DrawImage()
            foreach (var coord in Map.FOV.CurrentFOV)
            {
               
            }
        }
    }
}
