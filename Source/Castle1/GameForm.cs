using System.Windows.Forms;
using CastleOfTheWinds.Objects;

namespace CastleOfTheWinds
{
    public partial class GameForm : Form
    {
        private readonly Game _game;
        private readonly DungeonPanel _dungeonPanel;
        private readonly InventoryPanel _inventoryPanel;

        public GameForm(Game game)
        {
            _game = game;

            InitializeComponent();

            Controls.Add(_dungeonPanel = new DungeonPanel(game));
            _dungeonPanel.Dock = DockStyle.Fill;
            
            Controls.Add(_inventoryPanel = new InventoryPanel(game));
            _inventoryPanel.Dock = DockStyle.Fill;
            _inventoryPanel.Visible = false;
        }
    }
}
