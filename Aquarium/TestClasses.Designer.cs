namespace Aquarium
{
    partial class TestClasses
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.загрузитьИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.графическийОбъектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getBitmapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ResizeCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаГраницfalseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isDraggedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isCollidingWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.машинаСостоянийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние5ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние5ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.состояние5ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(19, 19);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьИзображениеToolStripMenuItem,
            this.gameObjectToolStripMenuItem,
            this.fishToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(444, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // загрузитьИзображениеToolStripMenuItem
            // 
            this.загрузитьИзображениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.графическийОбъектToolStripMenuItem,
            this.getBitmapToolStripMenuItem,
            this.getBitmapToolStripMenuItem1,
            this.ResizeCheck,
            this.проверкаГраницfalseToolStripMenuItem,
            this.isDraggedToolStripMenuItem});
            this.загрузитьИзображениеToolStripMenuItem.Name = "загрузитьИзображениеToolStripMenuItem";
            this.загрузитьИзображениеToolStripMenuItem.Size = new System.Drawing.Size(118, 24);
            this.загрузитьИзображениеToolStripMenuItem.Text = "GraphicObject";
            // 
            // графическийОбъектToolStripMenuItem
            // 
            this.графическийОбъектToolStripMenuItem.Name = "графическийОбъектToolStripMenuItem";
            this.графическийОбъектToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.графическийОбъектToolStripMenuItem.Text = "GetBitmap (FileNotFoundException)";
            this.графическийОбъектToolStripMenuItem.Click += new System.EventHandler(this.GetBitmap1ToolStripMenuItem_Click);
            // 
            // getBitmapToolStripMenuItem
            // 
            this.getBitmapToolStripMenuItem.Name = "getBitmapToolStripMenuItem";
            this.getBitmapToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.getBitmapToolStripMenuItem.Text = "GetBitmap (FileLoadException)";
            this.getBitmapToolStripMenuItem.Click += new System.EventHandler(this.getBitmapToolStripMenuItem_Click);
            // 
            // getBitmapToolStripMenuItem1
            // 
            this.getBitmapToolStripMenuItem1.Name = "getBitmapToolStripMenuItem1";
            this.getBitmapToolStripMenuItem1.Size = new System.Drawing.Size(330, 26);
            this.getBitmapToolStripMenuItem1.Text = "GetBitmap (ArgumentException)";
            this.getBitmapToolStripMenuItem1.Click += new System.EventHandler(this.getBitmapToolStripMenuItem1_Click);
            // 
            // ResizeCheck
            // 
            this.ResizeCheck.Name = "ResizeCheck";
            this.ResizeCheck.Size = new System.Drawing.Size(330, 26);
            this.ResizeCheck.Text = "BitmapResize";
            this.ResizeCheck.Click += new System.EventHandler(this.ResizeCheckClick);
            // 
            // проверкаГраницfalseToolStripMenuItem
            // 
            this.проверкаГраницfalseToolStripMenuItem.Name = "проверкаГраницfalseToolStripMenuItem";
            this.проверкаГраницfalseToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.проверкаГраницfalseToolStripMenuItem.Text = "Проверка границ";
            this.проверкаГраницfalseToolStripMenuItem.Click += new System.EventHandler(this.проверкаГраницfalseToolStripMenuItem_Click);
            // 
            // isDraggedToolStripMenuItem
            // 
            this.isDraggedToolStripMenuItem.Name = "isDraggedToolStripMenuItem";
            this.isDraggedToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.isDraggedToolStripMenuItem.Text = "IsDragged";
            this.isDraggedToolStripMenuItem.Click += new System.EventHandler(this.isDraggedToolStripMenuItem_Click);
            // 
            // gameObjectToolStripMenuItem
            // 
            this.gameObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.isCollidingWithToolStripMenuItem,
            this.updateToolStripMenuItem});
            this.gameObjectToolStripMenuItem.Name = "gameObjectToolStripMenuItem";
            this.gameObjectToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.gameObjectToolStripMenuItem.Text = "GameObject";
            // 
            // isCollidingWithToolStripMenuItem
            // 
            this.isCollidingWithToolStripMenuItem.Name = "isCollidingWithToolStripMenuItem";
            this.isCollidingWithToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.isCollidingWithToolStripMenuItem.Text = "IsCollidingWith";
            this.isCollidingWithToolStripMenuItem.Click += new System.EventHandler(this.isCollidingWithToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // fishToolStripMenuItem
            // 
            this.fishToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.машинаСостоянийToolStripMenuItem,
            this.updateToolStripMenuItem1});
            this.fishToolStripMenuItem.Name = "fishToolStripMenuItem";
            this.fishToolStripMenuItem.Size = new System.Drawing.Size(48, 24);
            this.fishToolStripMenuItem.Text = "Fish";
            // 
            // машинаСостоянийToolStripMenuItem
            // 
            this.машинаСостоянийToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.состояние0ToolStripMenuItem,
            this.состояние1ToolStripMenuItem,
            this.состояние5ToolStripMenuItem,
            this.состояние5ToolStripMenuItem1,
            this.состояние5ToolStripMenuItem2,
            this.состояние5ToolStripMenuItem3});
            this.машинаСостоянийToolStripMenuItem.Name = "машинаСостоянийToolStripMenuItem";
            this.машинаСостоянийToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
            this.машинаСостоянийToolStripMenuItem.Text = "Машина состояний";
            // 
            // состояние0ToolStripMenuItem
            // 
            this.состояние0ToolStripMenuItem.Name = "состояние0ToolStripMenuItem";
            this.состояние0ToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.состояние0ToolStripMenuItem.Text = "Состояние 0";
            this.состояние0ToolStripMenuItem.Click += new System.EventHandler(this.состояние0ToolStripMenuItem_Click);
            // 
            // состояние1ToolStripMenuItem
            // 
            this.состояние1ToolStripMenuItem.Name = "состояние1ToolStripMenuItem";
            this.состояние1ToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.состояние1ToolStripMenuItem.Text = "Состояние 5";
            this.состояние1ToolStripMenuItem.Click += new System.EventHandler(this.состояние1ToolStripMenuItem_Click);
            // 
            // состояние5ToolStripMenuItem
            // 
            this.состояние5ToolStripMenuItem.Name = "состояние5ToolStripMenuItem";
            this.состояние5ToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.состояние5ToolStripMenuItem.Text = "Состояние 10";
            this.состояние5ToolStripMenuItem.Click += new System.EventHandler(this.состояние5ToolStripMenuItem_Click);
            // 
            // состояние5ToolStripMenuItem1
            // 
            this.состояние5ToolStripMenuItem1.Name = "состояние5ToolStripMenuItem1";
            this.состояние5ToolStripMenuItem1.Size = new System.Drawing.Size(185, 26);
            this.состояние5ToolStripMenuItem1.Text = "Состояние 11";
            this.состояние5ToolStripMenuItem1.Click += new System.EventHandler(this.состояние5ToolStripMenuItem1_Click);
            // 
            // состояние5ToolStripMenuItem2
            // 
            this.состояние5ToolStripMenuItem2.Name = "состояние5ToolStripMenuItem2";
            this.состояние5ToolStripMenuItem2.Size = new System.Drawing.Size(185, 26);
            this.состояние5ToolStripMenuItem2.Text = "Состояние 15";
            this.состояние5ToolStripMenuItem2.Click += new System.EventHandler(this.состояние5ToolStripMenuItem2_Click);
            // 
            // состояние5ToolStripMenuItem3
            // 
            this.состояние5ToolStripMenuItem3.Name = "состояние5ToolStripMenuItem3";
            this.состояние5ToolStripMenuItem3.Size = new System.Drawing.Size(185, 26);
            this.состояние5ToolStripMenuItem3.Text = "Состояние 51";
            this.состояние5ToolStripMenuItem3.Click += new System.EventHandler(this.состояние5ToolStripMenuItem3_Click);
            // 
            // updateToolStripMenuItem1
            // 
            this.updateToolStripMenuItem1.Name = "updateToolStripMenuItem1";
            this.updateToolStripMenuItem1.Size = new System.Drawing.Size(227, 26);
            this.updateToolStripMenuItem1.Text = "Update";
            this.updateToolStripMenuItem1.Click += new System.EventHandler(this.updateToolStripMenuItem1_Click);
            // 
            // TestClasses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 109);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TestClasses";
            this.Text = "Тестирование классов";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem загрузитьИзображениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem графическийОбъектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getBitmapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getBitmapToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ResizeCheck;
        private System.Windows.Forms.ToolStripMenuItem проверкаГраницfalseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isDraggedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isCollidingWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem машинаСостоянийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem состояние0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem состояние1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem состояние5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem состояние5ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem состояние5ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem состояние5ToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem1;
    }
}