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

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Bancos de claves GMC (*.gmck)|*.gmck";
            openFileDialog1.ShowDialog(this);

            if (openFileDialog1.FileName.Length == 0 || !File.Exists(openFileDialog1.FileName))
                return;

            byte[] data = File.ReadAllBytes(openFileDialog1.FileName);
            string name = Encoding.ASCII.GetString(data.TakeLast(77).ToArray());

            string dst = Application.CommonAppDataPath + "\\" + name;
            if (File.Exists(dst))
            {
                var res = MessageBox.Show("This bank already exists. Overwrite?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.No)
                    return;

                File.Delete(dst);
            }

            File.WriteAllBytes(dst, data.Take(data.Length - 77).ToArray());
        }

        private void ChgKey_Click(object sender, EventArgs e)
        {
            if (OldKTB.Text.Length == 0)
            {
                MessageBox.Show("Current key can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NewKTB.Text.Length == 0)
            {
                MessageBox.Show("New key can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (UpdateKey(OldKTB.Text, NewKTB.Text) == 0)
            {
                MessageBox.Show("Key updated successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int UpdateKey(string Old, string New)
        {
            string PathOld = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(Old)))}.gmck";
            string PathNew = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(New)))}.gmck";

            if (!File.Exists(PathOld))
            {
                MessageBox.Show("No bank found with current key", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            if (!File.Exists(PathNew))
            {
                FileStream fs = File.Create(PathNew);
                fs.Close();
            }
            else
            {
                MessageBox.Show("A bank with the new key already exists", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            byte[] raw = File.ReadAllBytes(PathOld);
            byte[] dec = new byte[raw.Length];

            CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(Old), raw, ref dec, true, 15);

            CypherCapsule.Generic(System.Text.Encoding.ASCII.GetBytes(New), dec, ref raw, false, 15);

            File.WriteAllBytes(PathNew, raw);
            File.Delete(PathOld);

            return 0;
        }

        private int DelBank(string Old)
        {
            string PathOld = Application.CommonAppDataPath + $@"\KeyBank_{System.Convert.ToHexString(SHA256.HashData(System.Text.Encoding.ASCII.GetBytes(Old)))}.gmck";

            if (!File.Exists(PathOld))
            {
                MessageBox.Show("No bank found with current key", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            File.Delete(PathOld);
            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (OldKTB.Text.Length == 0)
            {
                MessageBox.Show("Current key can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var res = MessageBox.Show("Are you sure you want to delete this bank?\n (This action cannot be undone)", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (res == DialogResult.No)
                return;

            if (DelBank(OldKTB.Text) == 0)
            {
                MessageBox.Show("Bank deleted successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (OldKTB.Text.Length == 0)
            {
                MessageBox.Show("Current key can't be empty", "GMC: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Bancos de claves GMC (*.gmck)|*.gmck";
            saveFileDialog1.DefaultExt = ".gmck";
            saveFileDialog1.ShowDialog(this);

            if (saveFileDialog1.FileName.Length == 0)
                return;

            string name = $"KeyBank_{Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(OldKTB.Text)))}.gmck";

            string PathLocal = Application.CommonAppDataPath + "\\" + name;

            if (File.Exists(saveFileDialog1.FileName))
            {
                var res = MessageBox.Show("This bank already exists. Overwrite?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.No)
                    return;
                File.Delete(saveFileDialog1.FileName);
            }

            File.Copy(PathLocal, saveFileDialog1.FileName);
            File.AppendAllText(saveFileDialog1.FileName, name);
        }
    }
}
