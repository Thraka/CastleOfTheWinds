using System;
using System.Windows.Forms;

namespace CastleOfTheWinds
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var game = new Game();

            Application.Run(new GameForm(game));
        }
    }
}
