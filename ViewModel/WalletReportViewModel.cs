using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class WalletReportViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }
    public class ROIWalletReportViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }
    public class RentWalletReportViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }
    public class DepositReportViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }

    public class IncomeWithdrawalHistoryViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }

    public class IncomeWithdrawalHistoryViewModel1
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
        public int? type { get; set; }
    }
    public class TransactionIncomeViewModel
    {
        public Guid URID { get; set; }
        public string? transtype { get; set; }
    }


    
    public class AddRechargeTransactionViewModel
    {
        public Guid URID { get; set; }
        public Guid ProductId { get; set; }
        public Guid ByURID { get; set; }
        public decimal Rkprice { get; set; }
        public Guid CreatedBy { get; set; }
        public int LeaseDuration { get; set; }

    }

    public class AddRechargeTransactionAdminViewModel
    {
        public Guid URID { get; set; }
        public int PackageType { get; set; }
        public int USDTValue { get; set; }

    }
  
    public class UpdateRentWalletAdressViewModel
    {
        public string? AuthLoginId { get; set; }
        public decimal debit { get; set; }
        public string? Wallet { get; set; }
        public string? TransHash { get; set; }      
    }
    public class updateSettingsViewModel
    {
        public int? sid { get; set; }
        public decimal limits { get; set; }       
    }

   
    
    public class accStateMent
    {
        public string? AuthLogin { get; set; }
        public string? transtype { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? wtype { get; set; }

    }
    public class AllWalletHistory
    {
        [Required]
        public Guid? URID { get; set; }
        [Required]
        public string? WalletType { get; set; }
        

    }

    public class RechargeTransactionAdminViewModel
    {
        public Guid URID { get; set; }
        public Guid ProductId { get; set; }
        public int? LeaseDuration { get; set; }
        public int? PackageType { get; set; }
    }
    public class DownlineLeftRightCountViewModel
    {
        public Guid URID { get; set; }
        public String? side { get; set; }
        public int? totcount { get; set; }
    }
    public class LeftRightdownlineTeamViewModel
    {
        public Guid Urid { get; set; }
        public String? side { get; set; }
        public int? kid { get; set; }
        public String? fromdate { get; set; }
        public String? toDate { get; set; }
    }
}
