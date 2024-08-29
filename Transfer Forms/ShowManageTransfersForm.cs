using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Transfer_Forms
{
    public partial class ShowManageTransfersForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;
        private DataTable dt;

        public ShowManageTransfersForm()
        {
            InitializeComponent();

            rbByPages.Checked = true;
        }


        private void _RefresgDataGridView()
        {


            if (rbByPages.Checked)
            {
                // Load the paged data
                dt = clsTransfers.GetPagedTransfers(currentPage, pageSize, out totalRecords);
                djvTransfers.DataSource = dt;
                UpdatePaginationButtons();
                lbRecords.Text = djvTransfers.RowCount.ToString();
            }
            else
            {
                // Load all data
                dt = clsTransfers.GetAllTransfers();
                djvTransfers.DataSource = dt;
                lbRecords.Text = djvTransfers.RowCount.ToString();
            }

            if (djvTransfers.RowCount > 0)
            {
                // TransferID, SenderClientID ,ReceiverClientID , Amount , TransferDate , Description, AddedByUserID

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

        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            _RefresgDataGridView();
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


                case "Added By User ID":
                    FilterColumn = "AddedByUser";
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
            if (cbFilterBy.SelectedIndex == 0)
                txtFilterValue.Visible = false;
            else
            {
                txtFilterValue.Clear();
                txtFilterValue.Visible = true;
                txtFilterValue.Focus();
            }
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future Is Not Implemented Yet.", "Inforamtion", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }
    }
}
