using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakaBankLogicLayer;

namespace ZakaBank_24.Account_Types
{
    public partial class SHowManageAccountTypesForm : Form
    {
        private DataTable dt;

        public SHowManageAccountTypesForm()
        {
            InitializeComponent();

        }

        private async Task _RefreshDataGridViewData()
        {

            try
            {

                dt = await clsAccountTypes.GetAllAccountTypesAsync();


                djvAccountTypes.DataSource = null; // Clear existing data
                djvAccountTypes.DataSource = dt;
                lbRecords.Text = djvAccountTypes.RowCount.ToString();

                if (djvAccountTypes.Rows.Count > 0)
                {
                    djvAccountTypes.Columns[0].HeaderText = "Account Type ID";
                    djvAccountTypes.Columns[0].Width = 120;

                    djvAccountTypes.Columns[1].HeaderText = "Name";
                    djvAccountTypes.Columns[1].Width = 120;

                    djvAccountTypes.Columns[2].HeaderText = "Description";
                    djvAccountTypes.Columns[2].Width = 300;
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, show message to user)
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private async void SHowManageAccountTypesForm_Load(object sender, EventArgs e)
        {
            await _RefreshDataGridViewData();
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
