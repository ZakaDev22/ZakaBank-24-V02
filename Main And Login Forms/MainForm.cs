using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Account_Types;
using ZakaBank_24.Client_Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.Login_Register_Forms;
using ZakaBank_24.Transactions_Forms;
using ZakaBank_24.Transfer_Forms;
using ZakaBank_24.User_Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Main_And_Login_Forms
{
    public partial class MainForm : Form
    {
        clsLoginRegisters _loginRegister;
        private LoginForm _loginForm;
        private int RegiterID;

        public MainForm(LoginForm loginform, int registerID)
        {
            InitializeComponent();
            this._loginForm = loginform;
            RegiterID = registerID;

            _loginRegister = clsLoginRegisters.FindByID(RegiterID);
        }

        private void _Logout()
        {
            _loginRegister.LogOutDateTime = DateTime.Now;

            if (!_loginRegister.Save())
                MessageBox.Show("error, something went wrong with login Register Record Update!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _loginForm.Show();
            this.Close();
        }


        private async void _RefreshDashboardInformation()
        {
            var peopleTask = Task.Run(() => clsPeople.GetAllPeople().Rows.Count);
            var usersTask = Task.Run(() => clsUsers.GetAllUsers().Rows.Count);
            var clientsTask = Task.Run(() => clsClients.GetAllClients().Rows.Count);
            var transactionsTask = Task.Run(() => clsTransactions.GetAllTransactions().Rows.Count);
            var transfersTask = Task.Run(() => clsTransfers.GetAllTransfers().Rows.Count);
            var registersTask = Task.Run(() => clsLoginRegisters.GetAll().Rows.Count);
            var accountTypesTask = Task.Run(() => clsAccountTypes.GetAllAccountTypes().Rows.Count);

            await Task.WhenAll(peopleTask, usersTask, clientsTask, transactionsTask, transfersTask, registersTask, accountTypesTask);

            lbPeople.Text = peopleTask.Result.ToString();
            lbUsers.Text = usersTask.Result.ToString();
            lbClients.Text = clientsTask.Result.ToString();
            lbTransactions.Text = transactionsTask.Result.ToString();
            lbTransfers.Text = transfersTask.Result.ToString();
            lbRegisters.Text = registersTask.Result.ToString();
            lbAccountType.Text = accountTypesTask.Result.ToString();
        }


        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            _Logout();
        }

        private void btnPeople_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnPeople.Top;

            ShowManagePeopleForm frm = new ShowManagePeopleForm();

            frm.ShowDialog();


            pnClickedButton.Top = btnDashboard.Top;
        }

        private void btnClients_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnClients.Top;
            ShowManageClientsForm frm = new ShowManageClientsForm();
            frm.Show();
            pnClickedButton.Top = btnDashboard.Top;
        }

        private void btnUsers_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnUsers.Top;
            ShowMangaeUsersForm frm = new ShowMangaeUsersForm();
            frm.Show();
            pnClickedButton.Top = btnDashboard.Top;
        }

        private void btnTransactions_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnTransactions.Top;
            ShowManagTransactionsForm frm = new ShowManagTransactionsForm();
            frm.Show();
            pnClickedButton.Top = btnDashboard.Top;
        }

        private void btnTransfers_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnTransfers.Top;
            ShowManageTransfersForm frm = new ShowManageTransfersForm();

            frm.Show();

            pnClickedButton.Top = btnDashboard.Top;
        }

        //private async Task _ShowLoginRegisterFormAndWaitAsync()
        //{
        //    pnClickedButton.Top = btnLoginRegisters.Top;

        //    var tcs = new TaskCompletionSource<bool>();
        //    var frm = new ShowManageLoginRegisterForm();

        //    // Handle form closed event
        //    frm.FormClosed += (s, e) => tcs.SetResult(true);

        //    // Show form modally
        //    frm.ShowDialog();

        //    // Await until the form is closed
        //    await tcs.Task;

        //    // After the form is closed, execute the remaining code
        //    pnClickedButton.Top = btnDashboard.Top;
        //}

        private void btnLoginRegisters_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnLoginRegisters.Top;

            var frm = new ShowManageLoginRegisterForm();
            frm.Show();

            pnClickedButton.Top = btnDashboard.Top;
        }


        private void btnLogout_Click(object sender, System.EventArgs e)
        {
            _Logout();
        }

        private void btnAccountTypes_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnAccountTypes.Top;
            SHowManageAccountTypesForm frm = new SHowManageAccountTypesForm();
            frm.Show();

            pnClickedButton.Top = btnDashboard.Top;
        }

        private void btnDashboard_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnDashboard.Top;
            _RefreshDashboardInformation();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            lbUserName.Text = clsGlobal._CurrentUser.UserName;
            _RefreshDashboardInformation();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Logout();
        }
    }
}
