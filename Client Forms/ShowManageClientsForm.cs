using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.People_Forms;
using ZakaBank_24.Transactions_Forms;
using ZakaBank_24.Transfer_Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Client_Forms
{
    public partial class ShowManageClientsForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;

        public ShowManageClientsForm()
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
                    var tuple = await clsClients.GetPagedClientsAsync(currentPage, pageSize);
                    totalRecords = tuple.totalCount;
                    dt = tuple.dataTable;

                }
                else
                {
                    dt = await clsClients.GetAllClientsAsync();
                }

                djvClients.DataSource = null; // Clear existing data
                djvClients.DataSource = dt;
                lbRecords.Text = djvClients.RowCount.ToString();

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
            if (djvClients.RowCount > 0)
            {
                djvClients.Columns[0].HeaderText = "Client ID";
                djvClients.Columns[0].Width = 90;

                djvClients.Columns[1].HeaderText = "Person ID";
                djvClients.Columns[1].Width = 130;

                djvClients.Columns[2].HeaderText = "Account Number";
                djvClients.Columns[2].Width = 110;

                djvClients.Columns[3].HeaderText = "Pin Code";
                djvClients.Columns[3].Width = 110;

                djvClients.Columns[4].HeaderText = "Balance";
                djvClients.Columns[4].Width = 130;

                djvClients.Columns[5].HeaderText = "Added By User";
                djvClients.Columns[5].Width = 130;

                djvClients.Columns[6].HeaderText = "Account Type";
                djvClients.Columns[6].Width = 120;

            }
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

        /// <summary>
        /// Updates the visibility of the pagination controls based on whether pagination is enabled.
        /// </summary>
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

        private async void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0; // Hide the filter value when the filter is "None"
            txtFilterValue.Clear();
            if (txtFilterValue.Visible) txtFilterValue.Focus(); // Focus on the filter value when the filter is not "None"
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            // ClientID, PersonID, AccountNumber, PinCode, AccountTypeID

            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;


                case "Client ID":
                    FilterColumn = "ClientID";
                    break;


                case "Account Number":
                    FilterColumn = "AccountNumber";
                    break;

                case "Pin Code":
                    FilterColumn = "PinCode";
                    break;

                case "Account Type":
                    FilterColumn = "AccountTypeName";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvClients.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID" || FilterColumn == "ClientID" || FilterColumn == "PinCode")
                //in this case we deal with integer not string.

                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbRecords.Text = djvClients.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this will allow only digits if person id is selected
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 2 || cbFilterBy.SelectedIndex == 4)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowManageClientsForm_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private async void btnAddNewClient_Click(object sender, EventArgs e)
        {
            ShowAddEditCLientsForm frm = new ShowAddEditCLientsForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditCLientsForm frm = new ShowAddEditCLientsForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditCLientsForm frm = new ShowAddEditCLientsForm((int)djvClients.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Client ?", "Confirm", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (await clsClients.DeleteAsync((int)djvClients.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Success, Client Was Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshDataGridViewData();
                }
                else
                {
                    MessageBox.Show("Error, Client Was Not Deleted", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("This Operation Was Canceled", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void personDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Client = await clsClients.FindByClientIDAsync((int)djvClients.CurrentRow.Cells[0].Value);

            ShowPersonDetailsForm frm = new ShowPersonDetailsForm(Client.PersonID);
            frm.ShowDialog();
        }

        private async void findClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SHowFindClientsForm frm = new SHowFindClientsForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }



        private void btnDeletedClients_Click(object sender, EventArgs e)
        {
            SHowManageDeleteClientsForm frm = new SHowManageDeleteClientsForm();
            frm.Show();
        }

        private void cmsMakeTransaction_Click(object sender, EventArgs e)
        {
            ShowAddTransactionsForm frm = new ShowAddTransactionsForm((int)djvClients.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cmsMakeTransfer_Click(object sender, EventArgs e)
        {
            ShowAddTransfersForm frm = new ShowAddTransfersForm((int)djvClients.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            ShowManagTransactionsForm frm = new ShowManagTransactionsForm();
            frm.Show();
        }

        private void btnTransfers_Click(object sender, EventArgs e)
        {
            ShowManageTransfersForm frm = new ShowManageTransfersForm();
            frm.Show();
        }

        private void cmsClientHistory_Click(object sender, EventArgs e)
        {
            ShowClientHistoryForm frm = new ShowClientHistoryForm((int)djvClients.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
