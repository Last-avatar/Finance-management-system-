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
using static System.Windows.Forms.AxHost;

namespace RTC
{
    public partial class AAccount : Form
    {
        public AAccount()
        {
            InitializeComponent();
            populate();
            panelCP.Hide();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from FDAccountTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                FDDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void EditAccount()
        {
            SqlConnection con = new SqlConnection(conString);

            if (FDNameTB.Text == "" || AddressTb.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || EmailTb.Text == "" || CStatusCB.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else if (key == 0)
            {
                MessageBox.Show("Please select a valid account from the table!");
            }
            else
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("update FDAccountTB set Name = @FDN, Address = @FDA, NIC = @FDNc, Contact_no = @FDC, Date = @FDD, Email = @FDE, Civil_status = @FDCS where FDID = @FDkey", con);
                    cmd.Parameters.AddWithValue("@FDN", FDNameTB.Text);
                    cmd.Parameters.AddWithValue("@FDA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@FDNc", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@FDC", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@FDD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@FDE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@FDCS", CStatusCB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FDkey", key); // Ensure this is set

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Updated!!!");

                    FDNameTB.Clear();
                    AddressTb.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    EmailTb.Clear();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        int key = 0;
        private void DeleteAccount()
        {
            SqlConnection con = new SqlConnection(conString);
            if (key == 0)
            {
                MessageBox.Show("Select a Account!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from FDAccountTB where FDID = @FDkey", con);
                    cmd.Parameters.AddWithValue("@FDkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Deleted!!!");
                    con.Close();
                    FDNameTB.Clear();
                    AddressTb.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    EmailTb.Clear();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }


        }
        private void InsertAccount()
        {
            SqlConnection con = new SqlConnection(conString);

            if (FDNameTB.Text == "" || AddressTb.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || EmailTb.Text == "" || CStatusCB.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into FDAccountTB (Name,Address,NIC,Contact_no,Date,Email,Civil_status) values(@FDN,@FDA,@FDNc,@FDC,@FDD,@FDE,@FDCS)", con);
                    cmd.Parameters.AddWithValue("@FDN", FDNameTB.Text);
                    cmd.Parameters.AddWithValue("@FDA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@FDNc", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@FDC", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@FDD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@FDE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@FDCS",CStatusCB.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Create Successfully!");
                    FDNameTB.Clear();
                    AddressTb.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    EmailTb.Clear();
                    con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }


        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertAccount();
        }

        private void DeleteBt_Click(object sender, EventArgs e)
        {
            DeleteAccount();
        }

        private void EditTb_Click(object sender, EventArgs e)
        {
            EditAccount();
        }

        private void FDDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            FDNameTB.Text = FDDGV.SelectedRows[0].Cells[1].Value.ToString();
            AddressTb.Text = FDDGV.SelectedRows[0].Cells[2].Value.ToString();
            NICNoTb.Text = FDDGV.SelectedRows[0].Cells[3].Value.ToString();
            ContactNoTb.Text = FDDGV.SelectedRows[0].Cells[4].Value.ToString();
            dateTB.Text = FDDGV.SelectedRows[0].Cells[5].Value.ToString();
            EmailTb.Text = FDDGV.SelectedRows[0].Cells[6].Value.ToString();
            CStatusCB.Text = FDDGV.SelectedRows[0].Cells[7].Value.ToString();

            if (FDNameTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(FDDGV.SelectedRows[0].Cells[0].Value.ToString());
               
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            CDashbord obj = new CDashbord();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            AAccount obj = new AAccount();
            obj.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            AAccount obj = new AAccount();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            SIAccount obj = new SIAccount();
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
            panelCP.Show();
        }

        private void label20_Click(object sender, EventArgs e)
        {
            CSettings obj = new CSettings();
            obj.Show();
            this.Hide();
        }
    }
}
