using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.People_Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.User_Forms
{
    public partial class ShowMangaeUsersForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;

        public ShowMangaeUsersForm()
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
                    var tuple = await clsUsers.GetPagedUsers(currentPage, pageSize);
                    totalRecords = tuple.TotalCount;
                    dt = tuple.dataTable;

                }
                else
                {
                    dt = await clsUsers.GetAllUsers();
                }

                djvUsers.DataSource = null; // Clear existing data
                djvUsers.DataSource = dt;
                lbRecords.Text = djvUsers.RowCount.ToString();

                FormatDataGridView();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, show message to user)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FormatDataGridView()
        {
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
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0;
            txtFilterValue.Clear();
            if (txtFilterValue.Visible) txtFilterValue.Focus();
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

        private async void btnAddNewUser_Click(object sender, EventArgs e)
        {
            ShowAddEditeUsersForm frm = new ShowAddEditeUsersForm();
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

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 2 || cbFilterBy.SelectedIndex == 4 || cbFilterBy.SelectedIndex == 5)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ShowMangaeUsersForm_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private async void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditeUsersForm frm = new ShowAddEditeUsersForm((int)djvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void personDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var User = await clsUsers.FindByUserIDAsync((int)djvUsers.CurrentRow.Cells[0].Value);

            ShowPersonDetailsForm frm = new ShowPersonDetailsForm(User.PersonID);
            frm.ShowDialog();
        }

        private async void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditeUsersForm frm = new ShowAddEditeUsersForm();
            frm.ShowDialog();

            await _RefreshDataGridViewData();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This User ?", "Confirm", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (await clsUsers.DeleteAsync((int)djvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Success, User Was Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshDataGridViewData();
                }
                else
                {
                    MessageBox.Show("Error, User Was Not Deleted", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("This Operation Was Canceled", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void findUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFindUserForm frm = new ShowFindUserForm();
            frm.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ShowUserInfoCardForm frm = new ShowUserInfoCardForm((int)djvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private async void cmsSetUserToActive_Click(object sender, EventArgs e)
        {
            int UserID = (int)djvUsers.CurrentRow.Cells[0].Value;

            if (MessageBox.Show("Are you sure you want to Set this User to Active Again?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                // if we send true then  the function will set ther user to active and if we send false it will be active
                if (await clsUsers.SetUserAsActiveOrInactive(UserID, true))
                {
                    MessageBox.Show("User was  Set To Active  successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshDataGridViewData();
                }
                else
                    MessageBox.Show("Error, User was not Set To Active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(" set User to Active was Canceled", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private async void cmsSetUserToInActive_Click(object sender, EventArgs e)
        {

            int UserID = (int)djvUsers.CurrentRow.Cells[0].Value;

            if ((UserID == 1) && clsGlobal._CurrentUser.ID != 1)
            {
                MessageBox.Show($"Error,You Can't Set The Admin As InActive 😎💪🙄", "Are You Serious", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to Set this User to InActive Again?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                // if we send true then  the function will set there user to active and if we send false it will be active
                if (await clsUsers.SetUserAsActiveOrInactive(UserID, false))
                {
                    MessageBox.Show("User was  Set To InActive successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshDataGridViewData();
                }
                else
                    MessageBox.Show("Error, User was not Set To InActive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(" set User to InActive was Canceled", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmsChangePassword_Click(object sender, EventArgs e)
        {
            ShowChangePasswordForm frm = new ShowChangePasswordForm((int)djvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private async void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if The DataGrid View has Any Row, if not Then This Code Will Not Implemented
            if (djvUsers.Rows.Count > 0)
            {
                // Set The Enabled Property Of The Context Menu Strip To True
                contextMenuStrip1.Enabled = true;

                int UserID = (int)djvUsers.CurrentRow.Cells[0].Value;

                bool UserActive = await clsUsers.IsUserActive(UserID);

                cmsSetUserToInActive.Enabled = UserActive;

                cmsSetUserToActive.Enabled = UserActive == false;
            }
            else
                contextMenuStrip1.Enabled = false;
        }

        private void btnDeletedClients_Click(object sender, EventArgs e)
        {
            ShowManageDeleteUsersForm frm = new ShowManageDeleteUsersForm();
            frm.Show();
        }
    }
}
