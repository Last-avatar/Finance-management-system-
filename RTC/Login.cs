using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Admin_Login obj = new Admin_Login();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUserType = UserTypeCB.SelectedItem.ToString();

            switch (selectedUserType)
            {
                case "Cashier":
                    CashierLogin cashierLogin = new CashierLogin();
                    cashierLogin.Show();
                    this.Hide();
                    break;

                case "Manager":
                    MLogin mLogin = new MLogin();
                    mLogin.Show();
                    this.Hide();
                    break;
                case "Saving Excecutive":
                    SELogin seLogin = new SELogin();
                    seLogin.Show();
                    this.Hide();
                    break;
                case "Loan Excecutive":
                    LELogin leLogin = new LELogin();
                    leLogin.Show();
                    this.Hide();
                    break;
                case "DGM":
                    DGMLogin dgmLogin = new DGMLogin();
                    dgmLogin.Show();
                    this.Hide();
                    break;
                case "CFO":
                    CFOLogin cfoLogin = new CFOLogin();
                    cfoLogin.Show();
                    this.Hide();
                    break;
                case "CEO":
                    CEOLogin ceoLogin = new CEOLogin();
                    ceoLogin.Show();
                    this.Hide();
                    break;

                default:
                    MessageBox.Show("Please select a valid user type.");
                    break;
            }

        }
    }
}
