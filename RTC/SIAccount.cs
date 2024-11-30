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
    public partial class SIAccount : Form
    {
        public SIAccount()
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
                String Query = "select * from PoddoAccountTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                PDGV.DataSource = ds.Tables[0];
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

            if (PNameTB.Text == "" || GNameTB.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || AddressTb.Text == "" || EmailTb.Text == "" || GenderTB.SelectedIndex == -1 || BCnoTB.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("update PoddoAccountTB set HName = @PHN,GName =@PGN,NIC = @PN,Contact_no = @PCn,Address = @PA,Email = @PE,Date = @PD,DBO =@PDob ,Gender = @PG,BC_No = PBn  where PID = @Pkey", con);
                    cmd.Parameters.AddWithValue("@PHN", PNameTB.Text);
                    cmd.Parameters.AddWithValue("@PGN", GNameTB.Text);
                    cmd.Parameters.AddWithValue("@PN", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@PCn", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@PA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@PE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@PD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@PDob", DBOTB.Value.Date);
                    cmd.Parameters.AddWithValue("@PG", GenderTB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PBn", BCnoTB.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Updated!!!");
                    PNameTB.Clear();
                    GNameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
                    EmailTb.Clear();
                    BCnoTB.Clear();
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
                    SqlCommand cmd = new SqlCommand("delete from PoddoAccountTB where PID = @Pkey", con);
                    cmd.Parameters.AddWithValue("@Pkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account Deleted!!!");
                    con.Close();
                    PNameTB.Clear();
                    GNameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
                    EmailTb.Clear();
                    BCnoTB.Clear();
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

            if (PNameTB.Text == "" || GNameTB.Text == "" || NICNoTb.Text == "" || ContactNoTb.Text == "" || AddressTb.Text == "" ||  EmailTb.Text == "" || GenderTB.SelectedIndex == -1 || BCnoTB.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into PoddoAccountTB (HName,GName,NIC,Contact_no,Address,Email,Date,DOB,Gender,BC_No) values(@PHN,@PGN,@PN,@PCn,@PA,@PE,@PD,@PDob,@PG,@PBn)", con);
                    cmd.Parameters.AddWithValue("@PHN", PNameTB.Text);
                    cmd.Parameters.AddWithValue("@PGN", GNameTB.Text);
                    cmd.Parameters.AddWithValue("@PN", NICNoTb.Text);
                    cmd.Parameters.AddWithValue("@PCn", ContactNoTb.Text);
                    cmd.Parameters.AddWithValue("@PA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@PE", EmailTb.Text);
                    cmd.Parameters.AddWithValue("@PD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@PDob",DBOTB.Value.Date);
                    cmd.Parameters.AddWithValue("@PG", GenderTB.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PBn",BCnoTB.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Poddo Account Create Successfully!");
                    PNameTB.Clear();
                    GNameTB.Clear();
                    NICNoTb.Clear();
                    ContactNoTb.Clear();
                    AddressTb.Clear();
                    EmailTb.Clear();
                    BCnoTB.Clear();
                    con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }


        }

        private void label11_Click(object sender, EventArgs e)
        {
            AAccount obj = new AAccount();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            AAccount obj = new AAccount();
            obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            CDashbord obj = new CDashbord();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
           SIAccount obj = new SIAccount();
            obj.Show();
            this.Hide();
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

        private void PDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            PNameTB.Text = PDGV.SelectedRows[0].Cells[1].Value.ToString();
            GNameTB.Text = PDGV.SelectedRows[0].Cells[2].Value.ToString();
            NICNoTb.Text = PDGV.SelectedRows[0].Cells[3].Value.ToString();
            ContactNoTb.Text = PDGV.SelectedRows[0].Cells[4].Value.ToString();
            AddressTb.Text = PDGV.SelectedRows[0].Cells[5].Value.ToString();
            EmailTb.Text = PDGV.SelectedRows[0].Cells[6].Value.ToString();
            dateTB.Text = PDGV.SelectedRows[0].Cells[7].Value.ToString();
            DBOTB.Text = PDGV.SelectedRows[0].Cells[8].Value.ToString();
            GenderTB.Text = PDGV.SelectedRows[0].Cells[9].Value.ToString();
            BCnoTB.Text = PDGV.SelectedRows[0].Cells[10].Value.ToString();

            if (PNameTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
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
