using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Currencies_Forms
{
    public partial class ShowManageCurrenciesForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;

        public ShowManageCurrenciesForm()
        {
            InitializeComponent();


            UpdatePaginationControls();
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
                    var tuple = await clsCurrency.GetPagedCurrenciesAsync(currentPage, pageSize);
                    totalRecords = tuple.totalCount;
                    dt = tuple.dataTable;

                }
                else
                {
                    dt = await clsCurrency.GetAllCurrenciesAsync();
                }

                djvCurrencies.DataSource = null; // Clear existing data
                djvCurrencies.DataSource = dt;
                lbRecords.Text = djvCurrencies.RowCount.ToString();

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
            if (djvCurrencies.RowCount > 0)
            {
                djvCurrencies.Columns[0].HeaderText = "Currency ID";
                djvCurrencies.Columns[0].Width = 90;

                djvCurrencies.Columns[1].HeaderText = "Currency Name ";
                djvCurrencies.Columns[1].Width = 130;

                djvCurrencies.Columns[2].HeaderText = "Currency Code";
                djvCurrencies.Columns[2].Width = 110;

                djvCurrencies.Columns[3].HeaderText = "Exchange Rate";
                djvCurrencies.Columns[3].Width = 120;

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


        private async void rbByAll_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            // ClientID, PersonID, AccountNumber, PinCode, AccountTypeID

            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Currency ID":
                    FilterColumn = "CurrencyID";
                    break;


                case "Currency Name":
                    FilterColumn = "CurrencyName";
                    break;


                case "Currency Code":
                    FilterColumn = "CurrencyCode";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvCurrencies.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "CurrencyID")
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbRecords.Text = djvCurrencies.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0; // Hide the filter value when the filter is "None"
            txtFilterValue.Clear();
            if (txtFilterValue.Visible) txtFilterValue.Focus(); // Focus on the filter value when the filter is not "None"
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

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private async void ShowManageCurrenciesForm_Load(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 1)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
