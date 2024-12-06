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
    public partial class LESettings : Form
    {
        public LESettings()
        {
            InitializeComponent();
            panel3.Hide();
        }

        private void OPasswordTB_Leave(object sender, EventArgs e)
        {
            String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from LExcecutiveTB where LEName = '" + UsernameTB.Text + "' and LEPassword = '" + OPasswordTB.Text + "' ";
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
            String sqlquery = "update  LExcecutiveTB set LEPassword ='" + CPasswordTb.Text + "' where LEName = '" + UsernameTB.Text + "' ";
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
            LEDashbord obj = new LEDashbord();
            obj.Show();
            this.Hide();
        }
    }
}
