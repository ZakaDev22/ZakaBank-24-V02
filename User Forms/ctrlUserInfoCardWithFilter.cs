using System;
using System.Windows.Forms;

namespace ZakaBank_24.User_Forms
{
    public partial class ctrlUserInfoCardWithFilter : UserControl
    {
        public ctrlUserInfoCardWithFilter()
        {
            InitializeComponent();
        }

        private void ResetInformations()
        {
            txtFilterBy.Clear();
            ctrlUserInfoCard1._ResetUserInfo();
            txtFilterBy.Focus();
        }


        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetInformations();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnFind.PerformClick();

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (cbFilterBy.SelectedIndex)
            {
                // User ID
                case 0:
                    ctrlUserInfoCard1.LoadUserInfo(int.Parse(txtFilterBy.Text.Trim()));
                    break;

                // Person ID
                case 1:
                    ctrlUserInfoCard1.LoadUserInfoByPersonID(int.Parse(txtFilterBy.Text.Trim()));
                    break;

            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtFilterBy.Text))
            {
                ResetInformations();
            }
        }

        private void txtFilterBy_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterBy, "This field is required.");
            }
            else
            {
                errorProvider1.SetError(txtFilterBy, null);
            }
        }
    }
}
