namespace Aquarium
{
    partial class Aquarium
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
            this.components = new System.ComponentModel.Container();
            this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenuAddFish = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenuFeedEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cMenuEx = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cMenu
            // 
            this.cMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenuAddFish,
            this.cMenuFeedEnable,
            this.toolStripSeparator1,
            this.cMenuEx});
            this.cMenu.Name = "cMenu";
            this.cMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cMenu.Size = new System.Drawing.Size(230, 76);
            // 
            // cMenuAddFish
            // 
            this.cMenuAddFish.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.cMenuAddFish.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cMenuAddFish.ForeColor = System.Drawing.Color.Blue;
            this.cMenuAddFish.Name = "cMenuAddFish";
            this.cMenuAddFish.Size = new System.Drawing.Size(229, 22);
            this.cMenuAddFish.Text = "Запустить ещё одну рыбку";
            this.cMenuAddFish.Click += new System.EventHandler(this.cMenuNewFish);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem2.Text = "1";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.f1);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem3.Text = "2";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.f2);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem4.Text = "3";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.f3);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem5.Text = "4";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.f4);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(81, 22);
            this.toolStripMenuItem6.Text = "5";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.f5);
            // 
            // cMenuFeedEnable
            // 
            this.cMenuFeedEnable.Checked = true;
            this.cMenuFeedEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cMenuFeedEnable.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cMenuFeedEnable.Name = "cMenuFeedEnable";
            this.cMenuFeedEnable.Size = new System.Drawing.Size(229, 22);
            this.cMenuFeedEnable.Text = "Взять корм";
            this.cMenuFeedEnable.Click += new System.EventHandler(this.cMenuEnableFeeding);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
            // 
            // cMenuEx
            // 
            this.cMenuEx.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cMenuEx.ForeColor = System.Drawing.Color.Red;
            this.cMenuEx.Name = "cMenuEx";
            this.cMenuEx.Size = new System.Drawing.Size(229, 22);
            this.cMenuEx.Text = "Выход";
            this.cMenuEx.Click += new System.EventHandler(this.cMenuExit);
            // 
            // trayIcon
            // 
            this.trayIcon.Text = "Рыболюция!";
            this.trayIcon.Visible = true;
            // 
            // timer
            // 
            this.timer.Interval = 17;
            this.timer.Tick += new System.EventHandler(this.UpdateFishies);
            // 
            // Aquarium
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 220);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.Name = "Aquarium";
            this.ShowInTaskbar = false;
            this.Text = "FISH\'O\'LUTION";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Aquarium_FormClosed);
            this.cMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cMenu;
        private System.Windows.Forms.ToolStripMenuItem cMenuAddFish;
        private System.Windows.Forms.ToolStripMenuItem cMenuFeedEnable;
        private System.Windows.Forms.ToolStripMenuItem cMenuEx;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
    }
}