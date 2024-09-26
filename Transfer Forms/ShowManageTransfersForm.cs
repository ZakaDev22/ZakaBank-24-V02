using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transfer_Forms
{
    public partial class ShowManageTransfersForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private DataTable dt;

        public ShowManageTransfersForm()
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
                    var tuple = await clsTransfers.GetPagedTransfersAsync(currentPage, pageSize);
                    totalRecords = tuple.totalCount;
                    dt = tuple.dtaTable;

                }
                else
                {
                    dt = await clsTransfers.GetAllTransfersAsync();
                }

                djvTransfers.DataSource = null; // Clear existing data
                djvTransfers.DataSource = dt;
                lbRecords.Text = djvTransfers.RowCount.ToString();

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
            if (djvTransfers.RowCount > 0)
            {

                djvTransfers.Columns[0].HeaderText = "Transfer ID";
                djvTransfers.Columns[0].Width = 90;

                djvTransfers.Columns[1].HeaderText = "Sender Client ID";
                djvTransfers.Columns[1].Width = 110;

                djvTransfers.Columns[2].HeaderText = "Receiver Client ID";
                djvTransfers.Columns[2].Width = 110;

                djvTransfers.Columns[3].HeaderText = "Amount";
                djvTransfers.Columns[3].Width = 90;

                djvTransfers.Columns[4].HeaderText = "Transfer Date";
                djvTransfers.Columns[4].Width = 140;

                djvTransfers.Columns[5].HeaderText = "Description";
                djvTransfers.Columns[5].Width = 100;

                djvTransfers.Columns[6].HeaderText = "Added By User ID";
                djvTransfers.Columns[6].Width = 90;

            }

            // Set the text of the page number button to the current page number
            btnPageNumber.Text = currentPage.ToString();
        }

        private async void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
            UpdatePaginationControls();

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

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            // TransferID, SenderClientID ,ReceiverClientID , Amount , TransferDate , Description, AddedByUserID

            switch (cbFilterBy.Text)
            {
                case "Transfer ID":
                    FilterColumn = "TransferID";
                    break;


                case "Sender Client ID":
                    FilterColumn = "SenderClientID";
                    break;


                case "Receiver Client ID":
                    FilterColumn = "ReceiverClientID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvTransfers.Rows.Count.ToString();
                return;
            }

            dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);

            lbRecords.Text = djvTransfers.RowCount.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0; // Hide the filter value when the filter is "None"
            txtFilterValue.Clear();
            if (txtFilterValue.Visible) txtFilterValue.Focus(); // Focus on the filter value when the filter is not "None"
        }

        private async void btnAddNewUser_Click(object sender, EventArgs e)
        {
            ShowAddTransfersForm frm = new ShowAddTransfersForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }
    }
}
