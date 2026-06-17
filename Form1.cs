using System.Buffers.Text;
using System.Media;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GMC
{
    public partial class UMC_UI_1 : Form
    {
        private string InPath;
        private string OutPath = "";
        private bool Decypher_Internal = false;
        private const string KeyName = "GMC_MASTER_KEY";
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

            if (!RegUtils.ValuePresent(Microsoft.Win32.RegistryHive.CurrentUser, @"Software\GenerallyMambo\GMC", "MasterKeyB64"))
            {
                Aes aes = Aes.Create();
                aes.GenerateKey();
                RegUtils.WriteValue(Microsoft.Win32.RegistryHive.CurrentUser, @"Software\GenerallyMambo\GMC", "MasterKeyB64", Convert.ToBase64String(TPMCypher(aes.Key)));
                aes.Dispose();
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

            Cursor.Current = Cursors.WaitCursor;
            if (!Decypher_Internal)
            {
                byte[] Key;
                List<byte> Out = new();
                CypherCapsule.Cypher(File.ReadAllBytes(InPath), Out, out Key);
                if (SaveKey(Key))
                {
                    MessageBox.Show("Key Saved");
                }
                else
                {
                    MessageBox.Show("Couldn't save the key");
                }
                File.WriteAllBytes(OutPath, Out.ToArray());
            }
            else
            {
                byte[]? Key = FindKey();
                if (Key == null)
                {
                    MessageBox.Show("No stored key found for this file. Cannot decypher", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                List<byte> Out = new();
                CypherCapsule.Decypher(File.ReadAllBytes(InPath), Out, Key);
                File.WriteAllBytes(OutPath, Out.ToArray());
            }
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

        public static byte[]? GetMasterKey()
        {
            if (!RegUtils.ValuePresent(Microsoft.Win32.RegistryHive.CurrentUser, @"Software\GenerallyMambo\GMC", "MasterKeyB64"))
                return null;

            string MasterB64 = RegUtils.ReadValueString(Microsoft.Win32.RegistryHive.CurrentUser, @"Software\GenerallyMambo\GMC", "MasterKeyB64") ?? "";

            if (MasterB64.Length == 0)
                return null;

            byte[] MasterKey_Cyph = Convert.FromBase64String(MasterB64);
            return TPMDecypher(MasterKey_Cyph);
        }

        private static CngKey ObtainOrHandleTPMKey()
        {
            // Force provider to be Windows' TPM
            CngProvider tpmProvider = CngProvider.MicrosoftPlatformCryptoProvider;

            if (CngKey.Exists(KeyName, tpmProvider))
            {
                return CngKey.Open(KeyName, tpmProvider);
            }
            else
            {
                // Create key if it doesn't exist
                CngKeyCreationParameters creationParameters = new CngKeyCreationParameters
                {
                    Provider = tpmProvider,
                    // None ensures we don't accidentally wipe an existing key if called concurrently
                    KeyCreationOptions = CngKeyCreationOptions.None
                };

                return CngKey.Create(CngAlgorithm.Rsa, KeyName, creationParameters);
            }
        }

        private static byte[] TPMCypher(byte[] data)
        {
            using (CngKey key = ObtainOrHandleTPMKey())
            using (RSACng rsa = new RSACng(key))
            {
                // Cypher using OAEP
                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            }
        }

        private static byte[] TPMDecypher(byte[] data)
        {
            using (CngKey key = ObtainOrHandleTPMKey())
            using (RSACng rsa = new RSACng(key))
            {
                return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
            }
        }

        public static byte[] GetKeyBank()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string dir = Path.Combine(appData, "GenerallyMambo", "GMC");
            Directory.CreateDirectory(dir);

            string pathFile = Path.Combine(dir, "KeyBank.gmck");
            if (!File.Exists(pathFile))
                return [];
            return File.ReadAllBytes(pathFile);
        }
        public static void SaveKeyBank(byte[] data)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string dir = Path.Combine(appData, "GenerallyMambo", "GMC");
            Directory.CreateDirectory(dir);

            string pathFile = Path.Combine(dir, "KeyBank.gmck");
            File.WriteAllBytes(pathFile, data);
        }

        public static void DeleteKeyBank()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string dir = Path.Combine(appData, "GenerallyMambo", "GMC");
            Directory.CreateDirectory(dir);

            string pathFile = Path.Combine(dir, "KeyBank.gmck");
            if (File.Exists(pathFile))
                File.Delete(pathFile);
        }

        private byte[]? FindKey()
        {
            if (InPath.Length == 0)
            {
                MessageBox.Show("No input file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }

            byte[] raw = GetKeyBank();

            if (raw.Length == 0)
                return null;
            List<byte> dec = new();

            string TargetName = InPath.Split('\\').Last();

            byte[]? MKey = GetMasterKey();
            if (MKey == null)
            {
                MessageBox.Show("Error obtaining Master Key", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            var dec_r = CypherCapsule.Decypher(raw, dec, MKey);

            if (dec_r != CypherReturnCode.OK)
            {
                MessageBox.Show("Error decyphering key bank", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            string[] dec_s = System.Text.Encoding.ASCII.GetString(dec.ToArray()).Split('\n');
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 2)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    return Convert.FromBase64String(sp[1]);
                }
            }

            return null;
        }

        private bool SaveKey(byte[] key)
        {
            if (OutPath.Length == 0)
            {
                MessageBox.Show("No output file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (key.Length == 0)
            {
                MessageBox.Show("Key empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }

            string[] dec_s;

            byte[]? MKey = GetMasterKey();
            if (MKey == null)
            {
                MessageBox.Show("Error obtaining Master Key", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            {
                byte[] raw = GetKeyBank();

                if (raw.Length == 0)
                {
                    dec_s = new string[0];
                }
                else
                {
                    List<byte> dec = new();

                    CypherCapsule.Decypher(raw, dec, MKey);

                    dec_s = System.Text.Encoding.ASCII.GetString(dec.ToArray()).Split('\n');
                }
            }

            string TargetName = OutPath.Split('\\').Last();

            string Out = "";
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 2)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    var res = MessageBox.Show("This bank already has a key for a file with this name. Overwrite?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (res == DialogResult.No)
                        return false;
                }
                else
                    Out += $"{sp[0]}\t{sp[1]}";
            }

            Out += $"{TargetName}\t{Convert.ToBase64String(key)}";

            List<byte> enc = new();
            var re = CypherCapsule.Cypher(System.Text.Encoding.ASCII.GetBytes(Out), enc, MKey);

            SaveKeyBank(enc.ToArray());
            return true;
        }

        private bool DeleteKey()
        {
            if (OutPath.Length == 0)
            {
                MessageBox.Show("No output file selected", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }

            string[] dec_s;

            byte[]? MKey = GetMasterKey();
            if (MKey == null)
            {
                MessageBox.Show("Error obtaining Master Key", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            {
                byte[] raw = GetKeyBank();

                if (raw.Length == 0)
                    return false;
                List<byte> dec = new();

                CypherCapsule.Decypher(raw, dec, MKey);

                dec_s = System.Text.Encoding.ASCII.GetString(dec.ToArray()).Split('\n');
            }

            string TargetName = OutPath.Split('\\').Last();

            string Out = "";
            foreach (string s in dec_s)
            {
                string[] sp = s.Split('\t');
                if (sp.Length < 2)
                    continue;

                if (sp[0].Equals(TargetName))
                {
                    var res = MessageBox.Show("Key found. Are you sure you want to delete it? This action cannot be undone", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (res == DialogResult.No)
                        return false;
                }
                else
                    Out += $"{sp[0]}\t{sp[1]}\n";
            }

            List<byte> enc = new();
            CypherCapsule.Cypher(System.Text.Encoding.ASCII.GetBytes(Out), enc, MKey);

            SaveKeyBank(enc.ToArray());
            return true;
        }

        private void DelStorKey_Click(object sender, EventArgs e)
        {
            DeleteKey();
        }

        private void ExchangeBTN_Click(object sender, EventArgs e)
        {
            var tmp = InPath;
            InPath =OutPath;
            OutPath = tmp;
            openFileDialog1.FileName = InPath;
            InputTB.Text = InPath;
            saveFileDialog1.FileName = OutPath;
            OutputTB.Text = OutPath;
            DecypherCHK.Checked = InPath.Contains(".gmc");
        }
    }
}
