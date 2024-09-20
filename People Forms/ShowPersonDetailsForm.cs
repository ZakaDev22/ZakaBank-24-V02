using System;
using System.Windows.Forms;

namespace ZakaBank_24.People_Forms
{
    public partial class ShowPersonDetailsForm : Form
    {
        private int _personID;
        public ShowPersonDetailsForm(int personID)
        {
            InitializeComponent();
            _personID = personID;
        }

        private async void ShowPersonDetailsForm_Load(object sender, EventArgs e)
        {
            await ctrlPersonInfoCard1.LoadPersonInfo(_personID);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
