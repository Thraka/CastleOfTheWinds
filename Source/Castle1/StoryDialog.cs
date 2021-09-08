using System.Windows.Forms;

namespace CastleOfTheWinds
{
    public partial class StoryDialog : Form
    {
        public StoryDialog(string text)
        {
            InitializeComponent();
            this.storyTextBox.Text = text;
        }
    }
}
