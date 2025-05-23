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
    public partial class CFODashbord : Form
    {
        public CFODashbord()
        {
            InitializeComponent();
            populate();
            panel6.Hide();
        }
        public String conString = "Data Source=CHETHANA;Initial Catalog=RTC;Integrated Security=True;";

        private void populate()
        {
            try
            {

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                String Query = "select * from FDWithdrawalsTB";
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
        private void SaveBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);

            if (StatusTB.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!!!");
                return;
            }

            try
            {
                con.Open();

                // Use the selected value from the combo box, not the index
                SqlCommand cmd = new SqlCommand("UPDATE FDWithdrawalsTB SET Status = @FS WHERE FDWID = @Fkey", con);
                cmd.Parameters.AddWithValue("@FS", StatusTB.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Fkey", key);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Account Updated!!!");
                }
                else
                {
                    MessageBox.Show("No record found to update.");
                }

                // Clear fields and r Acefresh data grid view

                AccountNoTB.Clear();
                AmountTB.Clear();

               


                StatusTB.SelectedIndex = -1; // Reset the combo box

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

        private void PADGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            AccountNoTB.Text = PADGV.SelectedRows[0].Cells[1].Value.ToString();
            AmountTB.Text = PADGV.SelectedRows[0].Cells[3].Value.ToString();
            dateTB.Text = PADGV.SelectedRows[0].Cells[2].Value.ToString();
            StatusTB.Text = PADGV.SelectedRows[0].Cells[4].Value.ToString();

            if (AccountNoTB.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PADGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            panel6.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            CFOSettings obj = new CFOSettings();
            obj.Show();
            this.Hide();
        }
    }   
}
