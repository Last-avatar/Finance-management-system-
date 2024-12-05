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
    public partial class MIDocument : Form
    {
        public MIDocument()
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
                String Query = "select * from FDDoumentTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                IADGV.DataSource = ds.Tables[0];
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

            if (AccountTB.SelectedIndex == -1 || NameTB.Text == "" || ContactTB.Text == "" || AddressTB.Text == "" || NICTB.Text == "")
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO FDDoumentTB (FDID,Name,NIC,Contact_No,Address,Date) VALUES (@FID,@FN,@FNic,@FC,@FA,@FD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@FID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@FN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@FNic", NICTB.Text);
                    cmd.Parameters.AddWithValue("@FC", ContactTB.Text);
                    cmd.Parameters.AddWithValue("@FA", AddressTB.Text);
                    cmd.Parameters.AddWithValue("@FD", dateTB.Value.Date);



                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Success message
                    MessageBox.Show("Document Created Successfully!");

                    // Clear fields and reload data
                    AccountTB.SelectedIndex = -1;
                    NameTB.Clear();
                    ContactTB.Clear();
                    AddressTB.Clear();
                    NICTB.Clear();
                    
                    StatusTB.Clear();

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
            string query = "SELECT FDID FROM FDAccountTB WHERE Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountTB.DataSource = dt;
                AccountTB.DisplayMember = "FDID";  // Ensure this matches the column name in your database
                AccountTB.ValueMember = "FDID";
                AccountTB.SelectedIndex = -1; // Set default to no selection
            }
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
            string query = "SELECT  Name,NIC,Contact_no,Address,Date,Status FROM FDAccountTB WHERE FDID = @AccountNo AND Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Display fetched values in the respective fields

                    NameTB.Text = reader["Name"].ToString();
                    ContactTB.Text = reader["Contact_no"].ToString();
                    AddressTB.Text = reader["Address"].ToString();
                    NICTB.Text = reader["NIC"].ToString();
                    dateTB.Text = reader["Date"].ToString();
                    StatusTB.Text = reader["Status"].ToString();


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

        }

        private void label6_Click(object sender, EventArgs e)
        {
            MLDocument obj = new MLDocument();
            obj.Show();
            this.Hide();
        }
    }
}
