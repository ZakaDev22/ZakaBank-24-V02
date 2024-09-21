using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Client_Forms
{

    public partial class ShowAddEditCLientsForm : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int ClientID = -1;
        clsClients _Client;
        private bool _OnPersonSelected = false;
        private bool _allowTabSwitch = false;
        private clsAccountTypes _AccountTypes;

        public ShowAddEditCLientsForm()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public ShowAddEditCLientsForm(int clientID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            ClientID = clientID;
        }

        private async Task _LoadAccountTypesInComboBox()
        {
            var dataTable = await clsAccountTypes.GetAllAccountTypesAsync();

            foreach (DataRow row in dataTable.Rows)
            {
                cbAccountTypes.Items.Add(row["Name"]);
            }
        }

        private void _ResetDefaultValues()
        {
            lbTitle.Text = _Mode == enMode.AddNew ? "Add New Client" : "Update Client ";
            this.Text = lbTitle.Text;

            _Client = _Mode == enMode.AddNew ? new clsClients() : _Client;

            if (_Mode == enMode.AddNew)
            {
                ctrlPersonInfoCardWithFilter1.FilterFocus();
            }

            txtAccountNo.Enabled = true;
            cbAccountTypes.SelectedIndex = 0;
            txtAccountNo.Clear();
            txtPinCode.Clear();
            txtBalance.Clear();
            lblClientID.Text = string.Empty;
        }

        private async void _LoadData()
        {

            _Client = await clsClients.FindByClientIDAsync(ClientID);
            ctrlPersonInfoCardWithFilter1.FilterEnabled = false;

            if (_Client == null)
            {
                MessageBox.Show("No Client with ID = " + ClientID, "Client Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the client was not found
            lbTitle.Text += _Client.ClientID.ToString();
            txtAccountNo.Text = _Client.AccountNumber;
            txtPinCode.Text = _Client.PinCode;
            txtBalance.Text = _Client.Balance.ToString();
            lblClientID.Text = _Client.ClientID.ToString();

            // here we have to set The Account No to Enabled = false To Not Let Client Update it
            txtAccountNo.Enabled = false;

            ctrlPersonInfoCardWithFilter1.LoadPersonInfo(_Client.PersonID);

            cbAccountTypes.SelectedIndex = _Client.AccountTypeID;

        }

        private void txtBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            switch (textBox.Name)
            {
                case "txtAccountNo":
                    clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrWhiteSpace(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    break;

                case "txtPinCode":
                    clsValidation.ValidateTextBox(textBox, text => clsValidation.IsNumber(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    break;

                case "txtBalance":
                    clsValidation.ValidateTextBox(textBox, text => clsValidation.IsNumber(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);

                    break;


            }
        }


        /// <summary>
        /// Fill All The Error Messages In a List and Show them To The User At Once
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(txtAccountNo.Text.Trim()))
                errors.Add("Account No cannot be blank.");

            if (string.IsNullOrEmpty(txtPinCode.Text.Trim()))
                errors.Add("Pin Code cannot be blank.");

            if (string.IsNullOrEmpty(txtBalance.Text.Trim()))
                errors.Add("Balance cannot be blank.");


            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private async void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!ValidateForm())
                return;

            _Client.PersonID = ctrlPersonInfoCardWithFilter1.PersonID;
            _Client.AccountNumber = txtAccountNo.Text;
            _Client.PinCode = txtPinCode.Text;
            _Client.Balance = Convert.ToDecimal(txtBalance.Text);
            _Client.AddedByUserID = clsGlobal._CurrentUser.ID;

            _AccountTypes = await clsAccountTypes.FindByAccountTypeByNameAsync(cbAccountTypes.Text);
            _Client.AccountTypeID = _AccountTypes.AccountTypeID;

            if (await _Client.SaveAsync())
            {
                MessageBox.Show($"Success, a New Client Was Added With ID {_Client.ClientID}", "Success",
                                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblClientID.Text = _Client.ClientID.ToString();
                lbTitle.Text = $" Update User With ID {_Client.ClientID}";

                _Mode = enMode.Update;
            }
            else
            {
                MessageBox.Show($"Error, Adding New Client Was Canceled", "Error",
                                                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ShowAddEditCLientsForm_Load(object sender, System.EventArgs e)
        {
            await _LoadAccountTypesInComboBox();

            _ResetDefaultValues();

            if (_Mode is enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private async void tabClientsControl_Selecting(object sender, TabControlCancelEventArgs e)
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
                if (await clsClients.ExistsByPersonIDAsync(ctrlPersonInfoCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already  a Client, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void btnNext_Click(object sender, System.EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                ctrlPersonInfoCardWithFilter1.Enabled = false;
                _allowTabSwitch = true; // Allow the tab switch
                tabClientsControl.SelectedTab = tabClientsControl.TabPages[1];
                _allowTabSwitch = false; // Reset after switching
                return;
            }

            // Incase of add new mode.
            if (ctrlPersonInfoCardWithFilter1.PersonID != -1)
            {
                if (await clsClients.ExistsByPersonIDAsync(ctrlPersonInfoCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already a Client, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonInfoCardWithFilter1.FilterFocus();
                }
                else
                {
                    _allowTabSwitch = true; // Allow the tab switch
                    tabClientsControl.SelectedTab = tabClientsControl.TabPages[1];
                    _allowTabSwitch = false; // Reset after switching
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonInfoCardWithFilter1.FilterFocus();
            }
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
    }
}
