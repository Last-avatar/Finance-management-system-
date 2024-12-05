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
    public partial class ILoan : Form
    {
        public ILoan()
        {
            InitializeComponent();
            LoadAccountNumbers();
            populate();
        }


        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";

        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from LoanTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                lDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        int key = 0;
       


        
        private void InsertLoan()
        {
            SqlConnection con = new SqlConnection(conString);

            if (AccountNoTB.SelectedIndex == -1 || AccountTyTB.Text == "" || NameTB.Text == "" || NICTB.Text == "" || LAmountTB.Text == "" || LRateCB.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    // Ensure the value from AccountNoTB is properly fetched
                    string accountNo = AccountNoTB.SelectedValue != null ? AccountNoTB.SelectedValue.ToString() : "";

                    // Prepare the SQL command
                    SqlCommand cmd = new SqlCommand("INSERT INTO LoanTB (LID, Account_type, Name, NIC, Date, Loan_amount, loan_Rate) VALUES (@LID, @LAT, @LN, @LNic, @LD, @LA, @LR)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@LID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@LAT", AccountTyTB.Text);
                    cmd.Parameters.AddWithValue("@LN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@LNic", NICTB.Text);
                    cmd.Parameters.AddWithValue("@LD", dateTB.Value.Date);
                    cmd.Parameters.AddWithValue("@LA", LAmountTB.Text);
                    cmd.Parameters.AddWithValue("@LR", LRateCB.SelectedItem.ToString());

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Success message
                    MessageBox.Show("Loan Created Successfully!");

                    // Clear fields and reload data
                    AccountNoTB.SelectedIndex = -1;
                    AccountTyTB.Clear();
                    NameTB.Clear();
                    NICTB.Clear();
                    LAmountTB.Clear();
                    LRateCB.SelectedIndex = -1;
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
        }


        private void LoadAccountNumbers()
        {
            string query = "SELECT LID FROM LoanRequestsTB WHERE LoanStatus = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountNoTB.DataSource = dt;
                AccountNoTB.DisplayMember = "LID";  // Ensure this matches the column name in your database
                AccountNoTB.ValueMember = "LID";
                AccountNoTB.SelectedIndex = -1; // Set default to no selection
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            CDashbord obj = new CDashbord();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            IssueLoan obj = new IssueLoan();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void AccountNoTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountNoTB.SelectedValue == null || AccountNoTB.SelectedValue is DataRowView)
            {
                return;
            }

            string accountNo = AccountNoTB.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(accountNo))
            {
                LoadAccountDetails(accountNo);
            }
        }
        private void LoadAccountDetails(string accountNo)
        {
            string query = "SELECT AccountType, Name, NIC FROM LoanRequestsTB WHERE LID = @AccountNo AND LoanStatus = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Display fetched values in the respective fields
                    AccountTyTB.Text = reader["AccountType"].ToString();
                    NameTB.Text = reader["Name"].ToString();
                    NICTB.Text = reader["NIC"].ToString();
                }
                conn.Close();
            }
        }

        private void lDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertLoan();
        }

        private void Deletebt_Click(object sender, EventArgs e)
        {
           
        }
    }
}
