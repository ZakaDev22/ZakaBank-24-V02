using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBankLogicLayer;

namespace ZakaBank_24.User_Forms
{
    public partial class ShowAddEditeUsersForm : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        clsUsers _User;
        private int _Permissions = 0;
        private bool _OnPersonSelected = false;
        private bool _allowTabSwitch = false;


        public ShowAddEditeUsersForm()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public ShowAddEditeUsersForm(int userID)
        {
            InitializeComponent();
            _UserID = userID;
            _Mode = enMode.Update;
        }

        private void _ResetDefualtValues()
        {
            lbTitle.Text = _Mode == enMode.AddNew ? "Add New User" : "Update User ";
            this.Text = lbTitle.Text;

            _User = _Mode == enMode.AddNew ? new clsUsers() : _User;

            if (_Mode == enMode.AddNew)
            {
                ctrlPersonInfoCardWithFilter1.FilterFocus();
            }


            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfrimPass.Text = "";
            tgsIsActive.Checked = true;


        }

        private async void _LoadData()
        {

            _User = await clsUsers.FindByUserIDAsync(_UserID);
            ctrlPersonInfoCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the person was not found
            lbTitle.Text += _User.ID.ToString();
            lblUserID.Text = _User.ID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.PassWordHash;
            txtConfrimPass.Text = _User.PassWordHash;
            tgsIsActive.Checked = _User.IsActive;
            ctrlPersonInfoCardWithFilter1.LoadPersonInfo(_User.PersonID);

            if (_User.Permissions == (int)clsUsers.enPermissions.All)
            {
                chkAll.Checked = true;
            }
            else
            {
                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.People))
                {
                    chkPeople.Checked = true;
                }

                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.Users))
                {
                    chkUsers.Checked = true;
                }

                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.Clients))
                {
                    chkClients.Checked = true;
                }

                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.Transactions))
                {
                    chkTransactions.Checked = true;
                }

                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.Transfers))
                {
                    chkTransfers.Checked = true;
                }

                if (clsValidation.IsUserHaveThisPermission(_User.Permissions, (int)clsUsers.enPermissions.LoginRegisters))
                {
                    chkLoginRegisters.Checked = true;
                }

            }


        }
        private void ShowAddEditeUsersForm_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void UpdatePermissions(CheckBox chkBox, int permissionValue)
        {
            if (chkAll.Checked) return;

            if (chkBox.Checked)
            {
                _Permissions |= permissionValue;
            }
            else
            {
                _Permissions &= ~permissionValue; // Remove permission
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                ctrlPersonInfoCardWithFilter1.Enabled = false;
                _allowTabSwitch = true; // Allow the tab switch
                tabUserControl.SelectedTab = tabUserControl.TabPages[1];
                _allowTabSwitch = false; // Reset after switching
                return;
            }

            // Incase of add new mode.
            if (ctrlPersonInfoCardWithFilter1.PersonID != -1)
            {
                if (await clsUsers.ExistsByPersonIDAsync(ctrlPersonInfoCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonInfoCardWithFilter1.FilterFocus();
                }
                else
                {
                    _allowTabSwitch = true; // Allow the tab switch
                    tabUserControl.SelectedTab = tabUserControl.TabPages[1];
                    _allowTabSwitch = false; // Reset after switching
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonInfoCardWithFilter1.FilterFocus();
            }

        }

        /// <summary>
        /// Fill All The Error Messages In a List and Show them To The User At Once
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
                errors.Add("Username cannot be blank.");

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                errors.Add("Password cannot be blank.");

            if (txtConfrimPass.Text.Trim() != txtPassword.Text.Trim())
                errors.Add("Password confirmation does not match.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                _Permissions = (int)clsUsers.enPermissions.All;

                chkPeople.Checked = true;
                chkUsers.Checked = true;
                chkClients.Checked = true;
                chkTransactions.Checked = true;
                chkTransfers.Checked = true;
                chkLoginRegisters.Checked = true;


                chkPeople.Enabled = false;
                chkUsers.Enabled = false;
                chkClients.Enabled = false;
                chkTransactions.Enabled = false;
                chkTransfers.Enabled = false;
                chkLoginRegisters.Enabled = false;


            }
            else
            {
                _Permissions = (int)clsUsers.enPermissions.All;

                chkPeople.Checked = false;
                chkUsers.Checked = false;
                chkClients.Checked = false;
                chkTransactions.Checked = false;
                chkTransfers.Checked = false;
                chkLoginRegisters.Checked = false;


                chkPeople.Enabled = true;
                chkUsers.Enabled = true;
                chkClients.Enabled = true;
                chkTransactions.Enabled = true;
                chkTransfers.Enabled = true;
                chkLoginRegisters.Enabled = true;

            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }


            _User.PersonID = ctrlPersonInfoCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.IsActive = tgsIsActive.Checked;
            _User.Permissions = _Permissions;
            _User.AddedByUserID = clsGlobal._CurrentUser.ID;

            // check if The Current User Is CHanging His Password So That We DOnt Have To Encrypt it Only Once 
            if (_User.PassWordHash != txtPassword.Text.Trim())
            {
                _User.PassWordHash = clsUtil.ComputeHash(txtPassword.Text.Trim());
            }


            if (await _User.SaveAsync())
            {
                lblUserID.Text = _User.ID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lbTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void chkPeople_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkPeople, (int)clsUsers.enPermissions.People);
        }

        private void chkUsers_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkUsers, (int)clsUsers.enPermissions.Users);
        }

        private void chkClients_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkClients, (int)clsUsers.enPermissions.Clients);
        }

        private void chkTransactions_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkTransactions, (int)clsUsers.enPermissions.Transactions);
        }

        private void chkLoginRegisters_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkLoginRegisters, (int)clsUsers.enPermissions.LoginRegisters);
        }

        private void chkTransfers_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePermissions(chkTransfers, (int)clsUsers.enPermissions.Transfers);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonInfoCardWithFilter1_OnPersonSelected(int obj)
        {
            _OnPersonSelected = obj != -1 ? true : false;
        }

        private void ctrlPersonInfoCardWithFilter1_OntxtFilterValueEmpty(bool obj)
        {
            if (obj)
            {
                ctrlPersonInfoCardWithFilter1.ClearTextBox();
            }
        }

        private async void tabUserControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_allowTabSwitch) return; // If the switch is allowed, skip the validation

            if (!_OnPersonSelected)
            {
                e.Cancel = true;
                return;
            }

            // Check if a person is selected
            if (ctrlPersonInfoCardWithFilter1.PersonID != -1)
            {
                // Check if the person already exists as a user
                if (await clsUsers.ExistsByPersonIDAsync(ctrlPersonInfoCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonInfoCardWithFilter1.FilterFocus();
                    e.Cancel = true;  // Prevent tab switch
                    return;
                }
            }
            else
            {
                // Person is not selected
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonInfoCardWithFilter1.FilterFocus();
                e.Cancel = true;  // Prevent tab switch
                return;
            }
        }
    }
}