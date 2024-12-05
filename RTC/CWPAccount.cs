using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RTC
{
    public partial class CWPAccount : Form
    {
        public CWPAccount()
        {
            InitializeComponent();
            panelCP.Hide();
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
                String Query = "select * from PoddoWithdrawalsTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                PWDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void LoadAccountNumbers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    string query = "SELECT PID FROM PoddoAccountTB";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable accounts = new DataTable();
                    adapter.Fill(accounts);

                    AccountNoTB.DataSource = accounts;
                    AccountNoTB.DisplayMember = "PID"; // What the user sees
                    AccountNoTB.ValueMember = "PID";   // The actual value
                    AccountNoTB.SelectedIndex = -1;    // Ensure no default selection
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading accounts: " + ex.Message);
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

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

        private void label7_Click(object sender, EventArgs e)
        {
            MAccount obj = new MAccount();
            obj.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            MAccount obj = new MAccount();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            CWPAccount obj = new CWPAccount();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            CWPAccount obj = new CWPAccount();
            obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            CWDAccount obj = new CWDAccount();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CWGAccount obj = new CWGAccount();
            obj.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            CWIAccount obj = new CWIAccount();
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

        private void AccountNoTB_SelectedIndexChanged(object sender, EventArgs e)
        {
          
                // Check if a valid account number is selected
                if (AccountNoTB.SelectedValue == null || AccountNoTB.SelectedValue is DataRowView)
                    return;

                string accountNo = AccountNoTB.SelectedValue.ToString();

                if (!string.IsNullOrEmpty(accountNo))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(conString))
                        {
                            conn.Open();

                            string query = "SELECT HName, NIC, Balance FROM PoddoAccountTB WHERE PID = @AccountNo";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@AccountNo", accountNo);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Assign values to the text boxes
                                        NameTB.Text = reader["HName"].ToString();
                                        NICTB.Text = reader["NIC"].ToString();
                                        BalanceTB.Text = reader["Balance"].ToString();
                                    }
                                    else
                                    {
                                        // Clear fields if no record is found
                                        MessageBox.Show("No account found for the selected Account Number.");
                                        NameTB.Clear();
                                        NICTB.Clear();
                                        BalanceTB.Clear();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error fetching account details: " + ex.Message);
                    }
                }
                else
                {
                    // Clear fields if account number is not valid
                    NameTB.Clear();
                    NICTB.Clear();
                    BalanceTB.Clear();
                }
            

        }

        private void WithdrawBt_Click(object sender, EventArgs e)
        {
           
                if (string.IsNullOrEmpty(WithdrawTb.Text) || AccountNoTB.SelectedValue == null)
                {
                    MessageBox.Show("Please fill out all fields.");
                    return;
                }

                decimal withdrawAmount;
                if (!decimal.TryParse(WithdrawTb.Text, out withdrawAmount))
                {
                    MessageBox.Show("Please enter a valid withdrawal amount.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Update Balance
                        string updateQuery = "UPDATE PoddoAccountTB SET Balance = Balance - @WithdrawAmount WHERE PID = @AccountNo";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction);
                        updateCmd.Parameters.AddWithValue("@WithdrawAmount", withdrawAmount);
                        updateCmd.Parameters.AddWithValue("@AccountNo", AccountNoTB.SelectedValue);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Failed to update balance. Check if the account exists.");
                        }

                        // Log Withdrawal
                        string insertQuery = "INSERT INTO PoddoWithdrawalsTB (PID, Date, Withdraw_amount) VALUES (@AccountNo, @Date, @WithdrawAmount)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction);
                        insertCmd.Parameters.AddWithValue("@AccountNo", AccountNoTB.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@Date", DateTime.Now); // Use current date
                        insertCmd.Parameters.AddWithValue("@WithdrawAmount", withdrawAmount);

                        insertCmd.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Withdrawal Successful! New Balance: " + (GetNewBalance(connection, AccountNoTB.SelectedValue)));

                        // Clear the UI and reset the input fields
                        WithdrawTb.Clear();
                        NameTB.Clear();
                        NICTB.Clear();
                        BalanceTB.Clear();
                        AccountNoTB.SelectedIndex = -1;

                        // Refresh the DataGridView after successful withdrawal
                        populate();  // Call the populate method again to reload the withdrawal details in the DataGridView

                        // Refresh the balance after successful withdrawal
                        AccountNoTB_SelectedIndexChanged(sender, e);  // To refresh the account details, if needed
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            

        }

        // Method to fetch the updated balance
        private decimal GetNewBalance(SqlConnection connection, object accountNo)
        {
            string balanceQuery = "SELECT Balance FROM PoddoAccountTB WHERE PID = @AccountNo";
            SqlCommand balanceCmd = new SqlCommand(balanceQuery, connection);
            balanceCmd.Parameters.AddWithValue("@AccountNo", accountNo);

            object result = balanceCmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }
        private void PWDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
       
        }
    }
}
