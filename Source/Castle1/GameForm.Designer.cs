
namespace CastleOfTheWinds
{
    partial class GameForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verbsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.characterToolStripMenuItem,
            this.inventoryToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.spellsToolStripMenuItem,
            this.activateToolStripMenuItem,
            this.verbsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(943, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // characterToolStripMenuItem
            // 
            this.characterToolStripMenuItem.Name = "characterToolStripMenuItem";
            this.characterToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.characterToolStripMenuItem.Text = "Character!";
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.inventoryToolStripMenuItem.Text = "Inventory!";
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mapToolStripMenuItem.Text = "Map!";
            // 
            // spellsToolStripMenuItem
            // 
            this.spellsToolStripMenuItem.Name = "spellsToolStripMenuItem";
            this.spellsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.spellsToolStripMenuItem.Text = "Spells!";
            // 
            // activateToolStripMenuItem
            // 
            this.activateToolStripMenuItem.Name = "activateToolStripMenuItem";
            this.activateToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.activateToolStripMenuItem.Text = "Activate";
            // 
            // verbsToolStripMenuItem
            // 
            this.verbsToolStripMenuItem.Name = "verbsToolStripMenuItem";
            this.verbsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.verbsToolStripMenuItem.Text = "Verbs";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 548);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameForm";
            this.Text = "Castle of the Winds";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spellsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verbsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

