using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.User_Forms
{
    public partial class ShowMangaeUsersForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;

        public ShowMangaeUsersForm()
        {
            InitializeComponent();

            rbByPages.Checked = true;
        }


        private void _RefresgDataGridView()
        {


            if (rbByPages.Checked)
            {
                // Load the paged data
                dt = clsUsers.GetPagedUsers(currentPage, pageSize, out totalRecords);
                djvUsers.DataSource = dt;
                UpdatePaginationButtons();
                lbRecords.Text = djvUsers.RowCount.ToString();
            }
            else
            {
                // Load all data
                dt = clsUsers.GetAllUsers();
                djvUsers.DataSource = dt;
                lbRecords.Text = djvUsers.RowCount.ToString();
            }

            if (djvUsers.RowCount > 0)
            {


                djvUsers.Columns[0].HeaderText = "User ID";
                djvUsers.Columns[0].Width = 90;

                djvUsers.Columns[1].HeaderText = "Person ID";
                djvUsers.Columns[1].Width = 90;

                djvUsers.Columns[2].HeaderText = "User Name";
                djvUsers.Columns[2].Width = 110;

                djvUsers.Columns[3].HeaderText = "Is Active";
                djvUsers.Columns[3].Width = 90;

                djvUsers.Columns[4].HeaderText = "Permissions";
                djvUsers.Columns[4].Width = 100;

                djvUsers.Columns[5].HeaderText = "Added By User";
                djvUsers.Columns[5].Width = 100;

                djvUsers.Columns[6].HeaderText = "Created Date";
                djvUsers.Columns[6].Width = 140;

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


                case "User ID":
                    FilterColumn = "UserID";
                    break;


                case "User Name":
                    FilterColumn = "Username";
                    break;

                case "Permissions":
                    FilterColumn = "Permissions";
                    break;

                case "AddedByUserID":
                    FilterColumn = "AddedByUserID";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvUsers.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID" || FilterColumn == "UserID" || FilterColumn == "Permissions" || FilterColumn == "AddedByUserID")
                //in this case we deal with integer not string.

                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbRecords.Text = djvUsers.Rows.Count.ToString();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            // show add edite user form
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

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 2 || cbFilterBy.SelectedIndex == 4 || cbFilterBy.SelectedIndex == 5)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
