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
    public partial class BDStatement : Form
    {
        public BDStatement()
        {
            InitializeComponent();
            populate();
            LoadAccountNumbers();
        }


        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";

        private void LoadAccountNumbers()
        {
            try
            {
                AccountNoTB.SelectedIndexChanged -= AccountNoTB_SelectedIndexChanged; // Detach event

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string query = "SELECT DID FROM DivisaruAccountTB";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    // Bind the dropdown
                    DataRow defaultRow = dt.NewRow();
                    defaultRow["DID"] = DBNull.Value;
                    dt.Rows.InsertAt(defaultRow, 0);

                    AccountNoTB.DataSource = null;
                    AccountNoTB.DataSource = dt;
                    AccountNoTB.DisplayMember = "DID";
                    AccountNoTB.ValueMember = "DID";
                }

                AccountNoTB.SelectedIndexChanged += AccountNoTB_SelectedIndexChanged; // Reattach event
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading account numbers: " + ex.Message);
            }
        }





        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from DivisaruAccountTransactionSummaryTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                BSDDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        int key = 0;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            BStatement obj = new BStatement();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
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

        private void AccountNoTB_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (AccountNoTB.SelectedValue != null && AccountNoTB.SelectedValue != DBNull.Value)
                {
                    int selectedPID;
                    if (int.TryParse(AccountNoTB.SelectedValue.ToString(), out selectedPID))
                    {
                        string query = "SELECT Name, NIC FROM DivisaruAccountTB WHERE DID = @DID";

                        using (SqlConnection conn = new SqlConnection(conString))
                        {
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@DID", selectedPID);
                            conn.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                NameTB.Text = reader["Name"].ToString();
                                NICTB.Text = reader["NIC"].ToString();
                            }
                            else
                            {
                                NameTB.Clear();
                                NICTB.Clear();
                                MessageBox.Show("No data found for the selected account!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid DID format!");
                    }
                }
                else
                {
                    NameTB.Clear();
                    NICTB.Clear();
                    MessageBox.Show("Please select a valid account!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching account details: " + ex.Message);
            }
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedPID = AccountNoTB.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedPID))
                {
                    MessageBox.Show("Please select a valid account number.");
                    return;
                }

                decimal totalDeposits = 0;
                decimal totalWithdrawals = 0;
                decimal balance = 0;
                String Name = "";
                DateTime transactionDate = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();

                    // Fetch total deposits
                    string depositQuery = "SELECT SUM(Deposit) AS TotalDeposits FROM DivisaruAccountCashTB WHERE DID = @DID";
                    SqlCommand depositCmd = new SqlCommand(depositQuery, conn);
                    depositCmd.Parameters.AddWithValue("@DID", selectedPID);
                    object depositResult = depositCmd.ExecuteScalar();
                    if (depositResult != DBNull.Value)
                        totalDeposits = Convert.ToDecimal(depositResult);

                    // Fetch total withdrawals
                    string withdrawalQuery = "SELECT SUM(Withdraw_Amount) AS TotalWithdrawals FROM DivisaruWithdrawalsTB WHERE DID = @DID";
                    SqlCommand withdrawalCmd = new SqlCommand(withdrawalQuery, conn);
                    withdrawalCmd.Parameters.AddWithValue("@DID", selectedPID);
                    object withdrawalResult = withdrawalCmd.ExecuteScalar();
                    if (withdrawalResult != DBNull.Value)
                        totalWithdrawals = Convert.ToDecimal(withdrawalResult);

                    // Fetch current balance
                    string balanceQuery = "SELECT Balance FROM DivisaruAccountTB WHERE DID = @DID";
                    SqlCommand balanceCmd = new SqlCommand(balanceQuery, conn);
                    balanceCmd.Parameters.AddWithValue("@DID", selectedPID);
                    object balanceResult = balanceCmd.ExecuteScalar();
                    if (balanceResult != DBNull.Value)
                        balance = Convert.ToDecimal(balanceResult);

                    // Fetch Name
                    string NameQuery = "SELECT Name FROM DivisaruAccountTB WHERE DID = @DID";
                    SqlCommand NameCmd = new SqlCommand(NameQuery, conn);
                    NameCmd.Parameters.AddWithValue("@DID", selectedPID);
                    object NameResult = NameCmd.ExecuteScalar();
                    if (NameResult != DBNull.Value)
                        Name = Convert.ToString(NameResult);

                    // Insert into AccountTransactionSummaryTB
                    string insertQuery = "INSERT INTO  DivisaruAccountTransactionSummaryTB (DID,Name, TotalDeposits, TotalWithdrawals, Balance, TransactionDate) VALUES (@DID,@DN, @Deposits, @Withdrawals, @Balance, @Date)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@DID", selectedPID);
                    insertCmd.Parameters.AddWithValue("@DN", Name);
                    insertCmd.Parameters.AddWithValue("@Deposits", totalDeposits);
                    insertCmd.Parameters.AddWithValue("@Withdrawals", totalWithdrawals);
                    insertCmd.Parameters.AddWithValue("@Balance", balance);
                    insertCmd.Parameters.AddWithValue("@Date", transactionDate);
                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Transaction summary saved successfully!");
                populate(); // Refresh DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving transaction: " + ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Please select a transaction to delete.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string query = "DELETE FROM  DivisaruAccountTransactionSummaryTB WHERE ID = @TransactionID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@TransactionID", key);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Transaction deleted successfully!");
                populate(); // Refresh DataGridView
                key = 0; // Reset key
                AccountNoTB.SelectedIndex = -1; ;
                NameTB.Clear();
                NICTB.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting transaction: " + ex.Message);
            }

        }

        private void BSDDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Ensure a valid row is clicked
                {
                    DataGridViewRow row = BSDDGV.Rows[e.RowIndex];
                    key = Convert.ToInt32(row.Cells["ID"].Value); // Assuming "TransactionID" is the column name

                    // Optional: Display the details in text boxes
                    AccountNoTB.Text = row.Cells["DID"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting transaction: " + ex.Message);
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            BGStatement obj = new BGStatement();
            obj.Show();
            this.Hide();
        }

        private void PrintBT_Click(object sender, EventArgs e)
        {
            BDStatementReport obj = new BDStatementReport();
            obj.Show();
           
        }
    }
}
