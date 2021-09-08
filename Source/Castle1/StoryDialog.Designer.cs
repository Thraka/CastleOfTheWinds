
namespace CastleOfTheWinds
{
    partial class StoryDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.doneButton = new System.Windows.Forms.Button();
            this.storyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // doneButton
            // 
            this.doneButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.doneButton.Location = new System.Drawing.Point(207, 335);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(140, 23);
            this.doneButton.TabIndex = 0;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            // 
            // storyTextBox
            // 
            this.storyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storyTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.storyTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.storyTextBox.Location = new System.Drawing.Point(13, 13);
            this.storyTextBox.Multiline = true;
            this.storyTextBox.Name = "storyTextBox";
            this.storyTextBox.Size = new System.Drawing.Size(529, 310);
            this.storyTextBox.TabIndex = 1;
            // 
            // StoryDialog
            // 
            this.AcceptButton = this.doneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.doneButton;
            this.ClientSize = new System.Drawing.Size(550, 360);
            this.ControlBox = false;
            this.Controls.Add(this.storyTextBox);
            this.Controls.Add(this.doneButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StoryDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.TextBox storyTextBox;
    }
}