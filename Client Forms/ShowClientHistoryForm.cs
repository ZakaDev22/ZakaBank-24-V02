using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Client_Forms
{
    public partial class ShowClientHistoryForm : Form
    {
        private DataTable TransactionDataTable;
        private DataTable TransfersDataTable;
        private int _ClientID;

        public ShowClientHistoryForm(int clientID)
        {
            InitializeComponent();
            _ClientID = clientID;
        }

        private async Task _RefreshTransactionDataGridViewData()
        {

            TransactionDataTable = await clsClients.GetClientTransactionsHistoryByIDAsync(_ClientID);

            djvClientsTransactionHistory.DataSource = null; // Clear existing data
            djvClientsTransactionHistory.DataSource = TransactionDataTable;
            lbRecords.Text = djvClientsTransactionHistory.RowCount.ToString();

            FormatTransactionDataGridView();

        }

        private async Task _RefreshTransfersDataGridViewData()
        {

            TransfersDataTable = await clsClients.GetClientTransfersHistoryByIDAsync(_ClientID);

            djvClientTransferHistory.DataSource = null; // Clear existing data
            djvClientTransferHistory.DataSource = TransfersDataTable;
            lbTransferRecords.Text = djvClientTransferHistory.RowCount.ToString();

            FormatTransferDataGridView();

        }


        /// <summary>
        /// This Method formats the data grid view.
        /// </summary>
        private void FormatTransactionDataGridView()
        {
            if (djvClientsTransactionHistory.RowCount > 0)
            {

                // TransactionID , ClientID, Amount , TransactionTypeID , Description , TransactionDate , AddedByUser
                djvClientsTransactionHistory.Columns[0].HeaderText = "Transaction ID";
                djvClientsTransactionHistory.Columns[0].Width = 90;

                djvClientsTransactionHistory.Columns[1].HeaderText = "Client ID";
                djvClientsTransactionHistory.Columns[1].Width = 90;

                djvClientsTransactionHistory.Columns[2].HeaderText = "Amount";
                djvClientsTransactionHistory.Columns[2].Width = 110;

                djvClientsTransactionHistory.Columns[3].HeaderText = "Transaction Type";
                djvClientsTransactionHistory.Columns[3].Width = 110;

                djvClientsTransactionHistory.Columns[4].HeaderText = "Description";
                djvClientsTransactionHistory.Columns[4].Width = 120;

                djvClientsTransactionHistory.Columns[5].HeaderText = "Transaction Date";
                djvClientsTransactionHistory.Columns[5].Width = 120;

                djvClientsTransactionHistory.Columns[6].HeaderText = "Added By User";
                djvClientsTransactionHistory.Columns[6].Width = 90;

            }

        }

        /// <summary>
        /// This Method formats the data grid view.
        /// </summary>
        private void FormatTransferDataGridView()
        {
            if (djvClientTransferHistory.RowCount > 0)
            {

                djvClientTransferHistory.Columns[0].HeaderText = "Transfer ID";
                djvClientTransferHistory.Columns[0].Width = 90;

                djvClientTransferHistory.Columns[1].HeaderText = "Sender Client ID";
                djvClientTransferHistory.Columns[1].Width = 110;

                djvClientTransferHistory.Columns[2].HeaderText = "Receiver Client ID";
                djvClientTransferHistory.Columns[2].Width = 110;

                djvClientTransferHistory.Columns[3].HeaderText = "Amount";
                djvClientTransferHistory.Columns[3].Width = 90;

                djvClientTransferHistory.Columns[4].HeaderText = "Transfer Date";
                djvClientTransferHistory.Columns[4].Width = 140;

                djvClientTransferHistory.Columns[5].HeaderText = "Description";
                djvClientTransferHistory.Columns[5].Width = 100;

                djvClientTransferHistory.Columns[6].HeaderText = "Added By User ID";
                djvClientTransferHistory.Columns[6].Width = 90;

            }

        }

        private async void ShowClientHistoryForm_Load(object sender, System.EventArgs e)
        {
            //clsClients Client = await clsClients.FindByClientIDAsync(_ClientID);

            //if (Client == null)
            //{
            //    MessageBox.Show($"Error, The Client With ID {_ClientID} Was Not Fount, This Form Will Close", "Error",
            //                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //}


            // i cancel The Check If Client Exist Because We Will Call This Form Only From Other Forms Will The Client ID IS 100% Exist
            ctrlClientinfoCardWithFilter1.LoadClientInfoByID(_ClientID);
            ctrlClientinfoCardWithFilter1.FilterEnabled = false;
            await _RefreshTransactionDataGridViewData();
            await _RefreshTransfersDataGridViewData();
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
