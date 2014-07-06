using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace eTalker
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {
            richTextBox1.LoadFile(Path.GetDirectoryName(Application.ExecutablePath)+@"\..\..\Help.rtf");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
