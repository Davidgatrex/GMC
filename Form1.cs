using System.Media;
using System.Reflection;
using System.Security.Cryptography;

namespace GMC
{
    public partial class UMC_UI_1 : Form
    {
        private string InPath;
        private string OutPath = "";
        private int roundCNT = 25;
        private string key = "PTyc@CWCb*";
        private bool Decypher_Internal = false;
        public UMC_UI_1(string defaultIn)
        {
            InPath = defaultIn;
            InitializeComponent();
            if (InPath.Contains(".gmc"))
            {
                DecypherCHK.Checked = true;
                Decypher_Internal = true;
            }
            openFileDialog1.FileName = InPath;
            InputTB.Text = InPath;

            if(true)
            {
                InPath = @"D:\CAM BOARD.pdf";
                openFileDialog1.FileName = InPath;
                InputTB.Text = InPath;
                OutPath = @"D:\CAM BOARD.pdf.gmc";
                saveFileDialog1.FileName = OutPath;
                OutputTB.Text = OutPath;
                KeyTB.Text = key;
                RoundCountNUD.Value = roundCNT;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void InputBTN_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                InPath = openFileDialog1.FileName;
                InputTB.Text = openFileDialog1.FileName;
            }
        }

        private void InputTB_TextChanged(object sender, EventArgs e)
        {
            InputTB.Text = InputTB.Text.Trim('\t');
            InPath = InputTB.Text;
        }

        private void OutputBTN_Click(object sender, EventArgs e)
        {
            if (Decypher_Internal)
            {
                saveFileDialog1.FileName = InPath.Replace(".gmc", "");
            }
            else
                saveFileDialog1.FileName = InPath + ".gmc";
            DialogResult res = saveFileDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                OutPath = saveFileDialog1.FileName;
                OutputTB.Text = saveFileDialog1.FileName;
            }
        }

        private void KeyTB_TextChanged(object sender, EventArgs e)
        {

            KeyTB.Text = KeyTB.Text.Trim('\t');
            key = KeyTB.Text;
        }

        private void RoundCountNUD_ValueChanged(object sender, EventArgs e)
        {
            roundCNT = (int)RoundCountNUD.Value;
        }

        private void DecypherCHK_CheckedChanged(object sender, EventArgs e)
        {
            Decypher_Internal = DecypherCHK.Checked;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (InPath.Length == 0)
            {
                MessageBox.Show("No input file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!File.Exists(InPath))
            {
                MessageBox.Show("Input file does not exist", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (OutPath.Length == 0)
            {
                MessageBox.Show("No output file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (roundCNT == 0)
            {
                MessageBox.Show("Round count can't be zero", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (key.Length == 0)
            {
                MessageBox.Show("Key must be not null", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            CypherCapsule.Main(System.Text.Encoding.ASCII.GetBytes(key), InPath, OutPath, Decypher_Internal, roundCNT);
            Cursor.Current = Cursors.Default;
        }

        private void OutputTB_TextChanged(object sender, EventArgs e)
        {
            OutputTB.Text = OutputTB.Text.Trim('\t');
            OutPath = OutputTB.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to destroy the original file?\n (This action cannot be undone)", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                return;
            if (InPath.Length == 0 || !File.Exists(InPath))
            {
                MessageBox.Show("Input file not selected or unexistant", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            DestroyFile(InPath);
        }

        private void DestroyFile(string path)
        {
            FileStream f = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            long sz = f.Length;

            byte[] buffer = new byte[sz];

            for (int i = 0; i < 10; i++)
            {
                f.Write(buffer);
                f.Flush();
            }

            f.Close();
            File.Delete(path);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        private string FindKey()
        {
            if (MKTB.Text.Length == 0)
            {
                MessageBox.Show("Master key must be not null", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return "";
            }
            if (InPath.Length == 0)
            {
                MessageBox.Show("No input file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return "";
            }
            string path = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(MKTB.Text)))}.gmck";
            if (!File.Exists(path))
            {
                MessageBox.Show("Unexistant key bank file", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return "";
            }

            byte[] raw = File.ReadAllBytes(path);
            byte[] dec = new byte[raw.Length];

            string TargetName = InPath.Split('\\').Last();

            CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(MKTB.Text), raw, ref dec, true, 15);
            string[] dec_s = System.Text.Encoding.ASCII.GetString(dec).Split('\n');
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 3)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    int.TryParse(sp[2], out roundCNT);
                    RoundCountNUD.Value = roundCNT;
                    return sp[1];
                }
            }

            return "";
        }

        private void SaveKey()
        {
            if (MKTB.Text.Length == 0)
            {
                MessageBox.Show("Master key must be not null", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (OutPath.Length == 0)
            {
                MessageBox.Show("No output file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (key.Length == 0)
            {
                MessageBox.Show("Key empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (roundCNT == 0)
            {
                MessageBox.Show("Rounds empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            string path = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(MKTB.Text)))}.gmck";
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }

            string[] dec_s;

            {
                byte[] raw = File.ReadAllBytes(path);
                byte[] dec = new byte[raw.Length];

                CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(MKTB.Text), raw, ref dec, true, 15);

                dec_s = System.Text.Encoding.ASCII.GetString(dec).Split('\n');
            }

            string TargetName = OutPath.Split('\\').Last();

            string Out = "";
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 3)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    var res = MessageBox.Show("This bank already has a key for a file with this name. Overwrite?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (res == DialogResult.No)
                        return;
                }
                else
                    Out += $"{sp[0]}\t{sp[1]}\t{sp[2]}\n";
            }

            Out += $"{TargetName}\t{key}\t{roundCNT}";

            byte[] enc = new byte[Out.Length];
            CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(MKTB.Text), System.Text.Encoding.ASCII.GetBytes(Out), ref enc, false, 15);

            File.WriteAllBytes(path, enc);
        }

        private void DeleteKey()
        {
            if (MKTB.Text.Length == 0)
            {
                MessageBox.Show("Master key must be not null", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (OutPath.Length == 0)
            {
                MessageBox.Show("No output file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if(key.Contains('\t'))
            {
                MessageBox.Show("Can't save a key with tabs", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (roundCNT == 0)
            {
                MessageBox.Show("Rounds empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            string path = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(MKTB.Text)))}.gmck";
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }

            string[] dec_s;

            {
                byte[] raw = File.ReadAllBytes(path);
                byte[] dec = new byte[raw.Length];

                CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(MKTB.Text), raw, ref dec, true, 15);

                dec_s = System.Text.Encoding.ASCII.GetString(dec).Split('\n');
            }

            string TargetName = OutPath.Split('\\').Last();

            string Out = "";
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 3)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    var res = MessageBox.Show("Key found in bank. Proceed with deletion?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (res == DialogResult.Yes)
                        continue;
                }
                Out += $"{sp[0]}\t{sp[1]}\t{sp[2]}\n";
            }

            byte[] enc = new byte[Out.Length];
            CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(MKTB.Text), System.Text.Encoding.ASCII.GetBytes(Out), ref enc, false, 15);

            File.WriteAllBytes(path, enc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string key_i = FindKey();
            Cursor.Current = Cursors.Default;
            if (key_i.Length == 0)
            {
                MessageBox.Show("Key not found", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                key = key_i;
                KeyTB.Text = key;
            }
        }

        private void SaveKeyBTN_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SaveKey();
            Cursor.Current = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteKey();
        }
    }
}
