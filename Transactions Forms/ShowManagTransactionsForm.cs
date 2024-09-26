using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transactions_Forms
{
    public partial class ShowManagTransactionsForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;

        public ShowManagTransactionsForm()
        {
            InitializeComponent();

            cbPageSize.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
        }

        private async Task _RefreshDataGridViewData()
        {

            try
            {
                if (rbByPages.Checked)
                {
                    // Load the paged data
                    var tuple = await clsTransactions.GetPagedTransactionsAsync(currentPage, pageSize);
                    totalRecords = tuple.totalCount;
                    dt = tuple.dataTable;

                }
                else
                {
                    dt = await clsTransactions.GetAllTransactionsAsync();
                }

                djvTransactions.DataSource = null; // Clear existing data
                djvTransactions.DataSource = dt;
                lbRecords.Text = djvTransactions.RowCount.ToString();

                FormatDataGridView();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, show message to user)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// This Method formats the data grid view.
        /// </summary>
        private void FormatDataGridView()
        {
            if (djvTransactions.RowCount > 0)
            {

                // TransactionID , ClientID, Amount , TransactionTypeID , Description , TransactionDate , AddedByUser
                djvTransactions.Columns[0].HeaderText = "Transaction ID";
                djvTransactions.Columns[0].Width = 90;

                djvTransactions.Columns[1].HeaderText = "Client ID";
                djvTransactions.Columns[1].Width = 90;

                djvTransactions.Columns[2].HeaderText = "Amount";
                djvTransactions.Columns[2].Width = 110;

                djvTransactions.Columns[3].HeaderText = "Transaction Type";
                djvTransactions.Columns[3].Width = 110;

                djvTransactions.Columns[4].HeaderText = "Description";
                djvTransactions.Columns[4].Width = 120;

                djvTransactions.Columns[5].HeaderText = "Transaction Date";
                djvTransactions.Columns[5].Width = 120;

                djvTransactions.Columns[6].HeaderText = "Added By User";
                djvTransactions.Columns[6].Width = 90;

            }

            // Set the text of the page number button to the current page number
            btnPageNumber.Text = currentPage.ToString();
        }


        /// <summary>
        /// Updates the enabled state and background color of the pagination buttons based on the current page and total records.
        /// </summary>
        private void UpdatePaginationButtons()
        {
            // Enable the left button if the current page is greater than 1
            btnLeft.Enabled = currentPage > 1;
            // Enable the right button if the current page times the page size is less than the total records
            btnRight.Enabled = currentPage * pageSize < totalRecords;
            // Set the background color of the left button to GreenYellow if it is enabled, otherwise set it to Red
            btnLeft.BackColor = btnLeft.Enabled ? Color.GreenYellow : Color.Red;
            // Set the background color of the right button to GreenYellow if it is enabled, otherwise set it to Red
            btnRight.BackColor = btnRight.Enabled ? Color.GreenYellow : Color.Red;
        }

        private async void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();

            UpdatePaginationControls(); // Update pagination controls based on whether pagination is enabled
        }

        private void UpdatePaginationControls()
        {
            // Show or hide pagination controls based on whether pagination is enabled
            if (rbByPages.Checked)
            {
                lbSize.Visible = true;
                cbPageSize.Visible = true;
                btnLeft.Visible = true;
                btnRight.Visible = true;
                btnPageNumber.Visible = true;
                UpdatePaginationButtons();
            }
            else
            {
                lbSize.Visible = false;
                cbPageSize.Visible = false;
                btnLeft.Visible = false;
                btnRight.Visible = false;
                btnPageNumber.Visible = false;
            }
        }


        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0;
            txtFilterValue.Clear();
            if (txtFilterValue.Visible) txtFilterValue.Focus();
        }

        private async void btnAddNewTransaction_Click(object sender, EventArgs e)
        {
            ShowAddTransactionsForm frm = new ShowAddTransactionsForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void btnRight_Click(object sender, EventArgs e)
        {
            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                await _RefreshDataGridViewData();
                UpdatePaginationControls();
            }
        }

        private async void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await _RefreshDataGridViewData();
                UpdatePaginationControls();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Transaction ID":
                    FilterColumn = "TransactionID";
                    break;


                case "Client ID":
                    FilterColumn = "ClientID";
                    break;


                case "Transaction Type":
                    FilterColumn = "TransactionTypeName";
                    break;


                case "Added By User":
                    FilterColumn = "AddedByUser";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvTransactions.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "TransactionID" || FilterColumn == "ClientID" || FilterColumn == "AddedByUser")
                //in this case we deal with integer not string.

                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbRecords.Text = djvTransactions.RowCount.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex != 3) // Transaction Type
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowManagTransactionsForm_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }
    }
}
