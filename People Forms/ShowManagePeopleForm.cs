using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24
{
    public partial class ShowManagePeopleForm : Form
    {
        private DataTable dt;
        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;

        public ShowManagePeopleForm()
        {
            InitializeComponent();

            rbByPages.Checked = true;
            _RefresgAllPeople();
        }

        private void _RefresgAllPeople()
        {
            dt = clsPeople.GetAllPeople();
            djvPeople.DataSource = dt;

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

            if (rbByPages.Checked)
            {
                // Load the paged data
                dt = clsPeople.GetPagedPeople(currentPage, pageSize, out totalRecords);
                djvPeople.DataSource = dt;
                UpdatePaginationButtons();
                lbRecords.Text = djvPeople.RowCount.ToString();
            }
            else
            {
                // Load all data
                dt = clsPeople.GetAllPeople();
                djvPeople.DataSource = dt;
                lbRecords.Text = djvPeople.RowCount.ToString();
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

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbByPages_CheckedChanged(object sender, EventArgs e)
        {
            if (rbByPages.Checked)
            {
                cbPageSize.SelectedIndex = 0;
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
            _RefresgAllPeople();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {

            if (currentPage * pageSize < totalRecords)
            {
                currentPage++;
                _RefresgAllPeople();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                _RefresgAllPeople();
            }
        }

        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(cbPageSize.Text);
            _RefresgAllPeople();
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


            if (FilterColumn == "PersonID" || FilterColumn == "PhoneNumber")
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

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            // Show Add Edite Person Form
            // AddEditePersonForm addEditePersonForm = new AddEditePersonForm();
            // addEditePersonForm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //this will allow only digits if person id is selected
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 5)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
