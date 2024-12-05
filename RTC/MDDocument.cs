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
    public partial class MDDocument : Form
    {
        public MDDocument()
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
                String Query = "select * from DivisaruDoumentTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                DADGV.DataSource = ds.Tables[0];
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO DivisaruDoumentTB (DID,Name,NIC,Contact_No,Address,Gender,Date) VALUES (@DID,@DN,@DNic,@DC,@DA,@DG,@DD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@DID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@DN", NameTB.Text);
                    cmd.Parameters.AddWithValue("@DNic", NICTB.Text);
                    cmd.Parameters.AddWithValue("@DC", ContactTB.Text);
                    cmd.Parameters.AddWithValue("@DA", AddressTB.Text);
                    cmd.Parameters.AddWithValue("@DG", GenderTB.Text);
                    cmd.Parameters.AddWithValue("@DD", dateTB.Value.Date);



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
            string query = "SELECT DID FROM DivisaruAccountTB WHERE Status = 'Approve'";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                AccountTB.DataSource = dt;
                AccountTB.DisplayMember = "DID";  // Ensure this matches the column name in your database
                AccountTB.ValueMember = "DID";
                AccountTB.SelectedIndex = -1; // Set default to no selection
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
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
            string query = "SELECT  Name,NIC,Contact_No,Address,Gender,Date,Status FROM DivisaruAccountTB WHERE DID = @AccountNo AND Status = 'Approve'";

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

        private void label13_Click(object sender, EventArgs e)
        {
            MDashbord obj = new MDashbord();
            obj.Show();
            this.Hide();
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertDoument();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MDashbord obj = new MDashbord();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            MPDocument obj = new MPDocument();
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

        private void PrintBT_Click(object sender, EventArgs e)
        {
            DDReports obj = new DDReports();
            obj.Show();
        }
    }
}
