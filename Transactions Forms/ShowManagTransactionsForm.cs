using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transactions_Forms
{
    public partial class ShowManagTransactionsForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;

        public ShowManagTransactionsForm()
        {
            InitializeComponent();

            rbByPages.Checked = true;
        }

        private void _RefresgDataGridView()
        {


            if (rbByPages.Checked)
            {
                // Load the paged data
                dt = clsTransactions.GetPagedTransactions(currentPage, pageSize, out totalRecords);
                djvTransactions.DataSource = dt;
                UpdatePaginationButtons();
                lbRecords.Text = djvTransactions.RowCount.ToString();
            }
            else
            {
                // Load all data
                dt = clsTransactions.GetAllTransactions();
                djvTransactions.DataSource = dt;
                lbRecords.Text = djvTransactions.RowCount.ToString();
            }

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
                djvTransactions.Columns[3].Width = 100;

                djvTransactions.Columns[4].HeaderText = "Description";
                djvTransactions.Columns[4].Width = 100;

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

        private void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            if (rbByPages.Checked)
            {
                lbSize.Visible = true;
                cbPageSize.Visible = true;
                btnLeft.Visible = true;
                btnRight.Visible = true;
                btnPageNumber.Visible = true;
            }
            else
            {
                lbSize.Visible = false;
                cbPageSize.Visible = false;
                btnLeft.Visible = false;
                btnRight.Visible = false;
                btnPageNumber.Visible = false;
            }

            cbFilterBy.SelectedIndex = 0;
            _RefresgDataGridView();
        }



        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            _RefresgDataGridView();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
        }

        private void btnAddNewTransaction_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future Is Not Implemented Yet.", "Inforamtion", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                _RefresgDataGridView();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                _RefresgDataGridView();
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


                case "Transaction Type ID":
                    FilterColumn = "TransactionTypeID";
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

            dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);

            lbRecords.Text = djvTransactions.RowCount.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
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
