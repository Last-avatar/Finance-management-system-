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
    public partial class MNDAccounts : Form
    {
        public MNDAccounts()
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
                String Query = "select * from DivisaruAccountTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                DADGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

            private void label14_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTB_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);

            if (StatusTB.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
                return;
            }

            try
            {
                con.Open();

                // Use the selected value from the combo box, not the index
                SqlCommand cmd = new SqlCommand("UPDATE DivisaruAccountTB SET Status = @DS WHERE DID = @Dkey", con);
                cmd.Parameters.AddWithValue("@DS", StatusTB.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Dkey", key);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Account Updated!!!");
                }
                else
                {
                    MessageBox.Show("No record found to update.");
                }

                // Clear fields and r Acefresh data grid view

                AccountNoTB.Clear();
                NameTB.Clear();
                ContactTB.Clear();
                AddressTB.Clear();
                GenderTB.Clear();
                StatusTB.SelectedIndex = -1; // Reset the combo box
                NICTB.Clear();

                populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        int key = 0;
        private void DADGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AccountNoTB.Text = DADGV.SelectedRows[0].Cells[0].Value.ToString();
            NameTB.Text = DADGV.SelectedRows[0].Cells[1].Value.ToString();
            ContactTB.Text = DADGV.SelectedRows[0].Cells[3].Value.ToString();
            AddressTB.Text = DADGV.SelectedRows[0].Cells[5].Value.ToString();
            NICTB.Text = DADGV.SelectedRows[0].Cells[2].Value.ToString();
            GenderTB.Text = DADGV.SelectedRows[0].Cells[4].Value.ToString();
            dateTB.Text = DADGV.SelectedRows[0].Cells[7].Value.ToString();
            StatusTB.Text = DADGV.SelectedRows[0].Cells[9].Value.ToString();

            if (AccountNoTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(DADGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            MDashbord obj = new MDashbord();
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
            MNPAccounts obj = new MNPAccounts();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            MNGAccounts obj = new MNGAccounts();
            obj.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            MNGAccounts obj = new MNGAccounts();
            obj.Show();
            this.Hide();
        }
    }
}
