﻿using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.People_Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24
{
    public partial class ShowManagePeopleForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;

        public ShowManagePeopleForm()
        {
            InitializeComponent();

            UpdatePaginationControls();
            cbPageSize.SelectedIndex = 0;
            cbFilterBy.SelectedIndex = 0;
        }

        private async Task _RefreshAllPeople()
        {

            try
            {
                if (rbByPages.Checked)
                {
                    var result = await clsPeople.GetPagedPeopleAsync(currentPage, pageSize);
                    totalRecords = result.TotalCount;
                    dt = result.dataTable;

                }
                else
                {
                    dt = await clsPeople.GetAllPeopleAsync();
                }

                djvPeople.DataSource = null; // Clear existing data
                djvPeople.DataSource = dt;
                lbRecords.Text = djvPeople.RowCount.ToString();

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
            if (djvPeople.RowCount > 0)
            {
                djvPeople.Columns[0].HeaderText = "Person ID";
                djvPeople.Columns[0].Width = 90;

                djvPeople.Columns[1].HeaderText = "Full Name";
                djvPeople.Columns[1].Width = 130;

                djvPeople.Columns[2].HeaderText = "Country Name";
                djvPeople.Columns[2].Width = 110;

                djvPeople.Columns[3].HeaderText = "Gender";
                djvPeople.Columns[3].Width = 110;

                djvPeople.Columns[4].HeaderText = "Address";
                djvPeople.Columns[4].Width = 130;

                djvPeople.Columns[5].HeaderText = "Date Of Birth";
                djvPeople.Columns[5].Width = 130;

                djvPeople.Columns[6].HeaderText = "Phone Number";
                djvPeople.Columns[6].Width = 120;

                djvPeople.Columns[7].HeaderText = "Email";
                djvPeople.Columns[7].Width = 155;
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

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

            await _RefreshAllPeople();

            UpdatePaginationControls();

        }

        private async void btnRight_Click(object sender, EventArgs e)
        {

            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                await _RefreshAllPeople();
                UpdatePaginationControls();
            }
        }

        private async void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await _RefreshAllPeople();
                UpdatePaginationControls();
            }
        }

        private async void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            await _RefreshAllPeople();
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

            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;


                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "Gendor";
                    break;

                case "Phone":
                    FilterColumn = "PhoneNumber";
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
                lbRecords.Text = djvPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbRecords.Text = djvPeople.Rows.Count.ToString();
        }


        private void ShowManagePeopleForm_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private async void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            // Show Add Edite Person Form
            ShowAddEditePeopleForm frm = new ShowAddEditePeopleForm();
            frm.ShowDialog();
            await _RefreshAllPeople();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this will allow only digits if person id is selected
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 5)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private async void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditePeopleForm frm = new ShowAddEditePeopleForm((int)djvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            await _RefreshAllPeople();
        }

        private void personDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonDetailsForm frm = new ShowPersonDetailsForm((int)djvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private async void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddEditePeopleForm frm = new ShowAddEditePeopleForm();
            frm.ShowDialog();

            await _RefreshAllPeople();
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Person ?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (await clsPeople.DeletePersonAsync((int)djvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Success, Person Was Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await _RefreshAllPeople();
                }
                else
                {
                    MessageBox.Show("Error, Person Was Not Deleted", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("This Operation Was Canceled", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void findPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFindPersonForm frm = new ShowFindPersonForm();
            frm.ShowDialog();
        }
    }
}
