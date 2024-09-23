using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.Properties;
using ZakaBankLogicLayer;

namespace ZakaBank_24.People_Forms
{
    public partial class ShowAddEditePeopleForm : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum EnMode { AddNew = 0, Update = 1, };

        private EnMode _Mode;

        private int _PersonID = -1;

        clsPeople _Person;
        clsCountry _Countries;

        public ShowAddEditePeopleForm()
        {
            InitializeComponent();
            _Mode = EnMode.AddNew;
        }

        public ShowAddEditePeopleForm(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            _Mode = EnMode.Update;
        }

        /// <summary>
        /// This Method will Fill The Combo Box With All The Countries Names
        /// </summary>
        private async Task FillCountriesInComboBox()
        {
            DataTable CountryTable = await clsCountry.GetAllCountriesAsync();

            foreach (DataRow Row in CountryTable.Rows)
            {
                cbCountries.Items.Add(Row["CountryName"]);
            }
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pcPersonImage.Image = rbMale.Checked == true ? Resources.Male : Resources.Female;
        }

        private async void ShowAddEditePeopleForm_Load(object sender, EventArgs e)
        {
            await FillCountriesInComboBox();
            rbMale.Checked = true;

            DateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            DateTimePicker1.MinDate = DateTime.Now.AddYears(-100);

            cbCountries.SelectedIndex = cbCountries.FindString("Morocco");

            linkChoseImage.Visible = (pcPersonImage.ImageLocation == null);
            linkRemoveImage.Visible = !linkChoseImage.Visible;


            if (_Mode == EnMode.AddNew)
            {
                lbTitle.Text = "Add New Person :-)";
                _Person = new clsPeople();
                return;
            }

            // the bag is here for update 
            _Person = await clsPeople.FindByPersonIDAsync(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show($"this Form Will Be Closed Because  Contact With {_Person.PersonID} Is Not Found :-( ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            FillPersonInformation(_Person);
        }

        /// <summary>
        /// This Method Will Fill The Person Information Into The Edite Form
        /// </summary>
        /// <param name="Person"></param>
        private async void FillPersonInformation(clsPeople Person)
        {
            lbTitle.Text = $"Edit Person With ID {_Person.PersonID} :-)";
            lbPersonID.Text = _PersonID.ToString();

            txtFirstName.Text = _Person.FirstName;

            txtLastName.Text = _Person.LastName;
            DateTimePicker1.Value = (DateTime)_Person.DateOfBirth;
            txtPhoneNo.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;

            _Countries = await clsCountry.FindByCountryID((int)_Person.CountryID);
            cbCountries.SelectedIndex = cbCountries.FindString(_Countries.CountryName);

            rbMale.Checked = _Person.Gender == 1 ? true : rbFemale.Checked = true;

            if (_Person.ImagePath != "")
                pcPersonImage.Load(_Person.ImagePath);

            linkRemoveImage.Visible = (_Person.ImagePath != "");
        }


        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Error! Some Fields Are Not Valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Countries = await clsCountry.FindByCountryName(cbCountries.Text);

            _Person.FirstName = txtFirstName.Text;
            _Person.LastName = txtLastName.Text;

            _Person.Email = txtEmail.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = DateTimePicker1.Value;
            _Person.Phone = txtPhoneNo.Text;
            _Person.CountryID = _Countries.CountryID;

            _Person.Gender = rbMale.Checked == true ? (short)1 : (short)0;

            _Person.ImagePath = pcPersonImage.ImageLocation != null ? pcPersonImage.ImageLocation : "";


            if (await _Person.SaveAsync())
            {
                MessageBox.Show("Data Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error! : Data Is Not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Mode = EnMode.Update;

            lbTitle.Text = $"Edit Person With ID {_Person.PersonID}";
            lbPersonID.Text = _Person.PersonID.ToString();
        }


        /// <summary>
        /// We Use The Guna TextBox if You Want To use Normal Text Box In the Future make Sure To Update This Method :-)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

            switch (textBox.Name)
            {
                case "txtFirstName":
                case "txtLastName":
                case "txtAddress":
                    clsValidation.ValidateTextBox(textBox, text => !string.IsNullOrWhiteSpace(text), $"{textBox.Name.Replace("txt", "")} cannot be empty", e, errorProvider1);
                    break;

                case "txtEmail":
                    clsValidation.ValidateTextBox(textBox, text => clsValidation.IsValidEmail(text), "Invalid email format", e, errorProvider1);
                    break;

                case "txtPhoneNo":
                    clsValidation.ValidateTextBox(textBox, text => clsValidation.IsValidPhoneNumber(text), "Invalid phone number", e, errorProvider1);
                    break;
            }
        }

        private void linkChoseImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pcPersonImage.Load(selectedFilePath);
                linkRemoveImage.Visible = true;
                // ...
            }
        }

        private void linkRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pcPersonImage.ImageLocation = null;

            pcPersonImage.Image = rbMale.Checked == true ? Resources.Male : Resources.Female;

            linkRemoveImage.Visible = false;
        }
    }
}
