using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Login_Register_Forms
{
    public partial class ShowManageLoginRegisterForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private DataTable dt;


        public ShowManageLoginRegisterForm()
        {
            InitializeComponent();

            cbPageSize.SelectedIndex = 1;
            cbFilterBy.SelectedIndex = 0;
        }


        private async Task _RefreshDataGridViewData()
        {

            try
            {
                if (rbByPages.Checked)
                {
                    // Load the paged data
                    var tuple = await clsLoginRegisters.GetPagedLoginRegisters(currentPage, pageSize);
                    totalRecords = tuple.TotalCount;
                    dt = tuple.dataTable;

                }
                else
                {
                    dt = await clsLoginRegisters.GetAllLoginRegisters();
                }

                djvLoginRegisters.DataSource = null; // Clear existing data
                djvLoginRegisters.DataSource = dt;
                lbRecords.Text = djvLoginRegisters.RowCount.ToString();

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
            if (djvLoginRegisters.RowCount > 0)
            {
                // LoginRegisterID , UserID , Users.Username , LoginDateTime , LogoutDateTime

                djvLoginRegisters.Columns[0].HeaderText = "Login Register ID";
                djvLoginRegisters.Columns[0].Width = 110;

                djvLoginRegisters.Columns[1].HeaderText = "User ID";
                djvLoginRegisters.Columns[1].Width = 110;

                djvLoginRegisters.Columns[2].HeaderText = "User Name";
                djvLoginRegisters.Columns[2].Width = 120;

                djvLoginRegisters.Columns[3].HeaderText = "Login Date";
                djvLoginRegisters.Columns[3].Width = 140;

                djvLoginRegisters.Columns[4].HeaderText = "Logout Date";
                djvLoginRegisters.Columns[4].Width = 140;


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
            }
            else
            {
                lbSize.Visible = false;
                cbPageSize.Visible = false;
                btnLeft.Visible = false;
                btnRight.Visible = false;
                btnPageNumber.Visible = false;
            }
            UpdatePaginationButtons();
        }

        private async void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = int.Parse(cbPageSize.Text);
            await _RefreshDataGridViewData();
            UpdatePaginationControls();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //  LoginRegisterID , LoginRegisters.UserID , Users.Username , LoginDateTime , LogoutDateTime

            // fix the bug her in the login register ID Filter
            switch (cbFilterBy.Text.Trim())
            {
                case "Login Register ID":
                    FilterColumn = "LoginRegisterID";
                    break;


                case "User ID":
                    FilterColumn = "UserID";
                    break;


                case "User Name":
                    FilterColumn = "Username";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lbRecords.Text = djvLoginRegisters.Rows.Count.ToString();
                return;
            }

            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 2)
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text);

            else
                dt.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterColumn, txtFilterValue.Text);

            lbRecords.Text = djvLoginRegisters.RowCount.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.SelectedIndex != 0;
            txtFilterValue.Clear();

            if (txtFilterValue.Visible) txtFilterValue.Focus();

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
            // make the right select indexes After 
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 2)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
