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
    public partial class MLDocument : Form
    {
        public MLDocument()
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
                String Query = "select * from LoanDoumentTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                LADGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        int key = 0;




        private void InsertDoument()
        {
            SqlConnection con = new SqlConnection(conString);

            if (AccountTB.SelectedIndex == -1 || AccountTyTB.Text == "" || NameTB.Text == "" || NICTB.Text == "" || AmmountTB.Text == "" || RateTB.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    // Ensure the value from AccountNoTB is properly fetched
                    string accountNo = AccountTB.SelectedValue != null ? AccountTB.SelectedValue.ToString() : "";

                    // Prepare the SQL command
                    SqlCommand cmd = new SqlCommand("INSERT INTO LoanDoumentTB (LID,Account_type,Name,NIC,Loan_amount,Loan_Rate,Date) VALUES (@LID,@LAT,@LN,@LNic,@La,@LR,@LD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@LID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@LAT", AccountTyTB.Text);
                    cmd.Parameters.AddWithValue("@LN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@LNic", NICTB.Text);
                    cmd.Parameters.AddWithValue("@La", AmmountTB.Text);
                    cmd.Parameters.AddWithValue("@LR", RateTB.Text);
                    cmd.Parameters.AddWithValue("@LD", dateTB.Value.Date);



                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Success message
                    MessageBox.Show("Document Created Successfully!");

                    // Clear fields and reload data
                    AccountTB.SelectedIndex = -1;
                    AccountTyTB.Clear();
                    NameTB.Clear();
                    AmmountTB.Clear();
                    RateTB.Clear();
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
        }


        private void LoadAccountNumbers()
        {
            string query = "SELECT LID FROM LoanTB ";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountTB.DataSource = dt;
                AccountTB.DisplayMember = "LID";  // Ensure this matches the column name in your database
                AccountTB.ValueMember = "LID";
                AccountTB.SelectedIndex = -1; // Set default to no selection
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void AccountTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountTB.SelectedValue == null || AccountTB.SelectedValue is DataRowView)
            {
                return;
            }

            string accountNo = AccountTB.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(accountNo))
            {
                LoadAccountDetails(accountNo);
            }
        }
        private void LoadAccountDetails(string accountNo)
        {
            string query = "SELECT  Account_type, Name,NIC,Date,Loan_amount,Loan_Rate FROM LoanTB WHERE LID = @AccountNo";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Display fetched values in the respective fields
                    AccountTyTB.Text = reader["Account_type"].ToString();
                    NameTB.Text = reader["Name"].ToString();
                    NICTB.Text = reader["NIC"].ToString();
                    dateTB.Text = reader["Date"].ToString();
                    AmmountTB.Text = reader["Loan_amount"].ToString();
                    RateTB.Text = reader["Loan_Rate"].ToString();


                }
                conn.Close();
            }
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertDoument();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            MDashbord obj = new MDashbord();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            MPDocument obj = new MPDocument();
            obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            MDDocument obj = new MDDocument();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            MGDocument obj = new MGDocument();
            obj.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            MIDocument obj = new MIDocument();
            obj.Show();
            this.Hide();
        }
    }
    
}
