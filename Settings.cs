using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Konscious.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GMC
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }


        public static byte[] GenerateSalt()
        {
            // 16 bytes es el estándar recomendado (128 bits)
            byte[] salt = new byte[16];

            // Llena el array con bytes aleatorios criptográficamente seguros
            RandomNumberGenerator.Fill(salt);

            return salt;
        }

        private byte[] KDF(string s, byte[] salt)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(s)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8; // Número de hilos
                argon2.MemorySize = 65536;     // 64 MB de RAM
                argon2.Iterations = 4;         // Número de pasadas

                return argon2.GetBytes(256 / 8);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(OldKTB.Text.Length == 0)
            {
                MessageBox.Show("Migration passphrase can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Bancos de claves GMC (*.gmck)|*.gmck";
            openFileDialog1.ShowDialog(this);

            if (openFileDialog1.FileName.Length == 0 || !File.Exists(openFileDialog1.FileName))
                return;

            var res = MessageBox.Show("Overwrite or keep matching keys? (Yes = Overwrite. No = Keep)", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (res == DialogResult.Cancel)
                return;

            byte[]? MasterKey = UMC_UI_1.GetMasterKey();
            if(MasterKey == null)
            {
                MessageBox.Show("This device's Master key could not be found. Can't decypher", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<byte> local = new();
            byte[] LocalRaw = UMC_UI_1.GetKeyBank();

            var local_r = (LocalRaw.Length > 0) ? CypherCapsule.Decypher(LocalRaw, local, MasterKey) : CypherReturnCode.OK;

            List<byte> foreign = new();
            List<byte> fore_raw = [.. File.ReadAllBytes(openFileDialog1.FileName)];
            if (fore_raw.Count == 0)
                return;
            if(fore_raw.Count < 16)
            {
                MessageBox.Show("Malformed input file", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] salt = fore_raw.Take(16).ToArray();
            fore_raw.RemoveRange(0, 16);
            var foreign_r = CypherCapsule.Decypher(fore_raw.ToArray(), foreign, KDF(OldKTB.Text, salt));

            if(!(local_r == CypherReturnCode.OK && foreign_r == CypherReturnCode.OK))
            {
                MessageBox.Show("Cryptographic error", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Dictionary<string, string> keyValuePairs = new();

            string loc_s = Encoding.ASCII.GetString(local.ToArray());
            string fore_s = Encoding.ASCII.GetString(foreign.ToArray());

            if(loc_s.Length > 0)
                foreach(string s in loc_s.Split('\n'))
                {
                    if (!s.Contains('\t'))
                        continue;
                    string[] sp = s.Split('\t');
                    if (sp.Length < 2)
                        continue;

                    keyValuePairs.Add(sp[0], sp[1]);
                }

            foreach (string s in fore_s.Split('\n'))
            {
                if (!s.Contains('\t'))
                    continue;
                string[] sp = s.Split('\t');
                if (sp.Length < 2)
                    continue;

                if(keyValuePairs.ContainsKey(sp[0]))
                    if(res == DialogResult.Yes)
                        keyValuePairs[sp[0]] = sp[1];
                    else
                        continue;
                else
                    keyValuePairs.Add(sp[0], sp[1]);
            }

            string outString = "";

            foreach(KeyValuePair<string, string> pair in keyValuePairs)
            {
                outString += $"{pair.Key}\t{pair.Value}\n";
            }

            List<byte> outBytes = new();

            var c_res = CypherCapsule.Cypher(Encoding.ASCII.GetBytes(outString), outBytes, MasterKey);

            if(c_res != CypherReturnCode.OK)
            {
                MessageBox.Show("Cryptographic error 2", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UMC_UI_1.SaveKeyBank(outBytes.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var res = MessageBox.Show("Are you sure you want to delete this bank?\n (This action cannot be undone)", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (res == DialogResult.No)
                return;

            UMC_UI_1.DeleteKeyBank();
            MessageBox.Show("Bank deleted successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (OldKTB.Text.Length == 0)
            {
                MessageBox.Show("Migration passphrase can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Bancos de claves GMC (*.gmck)|*.gmck";
            saveFileDialog1.ShowDialog(this);

            if (saveFileDialog1.FileName.Length == 0 || !File.Exists(saveFileDialog1.FileName))
                return;

            byte[]? MasterKey = UMC_UI_1.GetMasterKey();
            if (MasterKey == null)
            {
                MessageBox.Show("This device's Master key could not be found. Can't decypher", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<byte> local = new();
            byte[] localRaw = UMC_UI_1.GetKeyBank();
            if(localRaw.Length == 0)
            {
                MessageBox.Show("Local key bank is empty. Nothing to export", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var local_r = CypherCapsule.Decypher(File.ReadAllBytes(Application.CommonAppDataPath + $@"\KeyBank.gmck"), local, MasterKey);

            List<byte> outBytes = new();

            var c_res = CypherCapsule.Cypher(local.ToArray(), outBytes, KDF(OldKTB.Text, GenerateSalt()));

            if (c_res != CypherReturnCode.OK)
            {
                MessageBox.Show("Cryptographic error 2", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            File.WriteAllBytes(saveFileDialog1.FileName, outBytes.ToArray());
        }
    }
}
