using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBankLogicLayer;

namespace ZakaBank_24.User_Forms
{
    public partial class ShowChangePasswordForm : Form
    {
        private int UserID = -1;
        private clsUsers _User;

        public ShowChangePasswordForm(int userID)
        {
            InitializeComponent();
            UserID = userID;
        }

        private async void ShowChangePasswordForm_Load(object sender, EventArgs e)
        {
            _User = await clsUsers.FindByUserIDAsync(UserID);

            if (_User != null)
            {
                txtCurrentPass.Select();

                ctrlUserInfoCard1.LoadUserInfo(UserID);
            }
            else
            {
                MessageBox.Show($"This Form Will Be Closed because User with ID {UserID} is Not Found",
                                  "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void txtCurrentPass_Validating(object sender, CancelEventArgs e)
        {
            if (txtCurrentPass.Text.Length != 64)
            {
                if (clsUtil.ComputeHash(txtCurrentPass.Text) != _User.PassWordHash)
                {
                    e.Cancel = true;
                    txtCurrentPass.Focus();
                    errorProvider1.SetError(txtCurrentPass, "You Have To Set The Current Password ❓");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtCurrentPass, "");
                }
            }
            else
            {
                if (txtCurrentPass.Text != _User.PassWordHash)
                {
                    e.Cancel = true;
                    txtCurrentPass.Focus();
                    errorProvider1.SetError(txtCurrentPass, "You Have To Set The Current Password ❓");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtCurrentPass, "");
                }
            }

        }

        private void txtNewPass_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPass.Text))
            {
                e.Cancel = true;
                txtNewPass.Focus();
                errorProvider1.SetError(txtNewPass, "You Have To Set The New Password ❓");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPass, "");
            }
        }

        private void txtConfrimPass_Validating(object sender, CancelEventArgs e)
        {

            if (txtConfrimPass.Text != txtNewPass.Text)
            {
                e.Cancel = true;
                txtConfrimPass.Focus();
                errorProvider1.SetError(txtConfrimPass, "The New Password Should Match The New Password ❗");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfrimPass, "");
            }
        }


        /// <summary>
        /// Fill All The Error Messages In a List and Show them To The User At Once
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(txtCurrentPass.Text.Trim()))
                errors.Add("Current Pass cannot be blank.");

            if (string.IsNullOrEmpty(txtNewPass.Text.Trim()))
                errors.Add(" New Password cannot be blank.");

            if (txtConfrimPass.Text.Trim() != txtNewPass.Text.Trim())
                errors.Add("Password confirmation does not match.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            // check if The Current User Is CHanging His Password So That We DOnt Have To Encrypt it Only Once
            if (_User.PassWordHash != txtNewPass.Text)
                _User.PassWordHash = clsUtil.ComputeHash(txtNewPass.Text);

            if (await _User.SaveAsync())
                MessageBox.Show("Password Changed Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed To Change Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
