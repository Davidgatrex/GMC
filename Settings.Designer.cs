namespace GMC
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            pictureBox1 = new PictureBox();
            button1 = new Button();
            openFileDialog1 = new OpenFileDialog();
            label1 = new Label();
            OldKTB = new TextBox();
            button2 = new Button();
            button3 = new Button();
            saveFileDialog1 = new SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.GMC_BIG;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(162, 147);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(180, 12);
            button1.Name = "button1";
            button1.Size = new Size(114, 23);
            button1.TabIndex = 1;
            button1.Text = "Import key bank";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(180, 38);
            label1.Name = "label1";
            label1.Size = new Size(120, 15);
            label1.TabIndex = 2;
            label1.Text = "Migration Passphrase";
            // 
            // OldKTB
            // 
            OldKTB.Location = new Point(180, 56);
            OldKTB.Name = "OldKTB";
            OldKTB.Size = new Size(234, 23);
            OldKTB.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(322, 85);
            button2.Name = "button2";
            button2.Size = new Size(92, 23);
            button2.TabIndex = 7;
            button2.Text = "Delete bank";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(300, 12);
            button3.Name = "button3";
            button3.Size = new Size(114, 23);
            button3.TabIndex = 8;
            button3.Text = "Export key bank";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(426, 170);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(OldKTB);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Settings";
            Text = "Settings";
            Load += Settings_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private OpenFileDialog openFileDialog1;
        private Label label1;
        private TextBox OldKTB;
        private Button button2;
        private Button button3;
        private SaveFileDialog saveFileDialog1;
    }
}