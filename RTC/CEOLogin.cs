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
    public partial class CEOLogin : Form
    {
        public CEOLogin()
        {
            InitializeComponent();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
        private void LoginBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            if (UsernameTB.Text == "" || PasswordTB.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("select count(*)from CEOTB where CEName = '" + UsernameTB.Text + "' and CEPassword = '" + PasswordTB.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        CEODashbord obj = new CEODashbord();
                        obj.Show();
                        this.Hide();
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong Username Or Password!!!");
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
