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
    public partial class FPReport : Form
    {
        public FPReport()
        {
            InitializeComponent();
            this.Load += FPReport_Load;
            populate();

        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";

        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from AccountSummaryTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                FRDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void FPReport_Load(object sender, EventArgs e)
        {
            AccountTyTB.Items.Clear(); // Clear existing items
            AccountTyTB.Items.Add("Poddo");
            AccountTyTB.Items.Add("Divisaru");
            AccountTyTB.Items.Add("Garusaru");
            AccountTyTB.SelectedIndex = 0; // Optional: Set default selection
        }

        private void DepositTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void AccountTyTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountTyTB.SelectedItem == null) return;

            string accountType = AccountTyTB.SelectedItem.ToString();

            decimal totalBalance = GetTotalBalance(accountType);
            decimal totalDeposit = GetTotalDeposit(accountType);
            decimal totalWithdrawal = GetTotalWithdrawal(accountType);

            BalanceTB.Text = totalBalance.ToString("C");
            DepositTB.Text = totalDeposit.ToString("C");
            WithdrawTB.Text = totalWithdrawal.ToString("C");
        }
        decimal GetTotalBalance(string accountType)
        {
            string tableName = GetAccountTableName(accountType);
            string query = $"SELECT SUM(Balance) FROM {tableName}";
            return ExecuteScalarDecimal(query);
        }

        decimal GetTotalDeposit(string accountType)
        {
            string tableName = GetDepositTableName(accountType);
            string query = $"SELECT SUM(Deposit) FROM {tableName}";
            return ExecuteScalarDecimal(query);
        }

        decimal GetTotalWithdrawal(string accountType)
        {
            string tableName = GetWithdrawalTableName(accountType);
            string query = $"SELECT SUM(Withdraw_amount) FROM {tableName}";
            return ExecuteScalarDecimal(query);
        }

        decimal ExecuteScalarDecimal(string query)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        string GetAccountTableName(string accountType)
        {
            switch (accountType)
            {
                case "Poddo": return "PoddoAccountTB";
                case "Divisaru": return "DivisaruAccountTB";
                case "Garusaru": return "GarusaruAccountTB";
                default: throw new ArgumentException("Invalid account type");
            }
        }

        string GetDepositTableName(string accountType)
        {
            switch (accountType)
            {
                case "Poddo": return "PoddoAccountCashTB";
                case "Divisaru": return "DivisaruAccountCashTB";
                case "Garusaru": return "GarusaruAccountCashTB";
                default: throw new ArgumentException("Invalid account type");
            }
        }

        string GetWithdrawalTableName(string accountType)
        {
            switch (accountType)
            {
                case "Poddo": return "PoddoWithdrawalsTB";
                case "Divisaru": return "DivisaruWithdrawalsTB";
                case "Garusaru": return "GarusaruWithdrawalsTB";
                default: throw new ArgumentException("Invalid account type");
            }
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

        private void InsertData(string accountType, decimal totalBalance, decimal totalDeposit, decimal totalWithdrawal)
        {
            string query = "INSERT INTO AccountSummaryTB (AccountType, TotalBalance, TotalDeposit, TotalWithdrawal) VALUES (@AccountType, @TotalBalance, @TotalDeposit, @TotalWithdrawal)";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountType", accountType);
                    command.Parameters.AddWithValue("@TotalBalance", totalBalance);
                    command.Parameters.AddWithValue("@TotalDeposit", totalDeposit);
                    command.Parameters.AddWithValue("@TotalWithdrawal", totalWithdrawal);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        private void SaveBt_Click(object sender, EventArgs e)
        {
            string accountType = AccountTyTB.SelectedItem.ToString();
            decimal totalBalance = GetTotalBalance(accountType);
            decimal totalDeposit = GetTotalDeposit(accountType);
            decimal totalWithdrawal = GetTotalWithdrawal(accountType);

            // Insert the data
            InsertData(accountType, totalBalance, totalDeposit, totalWithdrawal);

            // Refresh the DataGridView after insertion
            LoadDataIntoDataGridView();
        }

        private void BSDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the AccountType from the clicked row
                string accountType = FRDGV.Rows[e.RowIndex].Cells["AccountType"].Value.ToString();

                // Ask for confirmation before deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this account?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Delete the record and refresh the DataGridView
                    DeleteData(accountType);
                    LoadDataIntoDataGridView();
                }
            }
        }
        private void DeleteData(string accountType)
        {
           
        }
        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM AccountSummaryTB";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    FRDGV.DataSource = dataTable;
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            // Get the selected AccountType from the DataGridView (assuming AccountType is one of the columns)
            if (FRDGV.SelectedRows.Count > 0)
            {
                string accountType = FRDGV.SelectedRows[0].Cells["AccountType"].Value.ToString();

                // Delete the selected account type from the database
                string query = "DELETE FROM AccountSummaryTB WHERE AccountType = @AccountType";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountType", accountType);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                // Refresh the DataGridView to reflect the changes
                LoadDataIntoDataGridView();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void PrintBT_Click(object sender, EventArgs e)
        {
            FPCReport obj = new FPCReport();
            obj.Show();
        }
    }
}
