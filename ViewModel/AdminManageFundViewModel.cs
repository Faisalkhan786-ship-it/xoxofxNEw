using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AdminManageFundViewModel
    {
        public int Wallettype { get; set; }
        public int CrDr { get; set; }
        public Guid URID { get; set; }
        public decimal Amt { get; set; }
        public string? Remark { get; set; }
    }
    public class AllWalletHistoryViewModel
    {
        public Guid URID { get; set; }
        public string WalletType { get; set; }
      
    }
}
