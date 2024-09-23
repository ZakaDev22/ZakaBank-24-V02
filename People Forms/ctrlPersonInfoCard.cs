using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBank_24.Global_Classes;
using ZakaBank_24.Properties;
using ZakaBankLogicLayer;

namespace ZakaBank_24.People_Forms
{
    public partial class ctrlPersonInfoCard : UserControl
    {
        public ctrlPersonInfoCard()
        {
            InitializeComponent();
        }


        private clsPeople _Person;

        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public async Task LoadPersonInfo(int PersonID)
        {
            _Person = await clsPeople.FindByPersonIDAsync(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();

                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillPersonInfo();
        }

        private void _LoadPersonImage()
        {

            pbPersonImage.Image = _Person.Gender == 1 ? Resources.Male : Resources.Female;

            string ImagePath = _Person.ImagePath;

            if (!string.IsNullOrEmpty(ImagePath))
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    clsLogExceptionsClass.LogExseptionsToLogerViewr("Could not find the image at " + ImagePath, System.Diagnostics.EventLogEntryType.FailureAudit);


        }

        private async void FillPersonInfo()
        {
            _LoadPersonImage();

            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName();
            lblGendor.Text = _Person.Gender == 1 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = ((DateTime)_Person.DateOfBirth).ToShortDateString();

            clsCountry _Country = _Person.CountryID != null ? await clsCountry.FindByCountryID((int)_Person.CountryID) : null;
            lblCountry.Text = _Country != null ? _Country.CountryName : "[Unknown]";

            lblAddress.Text = _Person.Address;
        }

        public void ResetPersonInfo()
        {

            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            pbGendor.Image = Resources.Male;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Female;
            llEditPersonInfo.Enabled = false;

        }

        private async void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_PersonID == 1 && clsGlobal._CurrentUser.ID != 1)
            {
                MessageBox.Show($"Error,You Can't Edit The Admin User Information 😎💪🙄🤣🤣", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowAddEditePeopleForm frm = new ShowAddEditePeopleForm(_PersonID);
            this.Enabled = false;
            frm.ShowDialog();
            this.Enabled = true;


            await LoadPersonInfo(_PersonID);
        }
    }
}
