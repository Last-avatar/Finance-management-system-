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
    public partial class CDGAccount : Form
    {
        public CDGAccount()
        {
            InitializeComponent();
            panelCP.Hide();
            populate();
            GetCategories();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";

        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from GarusaruAccountCashTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                CGDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void GetCategories()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT GID FROM GarusaruAccountTB", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("GID", typeof(int));
            dt.Load(rdr);

            AccountNoTb.ValueMember = "GID"; // Ensure this matches the column in the database
            AccountNoTb.DisplayMember = "GID"; // Optional, can set to another column like account name
            AccountNoTb.DataSource = dt;

            con.Close();
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
            MAccount obj = new MAccount();
            obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            CDDAccount obj = new CDDAccount();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CDGAccount obj = new CDGAccount();
            obj.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            CDIAccount obj = new CDIAccount();
            obj.Show();
            this.Hide();
        }

        private void label26_Click(object sender, EventArgs e)
        {
            CSettings obj = new CSettings();
            obj.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            panelCP.Show(); 
        }

        private void DepositBt_Click(object sender, EventArgs e)
        {
            string accountNo, depositorName, nic;
            double depositAmount;
            DateTime depositDate;

            // Validate inputs
            if (AccountNoTb.SelectedValue != null)
                accountNo = AccountNoTb.SelectedValue.ToString();
            else
            {
                MessageBox.Show("Please select a valid account number.");
                return;
            }

            depositorName = NameTB.Text;
            if (string.IsNullOrWhiteSpace(depositorName))
            {
                MessageBox.Show("Please enter the depositor's name.");
                return;
            }

            nic = NICTb.Text;
            if (string.IsNullOrWhiteSpace(nic))
            {
                MessageBox.Show("Please enter the depositor's NIC.");
                return;
            }

            if (!double.TryParse(DepositTB.Text, out depositAmount) || depositAmount <= 0)
            {
                MessageBox.Show("Please enter a valid deposit amount.");
                return;
            }

            if (!DateTime.TryParse(dateTB.Text, out depositDate))
            {
                MessageBox.Show("Please enter a valid date.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();

                    // Check if the account exists and get the current balance
                    string checkAccountQuery = "SELECT ISNULL(Balance, 0) AS Balance FROM GarusaruAccountTB WHERE GID = @accountNo";
                    SqlCommand checkAccountCommand = new SqlCommand(checkAccountQuery, connection);
                    checkAccountCommand.Parameters.AddWithValue("@accountNo", accountNo);

                    object result = checkAccountCommand.ExecuteScalar();
                    double currentBalance = Convert.ToDouble(result);

                    // Debugging message
                    MessageBox.Show($"Current Balance (before deposit): {currentBalance}");

                    // Update the account balance
                    string updateBalanceQuery = "UPDATE GarusaruAccountTB SET Balance = ISNULL(Balance, 0) + @depositAmount WHERE GID = @accountNo";
                    SqlCommand updateBalanceCommand = new SqlCommand(updateBalanceQuery, connection);
                    updateBalanceCommand.Parameters.AddWithValue("@depositAmount", depositAmount);
                    updateBalanceCommand.Parameters.AddWithValue("@accountNo", accountNo);

                    int rowsAffected = updateBalanceCommand.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Failed to update balance. Account might not exist.");
                        return;
                    }

                    // Log the deposit in PoddoAccountCashTB
                    string insertDepositQuery = "INSERT INTO GarusaruAccountCashTB (GID, Name, NIC, Deposit, Date) " +
                                                "VALUES (@accountNo, @depositorName, @nic, @depositAmount, @depositDate)";
                    SqlCommand insertDepositCommand = new SqlCommand(insertDepositQuery, connection);
                    insertDepositCommand.Parameters.AddWithValue("@accountNo", accountNo);
                    insertDepositCommand.Parameters.AddWithValue("@depositorName", depositorName);
                    insertDepositCommand.Parameters.AddWithValue("@nic", nic);
                    insertDepositCommand.Parameters.AddWithValue("@depositAmount", depositAmount);
                    insertDepositCommand.Parameters.AddWithValue("@depositDate", depositDate);
                    insertDepositCommand.ExecuteNonQuery();

                    MessageBox.Show("Deposit successfully recorded and balance updated!");

                    // Refresh the DataGridView
                    NameTB.Clear();
                    NICTb.Clear();
                    DepositTB.Clear();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\nStack Trace: {ex.StackTrace}");
                }
            }

        }

        private void CGDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int key = 0;
            AccountNoTb.Text = CGDGV.SelectedRows[0].Cells[1].Value.ToString();
            NameTB.Text = CGDGV.SelectedRows[0].Cells[2].Value.ToString();
            NICTb.Text = CGDGV.SelectedRows[0].Cells[3].Value.ToString();
            dateTB.Text = CGDGV.SelectedRows[0].Cells[4].Value.ToString();
            DepositTB.Text = CGDGV.SelectedRows[0].Cells[5].Value.ToString();

            if (AccountNoTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(CGDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
