using System.Data;
using System.Threading.Tasks;
using ZakaBankDataLayer;

namespace ZakaBankLogicLayer
{
    public class clsDashboard
    {
        public static async Task<DataTable> GetTotalTransactionsAsync()
        {
            return await clsDashboardData.GetTotalTransactionsAsync();
        }

        public static async Task<DataTable> GetTotalTransfersAsync()
        {
            return await clsDashboardData.GetTotalTransfersAsync();
        }

        public static async Task<DataTable> GetTransactionTypesAsync()
        {
            return await clsDashboardData.GetTransactionTypesAsync();
        }

        public static async Task<DataTable> GetClientBalanceOverviewAsync()
        {
            return await clsDashboardData.GetClientBalanceOverviewAsync();
        }
    }
}
