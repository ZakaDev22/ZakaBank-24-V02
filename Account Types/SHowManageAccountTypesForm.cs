using System.Data;
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

            _RefreshDataGridView();
        }

        private void _RefreshDataGridView()
        {
            dt = clsAccountTypes.GetAllAccountTypes();
            djvAccountTypes.DataSource = dt;

            if (djvAccountTypes.Rows.Count > 0)
            {
                djvAccountTypes.Columns[0].HeaderText = "Account Type ID";
                djvAccountTypes.Columns[0].Width = 120;

                djvAccountTypes.Columns[1].HeaderText = "Name";
                djvAccountTypes.Columns[1].Width = 120;

                djvAccountTypes.Columns[2].HeaderText = "Description";
                djvAccountTypes.Columns[2].Width = 300;
            }

            lbRecords.Text = djvAccountTypes.Rows.Count.ToString();
        }

        private void btnCLose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
