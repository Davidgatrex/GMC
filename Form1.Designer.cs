namespace GMC
{
    partial class UMC_UI_1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UMC_UI_1));
            label1 = new Label();
            label2 = new Label();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            InputTB = new TextBox();
            label3 = new Label();
            InputBTN = new Button();
            OutputBTN = new Button();
            label4 = new Label();
            OutputTB = new TextBox();
            label5 = new Label();
            KeyTB = new TextBox();
            DecypherCHK = new CheckBox();
            RoundCountNUD = new NumericUpDown();
            label6 = new Label();
            RunButton = new Button();
            button1 = new Button();
            SettingsButton = new Button();
            button2 = new Button();
            label7 = new Label();
            MKTB = new TextBox();
            SaveKeyBTN = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)RoundCountNUD).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Impact", 30F);
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(93, 48);
            label1.TabIndex = 0;
            label1.Text = "GMC";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 56);
            label2.Name = "label2";
            label2.Size = new Size(171, 16);
            label2.TabIndex = 1;
            label2.Text = "Generally Mambo Cyphering";
            label2.Click += label2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.FileOk += openFileDialog1_FileOk;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "gmc";
            saveFileDialog1.FileOk += saveFileDialog1_FileOk;
            // 
            // InputTB
            // 
            InputTB.Location = new Point(12, 103);
            InputTB.Name = "InputTB";
            InputTB.Size = new Size(171, 23);
            InputTB.TabIndex = 2;
            InputTB.TextChanged += InputTB_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 84);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 3;
            label3.Text = "Input";
            // 
            // InputBTN
            // 
            InputBTN.Location = new Point(189, 103);
            InputBTN.Name = "InputBTN";
            InputBTN.Size = new Size(53, 23);
            InputBTN.TabIndex = 4;
            InputBTN.Text = "Select";
            InputBTN.UseVisualStyleBackColor = true;
            InputBTN.Click += InputBTN_Click;
            // 
            // OutputBTN
            // 
            OutputBTN.Location = new Point(189, 160);
            OutputBTN.Name = "OutputBTN";
            OutputBTN.Size = new Size(53, 23);
            OutputBTN.TabIndex = 7;
            OutputBTN.Text = "Select";
            OutputBTN.UseVisualStyleBackColor = true;
            OutputBTN.Click += OutputBTN_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 141);
            label4.Name = "label4";
            label4.Size = new Size(45, 15);
            label4.TabIndex = 6;
            label4.Text = "Output";
            // 
            // OutputTB
            // 
            OutputTB.Location = new Point(12, 160);
            OutputTB.Name = "OutputTB";
            OutputTB.Size = new Size(171, 23);
            OutputTB.TabIndex = 5;
            OutputTB.TextChanged += OutputTB_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 195);
            label5.Name = "label5";
            label5.Size = new Size(26, 15);
            label5.TabIndex = 9;
            label5.Text = "Key";
            // 
            // KeyTB
            // 
            KeyTB.Location = new Point(12, 214);
            KeyTB.Name = "KeyTB";
            KeyTB.Size = new Size(171, 23);
            KeyTB.TabIndex = 8;
            KeyTB.TextChanged += KeyTB_TextChanged;
            // 
            // DecypherCHK
            // 
            DecypherCHK.AutoSize = true;
            DecypherCHK.Location = new Point(16, 332);
            DecypherCHK.Name = "DecypherCHK";
            DecypherCHK.Size = new Size(76, 19);
            DecypherCHK.TabIndex = 11;
            DecypherCHK.Text = "Decypher";
            DecypherCHK.UseVisualStyleBackColor = true;
            DecypherCHK.CheckedChanged += DecypherCHK_CheckedChanged;
            // 
            // RoundCountNUD
            // 
            RoundCountNUD.Location = new Point(12, 303);
            RoundCountNUD.Name = "RoundCountNUD";
            RoundCountNUD.Size = new Size(120, 23);
            RoundCountNUD.TabIndex = 12;
            RoundCountNUD.Value = new decimal(new int[] { 15, 0, 0, 0 });
            RoundCountNUD.ValueChanged += RoundCountNUD_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 285);
            label6.Name = "label6";
            label6.Size = new Size(47, 15);
            label6.TabIndex = 13;
            label6.Text = "Rounds";
            // 
            // RunButton
            // 
            RunButton.Location = new Point(12, 357);
            RunButton.Name = "RunButton";
            RunButton.Size = new Size(75, 23);
            RunButton.TabIndex = 14;
            RunButton.Text = "Run!";
            RunButton.UseVisualStyleBackColor = true;
            RunButton.Click += RunButton_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.FlatAppearance.BorderColor = Color.Yellow;
            button1.FlatAppearance.MouseDownBackColor = Color.Maroon;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 0, 0);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(164, 332);
            button1.Name = "button1";
            button1.Size = new Size(75, 48);
            button1.TabIndex = 15;
            button1.Text = "Destroy Original";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // SettingsButton
            // 
            SettingsButton.Location = new Point(159, 12);
            SettingsButton.Name = "SettingsButton";
            SettingsButton.Size = new Size(75, 23);
            SettingsButton.TabIndex = 16;
            SettingsButton.Text = "Settings";
            SettingsButton.UseVisualStyleBackColor = true;
            SettingsButton.Click += SettingsButton_Click;
            // 
            // button2
            // 
            button2.Location = new Point(189, 203);
            button2.Name = "button2";
            button2.Size = new Size(53, 23);
            button2.TabIndex = 17;
            button2.Text = "Find";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 240);
            label7.Name = "label7";
            label7.Size = new Size(64, 15);
            label7.TabIndex = 19;
            label7.Text = "Master key";
            // 
            // MKTB
            // 
            MKTB.Location = new Point(12, 259);
            MKTB.Name = "MKTB";
            MKTB.Size = new Size(171, 23);
            MKTB.TabIndex = 18;
            // 
            // SaveKeyBTN
            // 
            SaveKeyBTN.Location = new Point(189, 232);
            SaveKeyBTN.Name = "SaveKeyBTN";
            SaveKeyBTN.Size = new Size(53, 23);
            SaveKeyBTN.TabIndex = 20;
            SaveKeyBTN.Text = "Save";
            SaveKeyBTN.UseVisualStyleBackColor = true;
            SaveKeyBTN.Click += SaveKeyBTN_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(255, 128, 128);
            button3.FlatAppearance.BorderColor = Color.Yellow;
            button3.FlatAppearance.MouseDownBackColor = Color.FromArgb(64, 0, 0);
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 0, 0);
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(189, 261);
            button3.Name = "button3";
            button3.Size = new Size(53, 23);
            button3.TabIndex = 21;
            button3.Text = "Delete";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // UMC_UI_1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(251, 392);
            Controls.Add(button3);
            Controls.Add(SaveKeyBTN);
            Controls.Add(label7);
            Controls.Add(MKTB);
            Controls.Add(button2);
            Controls.Add(SettingsButton);
            Controls.Add(button1);
            Controls.Add(RunButton);
            Controls.Add(label6);
            Controls.Add(RoundCountNUD);
            Controls.Add(DecypherCHK);
            Controls.Add(label5);
            Controls.Add(KeyTB);
            Controls.Add(OutputBTN);
            Controls.Add(label4);
            Controls.Add(OutputTB);
            Controls.Add(InputBTN);
            Controls.Add(label3);
            Controls.Add(InputTB);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "UMC_UI_1";
            Text = "GMC UI";
            ((System.ComponentModel.ISupportInitialize)RoundCountNUD).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private TextBox InputTB;
        private Label label3;
        private Button InputBTN;
        private Button OutputBTN;
        private Label label4;
        private TextBox OutputTB;
        private Label label5;
        private TextBox KeyTB;
        private CheckBox DecypherCHK;
        private NumericUpDown RoundCountNUD;
        private Label label6;
        private Button RunButton;
        private Button button1;
        private Button SettingsButton;
        private Button button2;
        private Label label7;
        private TextBox MKTB;
        private Button SaveKeyBTN;
        private Button button3;
    }
}
