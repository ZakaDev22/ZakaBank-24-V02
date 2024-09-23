using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Client_Forms
{
    public partial class ctrlClientinfoCardWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnClientSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void ClientSelected(int clientID)
        {
            Action<int> handler = OnClientSelected;
            if (handler != null)
            {
                handler(clientID); // Raise the event with the parameter
            }
        }

        // Define a custom event handler delegate with parameters
        public event Action<bool> OntxtFilterValueEmpty;
        // Create a protected method to raise the event with a parameter
        protected virtual void RiseOntxtFilterValueEmpty(bool IsTxtFilterValueEmpty)
        {
            Action<bool> handler = OntxtFilterValueEmpty;
            if (handler != null)
            {
                handler(IsTxtFilterValueEmpty); // Raise the event with the parameter
            }
        }

        public event Action<bool> OnAddNewCLient;
        protected virtual void AddNewClient(bool IsclientAdded)
        {
            Action<bool> handler = AddNewClient;
            if (handler != null)
            {
                handler(IsclientAdded); // Raise the event with the parameter
            }
        }

        public ctrlClientinfoCardWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        public void FilterFocus()
        {
            txtFilterBy.Focus();
        }

        public void PerformClick()
        {
            btnFind.PerformClick();
        }

        /// <summary>
        /// Get The Client Object And Fill The Label Controls With The data Inside The Client obj
        /// </summary>
        /// <param name="Client"></param>
        private async Task<bool> _FillClientData(clsClients Client)
        {
            if (Client == null)
            {
                MessageBox.Show($"Error, there Is No Client With That ID ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            await ctrlPersonInfoCard1.LoadPersonInfo(Client.PersonID);

            lbClientID.Text = Client.ClientID.ToString();
            lbAccountNo.Text = Client.AccountNumber;
            lbPinCode.Text = Client.PinCode;
            lbBalance.Text = Client.Balance.ToString();

            // check If The User Number 1 how has Null give exception here After
            lbAddedBy.Text = Client.AddedByUserID.ToString();

            lbIsDeleted.Text = Client.IsDeleted == true ? "Yes." : "No.";

            var AccountType = await clsAccountTypes.FindByAccountTypeIDAsync(Client.AccountTypeID);
            lbAccountType.Text = AccountType.Name;

            return true;
        }

        public async void LoadClientInfoByID(int clientID)
        {
            var Client = await clsClients.FindByClientIDAsync(clientID);

            if (await _FillClientData(Client))
            {
                OnClientSelected?.Invoke(Client.ClientID);
            }
        }

        private async void FindNow()
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text))
                return;

            var _client = await clsClients.FindByClientIDAsync(int.Parse(txtFilterBy.Text));

            if (await _FillClientData(_client))
            {
                OnClientSelected?.Invoke(_client.ClientID);
            }
        }

        private async void DataBackEvent(object sender, int clientID)
        {
            // Handle the data received

            txtFilterBy.Text = clientID.ToString();
            var _client = await clsClients.FindByClientIDAsync(clientID);

            if (await _FillClientData(_client))
            {
                // rise The Event To Tell The Form That The Data Is Back 
                OnAddNewCLient?.Invoke(true);
            }


        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            FindNow();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ShowAddEditCLientsForm frm = new ShowAddEditCLientsForm();
            frm.DataBack += DataBackEvent; // Subscribe To the Event
            frm.ShowDialog();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }

            //this will allow only digits if person id is selected
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ResetClientInformation()
        {
            ctrlPersonInfoCard1.ResetPersonInfo();

            lbClientID.Text = "[????]";
            lbAccountNo.Text = "[????]";
            lbPinCode.Text = "[????]";
            lbIsDeleted.Text = "[????]";
            lbAccountType.Text = "[????]";
            lbBalance.Text = "[????]";
            lbAddedBy.Text = "[????]";
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text))
            {
                // Reset The Information
                ResetClientInformation();

                // Rise The Event That The Text Box Is Empty To Set The Save Button To Enabled = false
                OntxtFilterValueEmpty?.Invoke(true);
            }
        }
    }
}
