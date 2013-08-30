namespace CharmControlLibrary
{
    partial class CharmMenu
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
            this.SuspendLayout();
            // 
            // CharmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 343);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CharmMenu";
            this.Text = "CharmMenu";
            this.Deactivate += new System.EventHandler(this.CharmMenu_Deactivate);
            this.VisibleChanged += new System.EventHandler(this.CharmMenu_VisibleChanged);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CharmMenu_MouseClick);
            this.MouseLeave += new System.EventHandler(this.CharmMenu_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CharmMenu_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}