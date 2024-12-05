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
    public partial class MSettings : Form
    {
        public MSettings()
        {
            InitializeComponent();
            panel3.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OPasswordTB_Leave(object sender, EventArgs e)
        {
            String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            String Query = "select * from ManagerTB where MName = '" + UsernameTB.Text + "' and MPassword = '" + OPasswordTB.Text + "' ";
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
            String sqlquery = "update  ManagerTB set MPassword ='" + CPasswordTb.Text + "' where MName = '" + UsernameTB.Text + "' ";
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
            MDashbord obj = new MDashbord();
            obj.Show();
            this.Hide();
        }
    }
}
