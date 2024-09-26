using System;
using System.Windows.Forms;

namespace ZakaBank_24.User_Forms
{
    public partial class ShowUserInfoCardForm : Form
    {
        private int _UserID;

        public ShowUserInfoCardForm(int userID)
        {
            InitializeComponent();
            _UserID = userID;

        }

        private void ShowUserInfoCardForm_Load(object sender, EventArgs e)
        {
            ctrlUserInfoCard1.LoadUserInfo(_UserID);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
