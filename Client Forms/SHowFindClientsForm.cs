using System.Windows.Forms;

namespace ZakaBank_24.Client_Forms
{
    public partial class SHowFindClientsForm : Form
    {
        public SHowFindClientsForm()
        {
            InitializeComponent();
        }

        public SHowFindClientsForm(int clientID)
        {
            InitializeComponent();

            ctrlClientinfoCardWithFilter1.LoadClientInfoByID(clientID);
            ctrlClientinfoCardWithFilter1.FilterEnabled = false;
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
