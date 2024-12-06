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
    public partial class NInvestments : Form
    {
        public NInvestments()
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
                String Query = "select * from FDAccountTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                IADGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            CEODashbord obj = new CEODashbord();
            obj.Show();
            this.Hide();
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
                SqlCommand cmd = new SqlCommand("UPDATE FDAccountTB SET Status = @IS WHERE FDID = @Ikey", con);
                cmd.Parameters.AddWithValue("@IS", StatusTB.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Ikey", key);

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

        private void IADGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AccountNoTB.Text = IADGV.SelectedRows[0].Cells[0].Value.ToString();
            NameTB.Text = IADGV.SelectedRows[0].Cells[1].Value.ToString();
            ContactTB.Text = IADGV.SelectedRows[0].Cells[4].Value.ToString();
            AddressTB.Text = IADGV.SelectedRows[0].Cells[2].Value.ToString();
            NICTB.Text = IADGV.SelectedRows[0].Cells[3].Value.ToString();
            dateTB.Text = IADGV.SelectedRows[0].Cells[5].Value.ToString();
            StatusTB.Text = IADGV.SelectedRows[0].Cells[9].Value.ToString();

            if (AccountNoTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(IADGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
