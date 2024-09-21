using System;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.Main_And_Login_Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24
{
    public partial class LoginForm : Form
    {
        private clsLoginRegisters _loginRegister;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsUsers user = await clsUsers.FindByUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if (user is null)
            {
                MessageBox.Show("Error, The Information Is False, Invalid UserName Or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            clsGlobal._CurrentUser = user;

            if (user.IsActive)
            {

                if (tgsRemeberMe.Checked)
                {
                    //store username and password
                    if (txtPassword.Text.Length != 64)
                        clsGlobal.RememberUsernameAndPasswordUsingRegistry(txtUserName.Text.Trim(), clsUtil.ComputeHash(txtPassword.Text.Trim()));
                    else
                        clsGlobal.RememberUsernameAndPasswordUsingRegistry(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                }
                else
                {

                    // remove User Name And The Password From Registry
                    clsGlobal.RememberUsernameAndPasswordUsingRegistry("", "");

                }


                _loginRegister = new clsLoginRegisters();

                _loginRegister.UserID = user.ID;
                _loginRegister.LoginDateTime = DateTime.Now;

                if (await _loginRegister.Save())
                {
                    //      
                    MainForm frm = new MainForm(this, _loginRegister.ID);
                    this.Hide();
                    frm.ShowDialog();

                }
                else
                {
                    MessageBox.Show("error, something went wrong with login Register Record!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
            else
            {
                MessageBox.Show("This Account Is Not Active, Please Connect Your Admin", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string UserName = string.Empty;
            string Password = string.Empty;

            if (clsGlobal.GetStoredCredentialFromRegistry(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                tgsRemeberMe.Checked = true;
            }
            else
            {
                txtUserName.Focus();
                tgsRemeberMe.Checked = false;
            }
        }

        private void txtUserName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // is User Existe by User Name 

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Please Enter Your UserName");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Please Enter Your Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

    }
}
