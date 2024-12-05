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
    public partial class IssueLoan : Form
    {
        public IssueLoan()
        {
            InitializeComponent();
            LoadAccountTypes();
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
                lRDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void LoadAccountTypes()
        {
            AccountTyTB.Items.Add("Divisaru");
            AccountTyTB.Items.Add("Garusaru");
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

        private void dateTB_ValueChanged(object sender, EventArgs e)
        {

        }

        private void AccountTyTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountType = AccountTyTB.SelectedItem.ToString();
            LoadAccountNumbers(accountType);
        }
        private void LoadAccountNumbers(string accountType)
        {
            string tableName = accountType == "Divisaru" ? "DivisaruAccountTB" : "GarusaruAccountTB";
            string columnName = accountType == "Divisaru" ? "DID" : "GID";
            string query = $"SELECT {columnName} AS AccountNo FROM {tableName}";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountNoTB.DataSource = dt;
                AccountNoTB.DisplayMember = "AccountNo";
                AccountNoTB.ValueMember = "AccountNo";
            }
        }

        private void AccountNoTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountNoTB.SelectedValue == null || AccountNoTB.SelectedValue is DataRowView)
            {
                // Skip if the SelectedValue is null or still a DataRowView
                return;
            }

            string accountType = AccountTyTB.SelectedItem.ToString();
            string accountNo = AccountNoTB.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(accountNo))
            {
                LoadAccountDetails(accountType, accountNo);
            }
        }
        // Load account details based on account number
        private void LoadAccountDetails(string accountType, string accountNo)
        {
            string tableName = accountType == "Divisaru" ? "DivisaruAccountTB" : "GarusaruAccountTB";
            string columnName = accountType == "Divisaru" ? "DID" : "GID";
            string query = $"SELECT Name, NIC FROM {tableName} WHERE {columnName} = @AccountNo";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    NameTB.Text = reader["Name"].ToString();
                    NICTB.Text = reader["NIC"].ToString();
                }
                conn.Close();
            }
        }

        private void RequesBt_Click(object sender, EventArgs e)
        {
                // Validate that all necessary fields are filled
                if (AccountTyTB.SelectedItem == null || AccountNoTB.SelectedValue == null ||
                    string.IsNullOrWhiteSpace(NameTB.Text) || string.IsNullOrWhiteSpace(NICTB.Text) ||
                    RequestTB.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all fields before submitting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gather data for insertion
                string accountType = AccountTyTB.SelectedItem.ToString();
                string accountNo = AccountNoTB.SelectedValue.ToString();
                string name = NameTB.Text;
                string nic = NICTB.Text;
                DateTime requestDate = dateTB.Value;
                string loanStatus = RequestTB.SelectedItem.ToString();

                // Insert into LoanRequestsTB
                string query = "INSERT INTO LoanRequestsTB (AccountType, AccountNo, Name, NIC, RequestDate, LoanStatus) " +
                               "VALUES (@AccountType, @AccountNo, @Name, @NIC, @RequestDate, @LoanStatus)";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AccountType", accountType);
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@NIC", nic);
                    cmd.Parameters.AddWithValue("@RequestDate", requestDate);
                    cmd.Parameters.AddWithValue("@LoanStatus", loanStatus);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear only the fields that were inserted
                            ClearFields();

                            // Refresh the DataGridView to display the new data
                            populate();
                        }
                        else
                        {
                            MessageBox.Show("No data was inserted. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
         
        }
        private void ClearFields()
        {
            // Clear only the fields that are inserted into the database
            NameTB.Clear();   // Clear the Name field
            NICTB.Clear();    // Clear the NIC field
            AccountNoTB.SelectedIndex = -1;  // Deselect Account Number
            RequestTB.SelectedIndex = -1;    // Deselect Loan Status
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ILoan obj = new ILoan();
            obj.Show();
            this.Hide();
        }
    }
}
