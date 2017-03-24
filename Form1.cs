using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Execute the Menu tests.
            MenuTests.Test1_Verify_MenuItem_Prices();
            MenuTests.Test2_Check_Unknown_MenuItem();
            MenuTests.Test3_Check_Standard_Bill();
        }
    }
}
