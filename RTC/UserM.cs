﻿using System;
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
    public partial class UserM : Form
    {
        public UserM()
        {
            InitializeComponent();
            populate();

        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";
        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from CashierTB";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                CUserDGV.DataSource = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void EditUser()
        {
            SqlConnection con = new SqlConnection(conString);

            if (UNameTB.Text == "" || UpasswordTb.Text == "" || UphoneNoTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {
                try
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("update CashierTB set CName = @CN, CPassword = @CPa, CPhone = @CPh where CID = @Ckey", con);
                    cmd.Parameters.AddWithValue("@CN", UNameTB.Text);
                    cmd.Parameters.AddWithValue("@CPa", UpasswordTb.Text);
                    cmd.Parameters.AddWithValue("@CPh", UphoneNoTb.Text);
                    cmd.Parameters.AddWithValue("@Ckey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated!!!");
                    UNameTB.Clear();
                    UpasswordTb.Clear();
                    UphoneNoTb.Clear();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        int key = 0;
        private void DeleteUser()
        {
            SqlConnection con = new SqlConnection(conString);
            if (key == 0)
            {
                MessageBox.Show("Select a User!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CashierTB where CID = @Ckey", con);
                    cmd.Parameters.AddWithValue("@Ckey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted!!!");
                    con.Close();
                    UNameTB.Clear();
                    UpasswordTb.Clear();
                    UphoneNoTb.Clear();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }


        }
        private void InsertUser()
        {
            SqlConnection con = new SqlConnection(conString);

            if (UNameTB.Text == "" || UpasswordTb.Text == "" || UphoneNoTb.Text == "")
            {
                MessageBox.Show("Missing Information!!!");
            }
            else
            {

                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CashierTB (CName,CPassword,CPhone) values(@CN,@CPa,@CPh)", con);
                    cmd.Parameters.AddWithValue("@CN", UNameTB.Text);
                    cmd.Parameters.AddWithValue("@CPa",UpasswordTb.Text);
                    cmd.Parameters.AddWithValue("@CPh",UphoneNoTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated!!!");
                    UNameTB.Clear();
                    UpasswordTb.Clear();
                    UphoneNoTb.Clear();
                    con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {
            ADashbord obj = new ADashbord();
            obj.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            BranchM obj = new BranchM();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditUser();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InsertUser();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            SEUserM obj = new SEUserM();
            obj.Show();
            this.Hide();
        }

        private void DeleteBt_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        private void CUserDGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            UNameTB.Text = CUserDGV.SelectedRows[0].Cells[1].Value.ToString();
            UpasswordTb.Text = CUserDGV.SelectedRows[0].Cells[2].Value.ToString();
            UphoneNoTb.Text = CUserDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (UNameTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(CUserDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            UserM obj = new UserM();
            obj.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            MUserM obj = new MUserM();
            obj.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            LEUserM obj = new LEUserM();
            obj.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            DGMUserM obj = new DGMUserM();
            obj.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            CFOUserM obj = new CFOUserM();
            obj.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CEOUserM obj = new CEOUserM();
            obj.Show();
            this.Hide();
        }
    }
}
