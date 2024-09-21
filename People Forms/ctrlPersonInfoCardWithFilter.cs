using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ZakaBank_24.People_Forms
{
    public partial class ctrlPersonInfoCardWithFilter : UserControl
    {
        public ctrlPersonInfoCardWithFilter()
        {
            InitializeComponent();
        }

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
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

        public event Action<bool> OnAddNewPerson;
        protected virtual void AddNewPerson(bool IsPersonAdded)
        {
            Action<bool> handler = OnAddNewPerson;
            if (handler != null)
            {
                handler(IsPersonAdded); // Raise the event with the parameter
            }
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        public void PerformSearchClick()
        {
            btnFind.PerformClick();
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

        public void ClearTextBox()
        {
            txtFilterBy.Clear();
        }

        public int PersonID
        {
            get { return ctrlPersonInfoCard1.PersonID; }
        }

        public void LoadPersonInfo(int PersonID)
        {
            txtFilterBy.Text = PersonID.ToString();
            FindNow();
        }

        private async void FindNow()
        {

            await ctrlPersonInfoCard1.LoadPersonInfo(int.Parse(txtFilterBy.Text));

            if (OnPersonSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnPersonSelected(ctrlPersonInfoCard1.PersonID);
        }

        private async void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received

            txtFilterBy.Text = PersonID.ToString();
            await ctrlPersonInfoCard1.LoadPersonInfo(PersonID);

            // rise The Event To Tell The Form That The Data Is Back 
            OnAddNewPerson?.Invoke(true);
        }

        public void FilterFocus()
        {
            txtFilterBy.Focus();
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



        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            ShowAddEditePeopleForm frm = new ShowAddEditePeopleForm();
            frm.DataBack += DataBackEvent; // Subscribe To the Event
            frm.ShowDialog();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text.Trim()))
            {
                ctrlPersonInfoCard1.ResetPersonInfo();

                // Raise the event
                if (OntxtFilterValueEmpty != null)
                {
                    OntxtFilterValueEmpty?.Invoke(true);
                }
            }
        }

        private void txtFilterBy_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterBy, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtFilterBy, null);
            }
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
    }
}
