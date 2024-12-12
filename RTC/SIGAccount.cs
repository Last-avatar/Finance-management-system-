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
    public partial class SIGAccount : Form
    {
        public SIGAccount()
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
                String Query = "select * from GarusaruAccountTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                GADGV.DataSource = ds.Tables[0];
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

            if (NameTB.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || GenderTB.SelectedIndex == -1 || AddressTb.Text == "" || EmailTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("update GarusaruAccountTB set Name = @GN,NIC = @GNc,Contact_No = @GCn,Gender = @GG, Address = @GA,Email = @GE,Date = @GD where GID = @Gkey", con);
                    cmd.Parameters.AddWithValue("@GN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@GNc", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@GCn", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@GG", GenderTB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@GA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@GE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@GD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@Gkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Updated!!!");
                    NameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
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
                    SqlCommand cmd = new SqlCommand("delete from  GarusaruAccountTB where GID = @Gkey", con);
                    cmd.Parameters.AddWithValue("@Gkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Deleted!!!");
                    con.Close();
                    NameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
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

            if (NameTB.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || GenderTB.SelectedIndex == -1 || AddressTb.Text == "" || EmailTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into  GarusaruAccountTB (Name,NIC,Contact_No,Gender,Address,Email,Date) values(@GN,@GNc,@GCn,@GG,@GA,@GE,@GD)", con);
                    cmd.Parameters.AddWithValue("@GN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@GNc", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@GCn", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@GG", GenderTB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@GA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@GE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@GD", dateTB.Value.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Create Successfully!");
                    NameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
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


        private void SIGAccount_Load(object sender, EventArgs e)
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

        private void DADGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NameTB.Text = GADGV.SelectedRows[0].Cells[1].Value.ToString();
            NICNoTb.Text = GADGV.SelectedRows[0].Cells[2].Value.ToString();
            ContactNoTb.Text = GADGV.SelectedRows[0].Cells[3].Value.ToString();
            GenderTB.Text = GADGV.SelectedRows[0].Cells[4].Value.ToString();
            AddressTb.Text = GADGV.SelectedRows[0].Cells[5].Value.ToString();
            EmailTb.Text = GADGV.SelectedRows[0].Cells[6].Value.ToString();
            dateTB.Text = GADGV.SelectedRows[0].Cells[7].Value.ToString();

            if (NameTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(GADGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
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

        private void label1_Click(object sender, EventArgs e)
        {
            CDashbord obj = new CDashbord();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            SIAccount obj = new SIAccount();
            obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            SIDAccount obj = new SIDAccount();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            SIGAccount obj = new SIGAccount();
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

        private void label26_Click(object sender, EventArgs e)
        {
            CSettings obj = new CSettings();
            obj.Show();
            this.Hide();
        }
    }
}
