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
    public partial class SESettings : Form
    {
        public SESettings()
        {
            InitializeComponent();
            panel3.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void OPasswordTB_Leave(object sender, EventArgs e)
        {
            String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from SExcecutiveTB where SEName = '" + UsernameTB.Text + "' and SEPassword = '" + OPasswordTB.Text + "' ";
            SqlCommand sqlcomm = new SqlCommand(Query, con);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sqlcomm.ExecuteNonQuery();
            if (dt.Rows.Count > 0)
            {
                panel3.Show();
            }
            else
            {
                MessageBox.Show("Wrong Password or Username..!");
            }
            OPasswordTB.Text = "";
            con.Close();
        }

        private void LoginBt_Click(object sender, EventArgs e)
        {
            String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
            SqlConnection con = new SqlConnection(conString);
            String sqlquery = "update  SExcecutiveTB set SEPassword ='" + CPasswordTb.Text + "' where SEName = '" + UsernameTB.Text + "' ";
            con.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sqlcomm.ExecuteNonQuery();
            MessageBox.Show("Password Changed");
            con.Close();

            OPasswordTB.Clear();
            NPasswordTb.Clear();
            CPasswordTb.Clear();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SEDashbord obj = new SEDashbord();
            obj.Show();
            this.Hide();
        }
    }
}
