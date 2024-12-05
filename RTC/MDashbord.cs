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
    public partial class MDashbord : Form
    {
        public MDashbord()
        {
            InitializeComponent();
            panel6.Hide();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
        private void label2_Click(object sender, EventArgs e)
        {
            MLRequests obj = new MLRequests();
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
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from DEAccountSummaryTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from PoddoAccountTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from DivisaruAccountTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from GarusaruAccountTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from FDAccountTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from LoanTB";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            MDDGV.DataSource = ds.Tables[0];
            con.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            MNPAccounts obj = new MNPAccounts();
            obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            MPDocument obj = new MPDocument();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            panel6.Show();
           
        }

        private void label11_Click(object sender, EventArgs e)
        {
            MSettings obj = new MSettings();
            obj.Show();
            this.Hide();
        }
    }
}
