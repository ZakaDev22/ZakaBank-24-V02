using System;
using System.Windows.Forms;

namespace ZakaBank_24.People_Forms
{
    public partial class ShowFindPersonForm : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public ShowFindPersonForm()
        {
            InitializeComponent();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {

            // Trigger the event to send data back to the caller form.
            DataBack?.Invoke(this, ctrlPersonInfoCardWithFilter1.PersonID);

            this.Close();
        }
    }
}
