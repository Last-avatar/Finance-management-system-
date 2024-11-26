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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RTC
{
    public partial class BranchM : Form
    {
        public BranchM()
        {
            InitializeComponent();
            populate();

        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from BranchTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                BranchDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void EditBranch()
        {
            SqlConnection con = new SqlConnection(conString);

            if (BnameTB.Text == "" || BAddressTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("update BranchTB set BName = @BN, BAddress = @BA where BID = @Bkey", con);
                    cmd.Parameters.AddWithValue("@BN", BnameTB.Text);
                    cmd.Parameters.AddWithValue("@BA", BAddressTb.Text);
                    cmd.Parameters.AddWithValue("@Bkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Branch Updated!!!");
                    BnameTB.Clear();
                    BAddressTb.Clear();
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
        private void DeleteBranch()
        {
            SqlConnection con = new SqlConnection(conString);
            if (key == 0)
            {
                MessageBox.Show("Select a Branch!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from BranchTB where BID = @Bkey", con);
                    cmd.Parameters.AddWithValue("@Bkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Branch Deleted!!!");
                    con.Close();
                    BnameTB.Clear();
                    BAddressTb.Clear();
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
        private void InsertBranch()
        {
            SqlConnection con = new SqlConnection(conString);

            if (BnameTB.Text == "" || BAddressTb.Text == "") 
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BranchTB(BName,BAddress) values(@BN,@BA)", con);
                    cmd.Parameters.AddWithValue("@BN", BnameTB.Text);
                    cmd.Parameters.AddWithValue("@BA", BAddressTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Branch Updated!!!");
                    BnameTB.Clear();
                    BAddressTb.Clear();
                    con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }


        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            ADashbord obj = new ADashbord();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            UserM obj = new UserM();
            obj.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BranchDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void BranchDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BnameTB.Text = BranchDGV.SelectedRows[0].Cells[1].Value.ToString();
            BAddressTb.Text = BranchDGV.SelectedRows[0].Cells[2].Value.ToString();
         
            if (BnameTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(BranchDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBt_Click(object sender, EventArgs e)
        {
            EditBranch();
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertBranch();
        }

        private void DeleteBt_Click(object sender, EventArgs e)
        {
            DeleteBranch();
        }

        private void BAddressTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void BranchDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
