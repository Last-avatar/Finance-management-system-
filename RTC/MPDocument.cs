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
    public partial class MPDocument : Form
    {
        public MPDocument()
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
                String Query = "select * from PoddoDoumentTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                PADGV.DataSource = ds.Tables[0];
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

            if (AccountTB.SelectedIndex == -1 || NameTB.Text == "" || ContactTB.Text == "" || AddressTB.Text == "" || GenderTB.Text == "")
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO PoddoDoumentTB (PID,Name,Contact_No,Address,Gender,Date) VALUES (@PID,@PN,@PC,@PA,@PG,@PD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@PID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@PN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@PC", ContactTB.Text);
                    cmd.Parameters.AddWithValue("@PA", AddressTB.Text);
                    cmd.Parameters.AddWithValue("@PG", GenderTB.Text);
                    cmd.Parameters.AddWithValue("@PD", dateTB.Value.Date);
                   
                  

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Success message
                    MessageBox.Show("Document Created Successfully!");

                    // Clear fields and reload data
                    AccountTB.SelectedIndex = -1;
                    NameTB.Clear();
                    ContactTB.Clear();
                    AddressTB.Clear();
                    GenderTB.Clear();
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
            string query = "SELECT PID FROM PoddoAccountTB WHERE Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountTB.DataSource = dt;
                AccountTB.DisplayMember = "PID";  // Ensure this matches the column name in your database
                AccountTB.ValueMember = "PID";
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
            string query = "SELECT  HName,Contact_no,Address,Gender,Date,Status FROM PoddoAccountTB WHERE PID = @AccountNo AND Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNo", accountNo);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Display fetched values in the respective fields
                  
                    NameTB.Text = reader["HName"].ToString();
                    ContactTB.Text = reader["Contact_no"].ToString();
                    AddressTB.Text = reader["Address"].ToString();
                    GenderTB.Text = reader["Gender"].ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            PDReports obj = new PDReports();
            obj.Show();
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

        private void label6_Click(object sender, EventArgs e)
        {
            MLDocument obj = new MLDocument();
            obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
