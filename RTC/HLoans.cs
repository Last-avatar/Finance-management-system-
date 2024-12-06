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
    public partial class HLoans : Form
    {
        public HLoans()
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
                String Query = "select * from LoanRequestsTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                MlRDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void SaveLoan()
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
                SqlCommand cmd = new SqlCommand("UPDATE LoanRequestsTB SET LoanStatus = @LS WHERE LID = @Lkey", con);
                cmd.Parameters.AddWithValue("@LS", StatusTB.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Lkey", key);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Loan Updated!!!");
                }
                else
                {
                    MessageBox.Show("No record found to update.");
                }

                // Clear fields and refresh data grid view
                AccountTyTB.Clear();
                AccountNoTB.Clear();
                NameTB.Clear();
                NICTB.Clear();

                StatusTB.SelectedIndex = -1; // Reset the combo box

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
        int key=0; 
        private void MlRDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AccountTyTB.Text = MlRDGV.SelectedRows[0].Cells[1].Value.ToString();
            AccountNoTB.Text = MlRDGV.SelectedRows[0].Cells[2].Value.ToString();
            NameTB.Text = MlRDGV.SelectedRows[0].Cells[3].Value.ToString();
            NICTB.Text = MlRDGV.SelectedRows[0].Cells[4].Value.ToString();
            dateTB.Text = MlRDGV.SelectedRows[0].Cells[5].Value.ToString();
            StatusTB.Text = MlRDGV.SelectedRows[0].Cells[6].Value.ToString();

            if (AccountTyTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(MlRDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            SaveLoan();
        }
    }
}
