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
    public partial class PCollections : Form
    {
        public PCollections()
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
                String Query = "select * from PoddoCollectionsTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                PCDGV.DataSource = ds.Tables[0];
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

            if (AccountTB.SelectedIndex == -1 || NameTB.Text == "" || AmountTB.Text == "" )
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO PoddoCollectionsTB (PID,Name,Amount,Date) VALUES (@PID,@PN,@PA,@PD)", con);

                    // Add parameters
                    cmd.Parameters.AddWithValue("@PID", accountNo); // Use SelectedValue for LID
                    cmd.Parameters.AddWithValue("@PN", NameTB.Text);               
                    cmd.Parameters.AddWithValue("@PA", AmountTB.Text);
                    cmd.Parameters.AddWithValue("@PD", dateTB.Value.Date);



                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Success message
                    MessageBox.Show("Document Created Successfully!");

                    // Clear fields and reload data
                    AccountTB.SelectedIndex = -1;
                    NameTB.Clear();
                    AmountTB.Clear();
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

        private void label15_Click(object sender, EventArgs e)
        {
            GCollections obj = new GCollections();
            obj.Show();
            this.Hide();
        }

        private void PrintBT_Click(object sender, EventArgs e)
        {
            PCReports obj = new PCReports();
            obj.Show();
        
        }

        private void label18_Click(object sender, EventArgs e)
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
            string query = "SELECT  HName FROM PoddoAccountTB WHERE PID = @AccountNo AND Status = 'Approve'";

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
                   


                }
                conn.Close();
            }
        }

        private void SaveBt_Click(object sender, EventArgs e)
        {
            InsertDoument();
        }
        private void DeleteDocument()
        {
            if (key == 0)
            {
                MessageBox.Show("Select a record to delete!");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string query = "DELETE FROM PoddoCollectionsTB WHERE CID = @Key"; // Replace 'ID' with your actual column name
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Key", key);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record deleted successfully!");
                    }
                }
                populate(); // Refresh the DataGridView
                key = 0;    // Reset the key
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBT_Click(object sender, EventArgs e)
        {
            DeleteDocument();
        }

        private void PCDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = PCDGV.Rows[e.RowIndex];
                key = Convert.ToInt32(row.Cells[0].Value); // Replace 0 with the index of your key column
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            SEDashbord obj = new SEDashbord();
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
            DCollections obj = new DCollections();
            obj.Show();
            this.Hide();
        }
    }
}
