using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Client_Forms
{
    public partial class ShowManageClientsForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;

        public ShowManageClientsForm()
        {
            InitializeComponent();

            rbByPages.Checked = true;
        }

        private void _RefresgDataGridView()
        {


            if (rbByPages.Checked)
            {
                // Load the paged data
                dt = clsClients.GetPagedClients(currentPage, pageSize, out totalRecords);
                djvClients.DataSource = dt;
                UpdatePaginationButtons();
                lbRecords.Text = djvClients.RowCount.ToString();
            }
            else
            {
                // Load all data
                dt = clsClients.GetAllClients();
                djvClients.DataSource = dt;
                lbRecords.Text = djvClients.RowCount.ToString();
            }

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

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowManageClientsForm_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }
    }
}
