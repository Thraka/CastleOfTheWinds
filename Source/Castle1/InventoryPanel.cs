using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CastleOfTheWinds
{
    public partial class InventoryPanel : UserControl
    {
        private readonly Game _game;

        public InventoryPanel(Game game)
        {
            _game = game;
            InitializeComponent();
        }
    }
}
