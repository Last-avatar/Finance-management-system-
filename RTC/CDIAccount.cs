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
using System.Xml.Linq;

namespace RTC
{
    public partial class CDIAccount : Form
    {
        public CDIAccount()
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
                String Query = "select * from FDAccountCashTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                FCDDGV.DataSource = ds.Tables[0];
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
                    string query = "SELECT FDID FROM FDAccountTB";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable accounts = new DataTable();
                    adapter.Fill(accounts);

                    AccountNoTB.DataSource = accounts;
                    AccountNoTB.DisplayMember = "FDID"; // What the user sees
                    AccountNoTB.ValueMember = "FDID";   // The actual value
                    AccountNoTB.SelectedIndex = -1;    // Ensure no default selection
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading accounts: " + ex.Message);
            }
        }


        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void dateTB_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
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

        private void AccountNoTB_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (AccountNoTB.SelectedValue is DataRowView) return; // Prevent using DataRowView directly

                string accountNo = AccountNoTB.SelectedValue?.ToString();

                if (!string.IsNullOrEmpty(accountNo))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(conString))
                        {
                            conn.Open();
                            string query = "SELECT Name FROM FDAccountTB WHERE FDID = @AccountNo";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                                object result = cmd.ExecuteScalar();

                                NameTB.Text = result != null ? result.ToString() : string.Empty;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error fetching account name: " + ex.Message);
                    }
                }
                else
                {
                    NameTB.Clear();
                }
            

        }

        private void DepositBt_Click(object sender, EventArgs e)
        {
                // Deposit Button Click Event
                if (ValidateInputs())
                {
                    string accountNo = AccountNoTB.SelectedValue.ToString();
                    string name = NameTB.Text;
                    DateTime depositDate = DdateTB.Value;
                    DateTime maturedDate = MdateTB.Value;
                    decimal depositAmount = decimal.Parse(DepositTB.Text);
                    string interestType = ITypeCB.Text;
                    decimal interestRate = decimal.Parse(IRateCb.Text);

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(conString))
                        {
                            conn.Open();

                            // Step 1: Insert into FDAccountCashTB
                            string insertQuery = "INSERT INTO FDAccountCashTB (FDID, Name, DDate, MDate, Deposit, Interest_Type, Interest_Rate) " +
                                                 "VALUES (@AccountNo, @Name, @DepositDate, @MaturedDate, @DepositAmount, @InterestType, @InterestRate)";
                            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                                cmd.Parameters.AddWithValue("@Name", name);
                                cmd.Parameters.AddWithValue("@DepositDate", depositDate);
                                cmd.Parameters.AddWithValue("@MaturedDate", maturedDate);
                                cmd.Parameters.AddWithValue("@DepositAmount", depositAmount);
                                cmd.Parameters.AddWithValue("@InterestType", interestType);
                                cmd.Parameters.AddWithValue("@InterestRate", interestRate);

                                cmd.ExecuteNonQuery();
                            }

                            // Step 2: Fetch the current balance from FDAccountTB
                            decimal currentBalance = 0;
                            string selectQuery = "SELECT Balance FROM FDAccountTB WHERE FDID = @AccountNo";
                            using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    currentBalance = Convert.ToDecimal(result);
                                }
                            }

                            // Step 3: Update the balance in FDAccountTB
                            decimal newBalance = currentBalance + depositAmount;
                            string updateQuery = "UPDATE FDAccountTB SET Balance = @Balance WHERE FDID = @AccountNo";
                            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@Balance", newBalance);
                                cmd.Parameters.AddWithValue("@AccountNo", accountNo);

                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Deposit recorded successfully! New Balance: " + newBalance.ToString("C"));
                        NameTB.Clear();
                       
                        DepositTB.Clear();
                        populate();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during deposit: " + ex.Message);
                    }
                
            }


        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(AccountNoTB.Text) ||
                string.IsNullOrEmpty(NameTB.Text) ||
                string.IsNullOrEmpty(DepositTB.Text) ||
                string.IsNullOrEmpty(ITypeCB.Text) ||
                string.IsNullOrEmpty(IRateCb.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return false;
            }

            if (!decimal.TryParse(DepositTB.Text, out _) ||
                !decimal.TryParse(IRateCb.Text, out _))
            {
                MessageBox.Show("Invalid numeric values for Deposit Amount or Interest Rate.");
                return false;
            }

            return true;
        }

        private void FCDDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int key = 0;
            AccountNoTB.Text = FCDDGV.SelectedRows[0].Cells[1].Value.ToString();
            NameTB.Text = FCDDGV.SelectedRows[0].Cells[2].Value.ToString();
            DdateTB.Text = FCDDGV.SelectedRows[0].Cells[3].Value.ToString();
            MdateTB.Text = FCDDGV.SelectedRows[0].Cells[4].Value.ToString();
            DepositTB.Text = FCDDGV.SelectedRows[0].Cells[5].Value.ToString();
            ITypeCB.Text = FCDDGV.SelectedRows[0].Cells[6].Value.ToString();
            IRateCb.Text = FCDDGV.SelectedRows[0].Cells[7].Value.ToString();

            if (AccountNoTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(FCDDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
