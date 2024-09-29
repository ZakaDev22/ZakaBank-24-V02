using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZakaBank_24.Account_Types;
using ZakaBank_24.Client_Forms;
using ZakaBank_24.Currencies_Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.Login_Register_Forms;
using ZakaBank_24.People_Forms;
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

            FillTheLoginRegisterInfo(registerID);
        }

        /// <summary>
        /// Fill The Login Register Info
        /// </summary>
        /// <param name="registerID"></param>
        private async void FillTheLoginRegisterInfo(int registerID)
        {
            _loginRegister = await clsLoginRegisters.FindByID(RegiterID);
        }


        private async void _Logout()
        {
            _loginRegister.LogOutDateTime = DateTime.Now;

            if (!await _loginRegister.Save())
                MessageBox.Show("error, something went wrong with login Register Record Update!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _loginForm.Show();
            this.Close();
        }

        private async void _ExitTheApplication()
        {
            _loginRegister.LogOutDateTime = DateTime.Now;

            if (!await _loginRegister.Save())
                MessageBox.Show("error, something went wrong with login Register Record Update!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // exit The Application Without the need of the login screen again
            Application.Exit();
        }


        private async Task _RefreshDashboardInformation()
        {
            var peopleTask = await clsPeople.GetAllPeopleAsync();
            var usersTask = await clsUsers.GetAllUsers();
            var clientsTask = await clsClients.GetAllClientsAsync();
            var transactionsTask = await clsTransactions.GetAllTransactionsAsync();
            var transfersTask = await clsTransfers.GetAllTransfersAsync();
            var registersTask = await clsLoginRegisters.GetAllLoginRegisters();
            var accountTypesTask = await clsAccountTypes.GetAllAccountTypesAsync();


            lbPeople.Text = peopleTask.Rows.Count.ToString();
            lbUsers.Text = usersTask.Rows.Count.ToString();
            lbClients.Text = clientsTask.Rows.Count.ToString();
            lbTransactions.Text = transactionsTask.Rows.Count.ToString();
            lbTransfers.Text = transfersTask.Rows.Count.ToString();
            lbRegisters.Text = registersTask.Rows.Count.ToString();
            lbAccountType.Text = accountTypesTask.Rows.Count.ToString();

            _CreatNewSeriesForColumnChart();
            _CreateNewBarChartSeries();
        }


        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            _ExitTheApplication();
        }

        private void btnPeople_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnPeople.Top;

            ShowManagePeopleForm frm = new ShowManagePeopleForm();

            frm.Show();


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

        private async void btnDashboard_Click(object sender, System.EventArgs e)
        {
            pnClickedButton.Top = btnDashboard.Top;
            await _RefreshDashboardInformation();
        }

        /// <summary>
        /// This Is The Main Form Load Event Where we Will Start Loading the Dashboard Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_Load(object sender, System.EventArgs e)
        {
            lbUserName.Text = clsGlobal._CurrentUser.UserName;
            await _RefreshDashboardInformation();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Logout();
        }

        private async void btnReports_Click(object sender, EventArgs e)
        {
            toglrQueckSearch.Checked = false;

            await _LoadChartDataAsync();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonDetailsForm frm = new ShowPersonDetailsForm(clsGlobal._CurrentUser.PersonID);
            frm.ShowDialog();
        }

        private void userInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserInfoCardForm frm = new ShowUserInfoCardForm(clsGlobal._CurrentUser.ID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowChangePasswordForm frm = new ShowChangePasswordForm(clsGlobal._CurrentUser.ID);
            frm.ShowDialog();
        }

        private void FindUsertoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // Add A constructor in find user to take the current user id and load it her
            ShowFindUserForm frm = new ShowFindUserForm();
            frm.ShowDialog();
        }

        private void curToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ShowManageCurrenciesForm frm = new ShowManageCurrenciesForm();
            frm.Show();
        }

        private void ATMtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future Will Come very Soon :-)", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toglrQueckSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (toglrQueckSearch.Checked)
            {
                splitContainer1.SplitterDistance = 561; // this if the user Want to see The Charts Data If He click Reports Button

                chartTotalTransactionsOverTime.Visible = false;
                chartClientBalanceOverview.Visible = false;
                chartTotalTransfers.Visible = false;
                chartTransactionTypesDistribution.Visible = false;


                lbTotalTransactionsForEachDay.Visible = false;
                lbTotalTransfersByEachDay.Visible = false;
                lbTotalTransactionTypes.Visible = false;
                lbBalancesRanges.Visible = false;
            }
            else
            {
                splitContainer1.SplitterDistance = 25; // this If He Click In Quick Search And Want To Find The Short Cuts

                chartTotalTransactionsOverTime.Visible = true;
                chartClientBalanceOverview.Visible = true;
                chartTotalTransfers.Visible = true;
                chartTransactionTypesDistribution.Visible = true;

                lbTotalTransactionsForEachDay.Visible = true;
                lbTotalTransfersByEachDay.Visible = true;
                lbTotalTransactionTypes.Visible = true;
                lbBalancesRanges.Visible = true;
            }
        }

        private void btnDeletedUsers_Click(object sender, EventArgs e)
        {
            ShowManageDeleteUsersForm frm = new ShowManageDeleteUsersForm();
            frm.Show();
        }

        private void btnDeletedClients_Click(object sender, EventArgs e)
        {
            SHowManageDeleteClientsForm frm = new SHowManageDeleteClientsForm();
            frm.Show();
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            ShowFindPersonForm frm = new ShowFindPersonForm();
            frm.ShowDialog();
        }

        private void btnFindClient_Click(object sender, EventArgs e)
        {
            SHowFindClientsForm frm = new SHowFindClientsForm();
            frm.ShowDialog();
        }

        private void btnFindUser_Click(object sender, EventArgs e)
        {
            ShowFindUserForm frm = new ShowFindUserForm();
            frm.ShowDialog();
        }

        // Dashboard Charts Methods 

        /// <summary>
        /// Load The Reports Charts with Data
        /// </summary>
        /// <returns></returns>
        private async Task _LoadChartDataAsync()
        {
            try
            {
                // Load total transactions
                var totalTransactionsData = await clsDashboard.GetTotalTransactionsAsync();
                PopulateTotalTransactionsChart(totalTransactionsData);

                // Load total transfers
                var totalTransfersData = await clsDashboard.GetTotalTransfersAsync();
                PopulateTotalTransfersChart(totalTransfersData);

                // Load transaction types
                var transactionTypesData = await clsDashboard.GetTransactionTypesAsync();
                PopulateTransactionTypesChart(transactionTypesData);

                // Load client balance overview
                var clientBalanceData = await clsDashboard.GetClientBalanceOverviewAsync();
                PopulateClientBalanceChart(clientBalanceData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading chart data: " + ex.Message);

                // log the exception in the log viewer to track it latter
                clsLogExceptionsClass.LogExseptionsToLogerViewr(ex.Message, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
        }



        private void _CreatNewSeriesForColumnChart()
        {
            // Ensure the series exists before adding data
            if (chartTotalTransactionsOverTime.Series.Count == 0)
            {
                Series Transactionseries = new Series("Transaction For Each Day")
                {
                    ChartType = SeriesChartType.Column,
                    XValueType = ChartValueType.DateTime // Ensure X-values are treated as DateTime
                };
                chartTotalTransactionsOverTime.Series.Add(Transactionseries);
            }

            // Configure the X-axis to show DateTime correctly
            chartTotalTransactionsOverTime.Series[0].XValueType = ChartValueType.DateTime;
            chartTotalTransactionsOverTime.Series[0].YValueType = ChartValueType.Int32;
            chartTotalTransactionsOverTime.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chartTotalTransactionsOverTime.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MM";

            // Set font for X-axis labels
            chartTotalTransactionsOverTime.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Change to desired font, size, and style

            // Increase border width and set other visual properties
            chartTotalTransactionsOverTime.Series[0].BorderWidth = 3;  // Increase for visibility
            chartTotalTransactionsOverTime.Series[0].IsValueShownAsLabel = true; // Show values on bars
            chartTotalTransactionsOverTime.Series[0].BorderDashStyle = ChartDashStyle.Solid; // Border style

            // Adjust the space between the columns
            chartTotalTransactionsOverTime.Series[0]["PointWidth"] = "0.5";  // 0.5 creates more space between columns
            chartTotalTransactionsOverTime.Series[0]["PixelPointWidth"] = "25"; // Adjust width of columns for better spacing

            // Ensure that the X-Axis fits all columns with better spacing
            chartTotalTransactionsOverTime.ChartAreas[0].AxisX.IsStartedFromZero = false; // Prevent starting from zero to fit dates

            // Set the margins to give more space to the columns
            chartTotalTransactionsOverTime.ChartAreas[0].InnerPlotPosition = new ElementPosition(5, 5, 90, 75); // Adjust the plot area
            chartTotalTransactionsOverTime.ChartAreas[0].Position = new ElementPosition(10, 10, 85, 80); // Ensure full chart is visible
            chartTotalTransactionsOverTime.ChartAreas[0].AxisX.MajorGrid.Enabled = false; // Disable grid lines to avoid clutter
        }


        private void PopulateTotalTransactionsChart(DataTable data)
        {
            // Clear previous data points
            chartTotalTransactionsOverTime.Series[0].Points.Clear();


            // Loop through the data and add points to the existing series
            foreach (DataRow row in data.Rows)
            {
                DateTime transactionDate = Convert.ToDateTime(row["TransactionDate"]);
                int totalTransactions = Convert.ToInt32(row["TotalTransactions"]);

                // Add the data point to the series, ensuring the X value is DateTime
                chartTotalTransactionsOverTime.Series[0].Points.AddXY(transactionDate, totalTransactions);
            }


        }

        private void _CreateNewBarChartSeries()
        {
            // Check if the series already exists
            if (chartTotalTransfers.Series.Count == 0)
            {
                Series barSeries = new Series("Transaction Bar Chart")
                {
                    ChartType = SeriesChartType.Bar,
                    XValueType = ChartValueType.DateTime // Ensure X values are treated as DateTime
                };
                chartTotalTransfers.Series.Add(barSeries);
            }

            // Configure the X-axis to show DateTime correctly
            chartTotalTransfers.Series[0].XValueType = ChartValueType.DateTime;
            chartTotalTransfers.Series[0].YValueType = ChartValueType.Int32;
            chartTotalTransfers.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chartTotalTransfers.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MM"; // Date format

            // Set font for X-axis labels
            chartTotalTransfers.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 9, FontStyle.Bold);


            // Increase border width and set other visual properties
            chartTotalTransfers.Series[0].BorderWidth = 3;  // Increase for visibility
            chartTotalTransfers.Series[0].IsValueShownAsLabel = true; // Show values on bars
            chartTotalTransfers.Series[0].BorderDashStyle = ChartDashStyle.Solid; // Border style

            // Adjust the space between the bars
            chartTotalTransfers.Series[0]["PointWidth"] = "0.3"; // 0.5 creates more space between bars
            chartTotalTransfers.Series[0]["PixelPointWidth"] = "15"; // Adjust width of bars for better spacing

            // Optional: Adjust the plot area for better visibility
            chartTotalTransfers.ChartAreas[0].InnerPlotPosition = new ElementPosition(10, 10, 80, 80); // Adjust the plot area
            chartTotalTransfers.ChartAreas[0].Position = new ElementPosition(10, 10, 80, 80); // Ensure full chart is visible
        }
        private void PopulateTotalTransfersChart(DataTable data)
        {
            chartTotalTransfers.Series[0].Points.Clear();

            foreach (DataRow row in data.Rows)
            {
                DateTime TransferDate = Convert.ToDateTime(row["TransferDate"]);
                int TotalTransfers = Convert.ToInt32(row["TotalTransfers"]);


                chartTotalTransfers.Series[0].Points.AddXY(TransferDate, TotalTransfers);
            }
        }

        private void PopulateTransactionTypesChart(DataTable data)
        {

            chartTransactionTypesDistribution.Series[0].Points.Clear();

            foreach (DataRow row in data.Rows)
            {
                int transactionTypeID = Convert.ToInt32(row["TransactionTypeID"]);
                string transactionTypeName = row["TransactionTypeName"].ToString();
                int transactionCount = Convert.ToInt32(row["TransactionCount"]);


                chartTransactionTypesDistribution.Series[0].Points.AddXY(transactionTypeName, transactionCount);
            }


        }

        private void PopulateClientBalanceChart(DataTable data)
        {
            chartClientBalanceOverview.Series[0].Points.Clear();

            foreach (DataRow row in data.Rows)
            {
                string BalanceRange = Convert.ToString(row["BalanceRange"]);
                int ClientCount = Convert.ToInt32(row["ClientCount"]);


                chartClientBalanceOverview.Series[0].Points.AddXY(BalanceRange, ClientCount);
            }
        }
    }
}
