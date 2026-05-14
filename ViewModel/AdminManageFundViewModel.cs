using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UnAppIncomeViewModel
    {
        public string? AuthLogin { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
    public class UpdateIncometWalletAdressViewModel
    {
        public string? AuthLoginId { get; set; }
        public decimal debit { get; set; }
        public string? Wallet { get; set; }
        public string? TransHash { get; set; }
    }

    public class AppRejFundViewModel
    {
        public int Id { get; set; }
        public string? AuthLoginId { get; set; }
        public int Rfstatus { get; set; }
        public string? Remark { get; set; }
    }
    public class AdminManageFundViewModel
    {
        public int Wallettype { get; set; }
        public int CrDr { get; set; }
        public Guid URID { get; set; }
        public decimal Amt { get; set; }
        public string? Remark { get; set; }
    }


    //public class AllWalletHistoryViewModel
    //{
    //    public string? Authlogin { get; set; }
    //    public string WalletType { get; set; }

    //}
}
