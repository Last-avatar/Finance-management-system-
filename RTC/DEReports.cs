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
    public partial class DEReports : Form
    {
        public DEReports()
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
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string query = "SELECT * FROM DEAccountSummaryTB";
                    SqlDataAdapter sda = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    DERDGV.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FPReport_Load(object sender, EventArgs e)
        {
            AccountTyTB.Items.Clear();
            AccountTyTB.Items.Add("Poddo");
            AccountTyTB.Items.Add("Divisaru");
            AccountTyTB.Items.Add("Garusaru");
            AccountTyTB.SelectedIndex = 0; // Default selection

            DateTB.Value = DateTime.Today; // Default date
            UpdateDataForSelection(); // Load data for default selections
        }

        private bool ValidateSelection()
        {
            if (AccountTyTB.SelectedItem == null)
            {
                MessageBox.Show("Please select an account type.");
                return false;
            }
            if (DateTB.Value == null)
            {
                MessageBox.Show("Please select a valid date.");
                return false;
            }
            return true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateDataForSelection();
        }

        private void AccountTyTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataForSelection();
        }
        private void LoadDataByDate(DateTime selectedDate, string accountType)
        {

            string query = "SELECT * FROM DEAccountSummaryTB WHERE CONVERT(DATE, Date) = @SelectedDate AND AccountType = @AccountType";

            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                        command.Parameters.AddWithValue("@AccountType", accountType);

                        using (SqlDataAdapter sda = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            sda.Fill(dataTable);
                            DERDGV.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for selected date and account type: {ex.Message}");
            }
        }

        private void UpdateDataForSelection()
        {
            if (AccountTyTB.SelectedItem != null)
            {
                string accountType = AccountTyTB.SelectedItem.ToString().Trim(); // Sanitize input
                DateTime selectedDate = DateTB.Value;

                // Display totals in text boxes
                decimal totalDeposit = GetTotalDepositForDate(selectedDate, accountType);
                decimal totalWithdrawal = GetTotalWithdrawalForDate(selectedDate, accountType);

                DepositTB.Text = totalDeposit.ToString("C");
                WithdrawTB.Text = totalWithdrawal.ToString("C");
            }
            else
            {
                MessageBox.Show("Please select an account type.");
            }
        }


        private decimal GetTotalDepositForDate(DateTime date, string accountType)
        {
            string depositTable = GetDepositTableName(accountType);
            string query = $"SELECT SUM(Deposit) FROM {depositTable} WHERE CONVERT(DATE, Date) = @Date";
            return ExecuteScalarDecimal(query, date);
        }

        private decimal GetTotalWithdrawalForDate(DateTime date, string accountType)
        {
            string withdrawalTable = GetWithdrawalTableName(accountType);
            string query = $"SELECT SUM(Withdraw_amount) FROM {withdrawalTable} WHERE CONVERT(DATE, Date) = @Date";
            return ExecuteScalarDecimal(query, date);
        }

        private decimal ExecuteScalarDecimal(string query, DateTime date)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date.Date);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
                return 0;
            }
        }

        private string GetDepositTableName(string accountType)
        {
            accountType = accountType.Trim(); // Removes extra spaces
            switch (accountType)
            {
                case "Poddo":
                    return "PoddoAccountCashTB";
                case "Divisaru":
                    return "DivisaruAccountCashTB";
                case "Garusaru":
                    return "GarusaruAccountCashTB";
                default:
                    throw new ArgumentException($"Invalid account type: {accountType}");
            }
        }

        
            
           

        private string GetWithdrawalTableName(string accountType)
        {
            accountType = accountType.Trim(); // Removes extra spaces
            switch (accountType)
            {
                case "Poddo":
                    return "PoddoWithdrawalsTB";
                case "Divisaru":
                    return "DivisaruWithdrawalsTB";
                case "Garusaru":
                    return "GarusaruWithdrawalsTB";
                default:
                    throw new ArgumentException($"Invalid account type: {accountType}");
            };
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            if (!ValidateSelection()) return;

            string accountType = AccountTyTB.SelectedItem.ToString();
            decimal totalDeposit = GetTotalDepositForDate(DateTime.Today, accountType); // Get total deposit
            decimal totalWithdrawal = GetTotalWithdrawalForDate(DateTime.Today, accountType);
            DateTime currentDate = DateTime.Today;

            // Include TotalDeposit in the INSERT statement
            string query = "INSERT INTO DEAccountSummaryTB (AccountType, TotalDeposit, TotalWithdrawal, Date) " +
                           "VALUES (@AccountType, @TotalDeposit, @TotalWithdrawal, @Date)";

            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountType", accountType);
                        command.Parameters.AddWithValue("@TotalDeposit", totalDeposit); // Add totalDeposit to query
                        command.Parameters.AddWithValue("@TotalWithdrawal", totalWithdrawal);
                        command.Parameters.AddWithValue("@Date", currentDate);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data saved successfully.");
                UpdateDataForSelection();
                populate(); // Refresh the DataGridView after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }
        private void DeleteData(string accountType)
        {
            string query = "DELETE FROM DEAccountSummaryTB WHERE AccountType = @AccountType";

            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountType", accountType);

                        connection.Open();
                        command.ExecuteNonQuery();

                        MessageBox.Show("Record deleted successfully.");
                        UpdateDataForSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data: {ex.Message}");
            }
        }





        private void Delete_Click(object sender, EventArgs e)
        {
            // Ensure a valid row is selected
            if (DERDGV.SelectedRows.Count > 0)
            {
                // Get the AccountType from the selected row
                string accountType = DERDGV.SelectedRows[0].Cells["AccountType"].Value.ToString();

                // Construct the DELETE query to remove only the selected row
                string query = "DELETE FROM DEAccountSummaryTB WHERE AccountType = @AccountType AND Date = @Date";

                try
                {
                    using (SqlConnection connection = new SqlConnection(conString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters to prevent SQL injection
                            command.Parameters.AddWithValue("@AccountType", accountType);
                            // Use the date from the selected row as well
                            DateTime selectedDate = Convert.ToDateTime(DERDGV.SelectedRows[0].Cells["Date"].Value);
                            command.Parameters.AddWithValue("@Date", selectedDate.Date);

                            connection.Open();
                            command.ExecuteNonQuery();

                            MessageBox.Show("Record deleted successfully.");
                            populate(); // Refresh the DataGridView after deletion
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

    

    private void DERDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = DERDGV.Rows[e.RowIndex];

                // Retrieve values from the clicked row
                string accountType = row.Cells["AccountType"].Value.ToString();
               
                string totalDeposit = row.Cells["TotalDeposit"].Value.ToString();
                string totalWithdrawal = row.Cells["TotalWithdrawal"].Value.ToString();
                string Date = row.Cells["Date"].Value.ToString();

                // Display values in the appropriate textboxes
                AccountTyTB.Text = accountType;
               
                DepositTB.Text = totalDeposit;
                WithdrawTB.Text = totalWithdrawal;
                DateTB.Text = Date;

                // Optionally enable delete functionality with this row
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            CDashbord obj = new CDashbord();
            obj.Show();
            this.Hide();
        }

        private void PrintBT_Click(object sender, EventArgs e)
        {
            DECReport obj = new DECReport();
            obj.Show();
            
        }
    }
}
