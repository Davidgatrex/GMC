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
            DecypherCHK = new CheckBox();
            RunButton = new Button();
            button1 = new Button();
            SettingsButton = new Button();
            DelStorKey = new Button();
            ExchangeBTN = new Button();
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
            // DecypherCHK
            // 
            DecypherCHK.AutoSize = true;
            DecypherCHK.Location = new Point(16, 218);
            DecypherCHK.Name = "DecypherCHK";
            DecypherCHK.Size = new Size(76, 19);
            DecypherCHK.TabIndex = 11;
            DecypherCHK.Text = "Decypher";
            DecypherCHK.UseVisualStyleBackColor = true;
            DecypherCHK.CheckedChanged += DecypherCHK_CheckedChanged;
            // 
            // RunButton
            // 
            RunButton.Location = new Point(12, 243);
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
            button1.Location = new Point(164, 218);
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
            // DelStorKey
            // 
            DelStorKey.BackColor = Color.LightCoral;
            DelStorKey.FlatAppearance.BorderColor = Color.Yellow;
            DelStorKey.FlatAppearance.MouseDownBackColor = Color.Maroon;
            DelStorKey.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 0, 0);
            DelStorKey.FlatStyle = FlatStyle.Flat;
            DelStorKey.Location = new Point(12, 189);
            DelStorKey.Name = "DelStorKey";
            DelStorKey.Size = new Size(171, 23);
            DelStorKey.TabIndex = 17;
            DelStorKey.Text = "Delete stored key";
            DelStorKey.UseVisualStyleBackColor = false;
            DelStorKey.Click += DelStorKey_Click;
            // 
            // ExchangeBTN
            // 
            ExchangeBTN.Location = new Point(67, 131);
            ExchangeBTN.Name = "ExchangeBTN";
            ExchangeBTN.Size = new Size(75, 23);
            ExchangeBTN.TabIndex = 18;
            ExchangeBTN.Text = "Exchange";
            ExchangeBTN.UseVisualStyleBackColor = true;
            ExchangeBTN.Click += ExchangeBTN_Click;
            // 
            // UMC_UI_1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 224, 192);
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(251, 274);
            Controls.Add(ExchangeBTN);
            Controls.Add(DelStorKey);
            Controls.Add(SettingsButton);
            Controls.Add(button1);
            Controls.Add(RunButton);
            Controls.Add(DecypherCHK);
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
        private CheckBox DecypherCHK;
        private Button RunButton;
        private Button button1;
        private Button SettingsButton;
        private Button DelStorKey;
        private Button ExchangeBTN;
    }
}
