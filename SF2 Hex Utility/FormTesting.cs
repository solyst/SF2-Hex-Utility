using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public partial class FormTesting : Form
    {
        public Rom TestRom { get; set; }

        public FormTesting()
        {
            InitializeComponent();
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            picTest.Refresh();
            txtHex.Text = HexRenderer.BytesToHexString(ByteConversion.ToBytesFromHex(txtHex.Text));
        }

        private void picTest_Paint(object sender, PaintEventArgs e)
        {
            byte[] bytes = ByteConversion.ToBytesFromHex(txtHex.Text);
            HexRenderer.DrawHexWithHighlight(e.Graphics, bytes, true, 10, (int)numIndex.Value, (int)numLength.Value, new Point(10, 10));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            byte?[] bytes = ByteConversion.ToBytesFromHexWildcard(txtSearchTerm.Text);
            lblSearchResult.Text = ByteConversion.ToHex(ByteConversion.ToBytes(TestRom.Find(bytes)));
        }
    }
}
