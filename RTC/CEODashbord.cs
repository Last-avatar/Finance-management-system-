using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class CEODashbord : Form
    {
        public CEODashbord()
        {
            InitializeComponent();
            panel3.Hide();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
private void label12_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from AccountSummaryTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DGMDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            NInvestments obj = new NInvestments();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            HLoans obj = new HLoans();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            panel3.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            CEOSettings obj = new CEOSettings();
            obj.Show();
            this.Hide();
        }
    }
}
