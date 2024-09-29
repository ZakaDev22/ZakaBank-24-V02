using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZakaBank_24.Client_Forms;
using ZakaBank_24.Global_Classes;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transfer_Forms
{
    public partial class ShowAddTransfersForm : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _SenderCLientID = -1;
        private int _RecieverCLientID = -1;
        private clsTransfers _Transfer;

        public ShowAddTransfersForm()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public ShowAddTransfersForm(int ClientID)
        {
            InitializeComponent();
            _SenderCLientID = ClientID;
            _Mode = enMode.Update;
        }

        private async void txtBoxes_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            switch (textBox.Name)
            {
                case "txtRecieverClient":

                    if (!string.IsNullOrEmpty(txtRecieverClient.Text))
                    {
                        if (!await clsClients.ExistsByIDAsync(int.Parse(txtRecieverClient.Text.Trim())))
                        {
                            MessageBox.Show($"error, There is No Client with ID {txtRecieverClient.Text}", "Error"
                                             , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            e.Cancel = true;
                            txtRecieverClient.Focus();
                        }

                    }
                    else
                    {
                        clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrWhiteSpace(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    }
                    break;

                case "txtDescription":
                    clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrWhiteSpace(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    break;

                case "txtAmount":
                    clsValidation.ValidateTextBox(textBox, text => clsValidation.IsNumber(text), "Invalid phone number", e, errorProvider1);
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

            if (string.IsNullOrEmpty(txtRecieverClient.Text.Trim()))
                errors.Add("The Receiver ID Cannot be blank.");

            if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                errors.Add("The Amount cannot be blank.");

            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                errors.Add("Description cannot be blank.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private async void btnSave_Click(object sender, System.EventArgs e)
        {

            if (!ValidateForm()) return;

            _Transfer.FromAccountID = _SenderCLientID;
            _Transfer.ToAccountID = Convert.ToInt32(txtRecieverClient.Text);
            _Transfer.Amount = Convert.ToDecimal(txtAmount.Text);
            _Transfer.Description = txtDescription.Text;
            _Transfer.AddedByUserID = clsGlobal._CurrentUser.ID;

            if (await _Transfer.SaveAsync())
            {

                lbTransferID.Text = _Transfer.TransferID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lbTitle.Text = "Update Transfer";

                btnSave.Enabled = false;
                txtAmount.Enabled = false;
                txtRecieverClient.Enabled = false;
                txtDescription.Enabled = false;

                linkClientHistory.Visible = true;
                linkRecieverInfo.Visible = true;
                // set The Link Label History To true to call the history form of each client
                linkClientHistory.Enabled = true;
                ctrlClientinfoCardWithFilter1.FilterEnabled = false;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ctrlClientinfoCardWithFilter1_OnClientSelected(int obj)
        {
            if (obj != -1) _SenderCLientID = obj;

            btnSave.Enabled = obj != -1 ? true : false;
            gbTransferInfo.Enabled = btnSave.Enabled;
        }

        private void ctrlClientinfoCardWithFilter1_OnAddNewCLient(bool obj)
        {
            if (obj)
                ctrlClientinfoCardWithFilter1.PerformClick();
        }

        private void ctrlClientinfoCardWithFilter1_OntxtFilterValueEmpty(bool obj)
        {
            _SenderCLientID = -1;
            _RecieverCLientID = -1;
            btnSave.Enabled = false;
            gbTransferInfo.Enabled = false;
            ctrlClientinfoCardWithFilter1.FilterFocus();
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void ShowAddTransfersForm_Load(object sender, System.EventArgs e)
        {
            lbUserID.Text = clsGlobal._CurrentUser.ID.ToString();

            _Transfer = new clsTransfers();
            if (_Mode == enMode.AddNew)
            {
                ctrlClientinfoCardWithFilter1.FilterFocus();
                return;
            }


            ctrlClientinfoCardWithFilter1.LoadClientInfoByID(_SenderCLientID);
            ctrlClientinfoCardWithFilter1.FilterEnabled = false;
            linkClientHistory.Visible = true;
        }

        private void linkRecieverInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SHowFindClientsForm frm = new SHowFindClientsForm(_Transfer.ToAccountID);
            frm.ShowDialog();
        }

        private void txtRecieverClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void linkClientHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowClientHistoryForm frm = new ShowClientHistoryForm(_Transfer.FromAccountID);
            frm.ShowDialog();
        }
    }
}
