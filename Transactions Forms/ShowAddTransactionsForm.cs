using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transactions_Forms
{
    public partial class ShowAddTransactionsForm : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _CLientID = -1;
        clsTransactions _Transaction;

        public ShowAddTransactionsForm()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public ShowAddTransactionsForm(int ClientID)
        {
            InitializeComponent();
            _CLientID = ClientID;

            _Mode = enMode.Update;
        }

        private async Task _FillComboBoxWithTransactionTypes()
        {
            var dt = await clsTransactions.GetAllTransactionsTypesAsync();

            foreach (DataRow row in dt.Rows)
            {
                cbTransactionTypes.Items.Add(row["TransactionTypeName"]);
            }
        }

        private async void ShowAddTransactionsForm_Load(object sender, System.EventArgs e)
        {
            await _FillComboBoxWithTransactionTypes();
            cbTransactionTypes.SelectedIndex = 0;
            lbUserID.Text = clsGlobal._CurrentUser.ID.ToString();

            if (_Mode == enMode.AddNew)
            {
                _Transaction = new clsTransactions();
                ctrlClientinfoCardWithFilter1.FilterFocus();
                return;
            }


            ctrlClientinfoCardWithFilter1.LoadClientInfoByID(_CLientID);
            linkClientHistory.Visible = true;

        }

        private void ctrlClientinfoCardWithFilter1_OnClientSelected(int obj)
        {
            if (obj != -1) _CLientID = obj;

            btnSave.Enabled = obj != -1 ? true : false;
            gbTransactionInfo.Enabled = btnSave.Enabled;
        }

        private void ctrlClientinfoCardWithFilter1_OntxtFilterValueEmpty(bool obj)
        {
            _CLientID = -1;
            btnSave.Enabled = false;
            gbTransactionInfo.Enabled = false;
            ctrlClientinfoCardWithFilter1.FilterFocus();
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void txtAmount_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            switch (textBox.Name)
            {
                case "txtDescription":
                    clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrWhiteSpace(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    break;

                case "txtAmount":
                    clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrEmpty(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
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
            if (!ValidateForm())
            {
                return;
            }

            _Transaction.ClientID = _CLientID;
            _Transaction.AddedByUserID = clsGlobal._CurrentUser.ID;
            _Transaction.Amount = Convert.ToInt32(txtAmount.Text.Trim());
            _Transaction.Description = txtDescription.Text.Trim();

            var dt = await clsTransactions.FindByTransactionTypeByNameAsync(cbTransactionTypes.Text.Trim());
            var row = dt.Rows[0];
            _Transaction.TransactionTypeID = Convert.ToInt32(row["TransactionTypeID"]);

            if (await _Transaction.SaveAsync())
            {
                lbTransactionID.Text = _Transaction.TransactionID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lbTitle.Text = "Update Transaction";

                btnSave.Enabled = false;
                txtAmount.Enabled = false;
                txtDescription.Enabled = false;
                cbTransactionTypes.Enabled = false;
                // set The Link Label History To true to call the history form of each client
                linkClientHistory.Visible = true;
                ctrlClientinfoCardWithFilter1.FilterEnabled = false;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void linkClientHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This Future Will Be In the Program Very Sone :-)", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
