using System;
using System.Data;
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


            rbByPages.Checked = true;
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

        private void rbByAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRight_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {

        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void ShowManageCurrenciesForm_Load(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
        }
    }
}
