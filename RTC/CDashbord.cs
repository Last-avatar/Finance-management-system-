using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class CDashbord : Form
    {
        public CDashbord()
        {
            InitializeComponent();
            panel6.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            AAccount obj = new AAccount();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MAccount obj = new MAccount();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            panel6.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            CSettings obj = new CSettings();
            obj.Show();
            this.Hide();
        }
    }
}
