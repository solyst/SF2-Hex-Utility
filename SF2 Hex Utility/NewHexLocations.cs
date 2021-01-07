using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public partial class NewHexLocations : Form
    {
        public string FilePath { get; set; }
        static public bool SuppressBeeps { get; set; } = false;

        string hexPath;
        string extension;
        char[] invalidChars = Path.GetInvalidFileNameChars();


        public NewHexLocations(string directoryPath, string extension)
        {
            this.hexPath = directoryPath;
            this.extension = extension;
            InitializeComponent();
        }


        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (isFileNameValid())
                txtFileName.ForeColor = Color.Black;
            else
                txtFileName.ForeColor = Color.Red;
        }


        /// <summary>
        /// File Name is valid if the file does not already exist and if it does not contain any invalid characters.
        /// </summary>
        /// <returns></returns>
        private bool isFileNameValid()
        {
            bool valid = true;
            foreach (char c in invalidChars)
            {
                if (txtFileName.Text.Contains(c))
                {
                    valid = false;
                    break;
                }
            }
            if (File.Exists(getFilePathFromTextBox()))
            {
                valid = false;
            }
            return valid;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (isFileNameValid())
            {
                FilePath = getFilePathFromTextBox();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (SuppressBeeps == false)
                    SystemSounds.Beep.Play();
                MessageBox.Show(txtFileName.Text + " is an invalid file name. Names must not have any invalid characters and must not already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private string getFilePathFromTextBox()
        {
            return Path.Combine(hexPath, Path.GetFileNameWithoutExtension(txtFileName.Text) + "." + extension);
        }

    }
}
