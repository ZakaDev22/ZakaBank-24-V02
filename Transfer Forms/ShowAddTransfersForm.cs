using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transfer_Forms
{
    public partial class ShowAddTransfersForm : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _CLientID = -1;
        clsTransfers _Transfer;

        public ShowAddTransfersForm()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public ShowAddTransfersForm(int ClientID)
        {
            InitializeComponent();
            _CLientID = ClientID;
            _Mode = enMode.Update;
        }
    }
}
