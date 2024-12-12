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
    public partial class MGDocument : Form
    {
        public MGDocument()
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
                String Query = "select * from GarusaruDoumentTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                GADGV.DataSource = ds.Tables[0];
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

            if (AccountTB.SelectedIndex == -1 || NameTB.Text == "" || ContactTB.Text == "" || AddressTB.Text == "" || NICTB.Text == "" || GenderTB.Text == "")
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO GarusaruDoumentTB (GID,Name,NIC,Contact_No,Address,Gender,Date) VALUES (@GID,@GN,@GNic,@GC,@GA,@GG,@GD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@GID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@GN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@GNic", NICTB.Text);
                    cmd.Parameters.AddWithValue("@GC", ContactTB.Text);
                    cmd.Parameters.AddWithValue("@GA", AddressTB.Text);
                    cmd.Parameters.AddWithValue("@GG", GenderTB.Text);
                    cmd.Parameters.AddWithValue("@GD", dateTB.Value.Date);



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
            string query = "SELECT GID FROM GarusaruAccountTB WHERE Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountTB.DataSource = dt;
                AccountTB.DisplayMember = "GID";  // Ensure this matches the column name in your database
                AccountTB.ValueMember = "GID";
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
            string query = "SELECT  Name,NIC,Contact_No,Address,Gender,Date,Status FROM GarusaruAccountTB WHERE GID = @AccountNo AND Status = 'Approve'";

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
                    GenderTB.Text = reader["Gender"].ToString();
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

        private void PrintBT_Click(object sender, EventArgs e)
        {
            GDReports obj = new GDReports();
            obj.Show();
        }
    }
}
