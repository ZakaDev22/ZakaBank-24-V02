using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.User_Forms
{
    public partial class ctrlUserInfoCard : UserControl
    {

        public ctrlUserInfoCard()
        {
            InitializeComponent();
        }

        private clsUsers _User;
        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }



        public async void LoadUserInfo(int UserID)
        {
            _User = await clsUsers.FindByUserIDAsync(UserID);
            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await _FillUserInfo();
        }

        public async void LoadUserInfoByPersonID(int PersonID)
        {
            _User = await clsUsers.FindUserByPersonIDAsync(PersonID);
            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await _FillUserInfo();
        }

        private async Task _FillUserInfo()
        {

            await ctrlPersonInfoCard1.LoadPersonInfo(_User.PersonID);
            lbUserID.Text = _User.ID.ToString();
            lbUserName.Text = _User.UserName.ToString();

            lbIsActive.Text = _User.IsActive == true ? "Yes" : "No";
            lbIsDeleted.Text = _User.IsDeleted == true ? "Yes" : "No";

        }

        public void _ResetUserInfo()
        {

            ctrlPersonInfoCard1.ResetPersonInfo();

            lbUserID.Text = "[???]";
            lbUserName.Text = "[???]";
            lbIsActive.Text = "[???]";
            lbIsDeleted.Text = "[???]";
        }
    }
}
