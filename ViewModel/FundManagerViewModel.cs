using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class FundManagerViewModel
    {
        public Guid URID { get; set; }
        public string? PaymentMode { get; set; }
        public decimal Amount { get; set; }
        public string? RefrenceNo { get; set; }
        public string? DepositDetails { get; set; }
        public string? Remark { get; set; }
    }
    public class WithDrawalRequestViewModel
    {
        public Guid URID { get; set; }
    }
    public class RequestUserwithdrawalCoin
    {
        public Guid URID { get; set; }
        public string? SecureCode { get; set; }
        public string? IpAddress { get; set; }
        public Decimal Amount { get; set; }
        public string? Emailid { get; set; }
        public string? WalletAdress { get; set; }
        public int PayMode { get; set; }
        public int walletType { get; set; }
    }
    public class P2PViewModel
    {
        public Guid URID { get; set; }
        public string? AuthLoginReciver { get; set; }
       // public string? fundtye { get; set; }
        public Decimal trnsamount { get; set; }
    }
    public class TransferIncomeToDepositWalletViewModel
    {
        public Guid URID { get; set; }
        public Decimal trnsamount { get; set; }
        public int walletType { get; set; }

    }

    public class TokenDepositsViewModel
    {
        public Guid URID { get; set; }                
        public decimal? trnsamount { get; set; }       
        public string? WalletAddress { get; set; }    
        public string? TransHash { get; set; }
        public string? CreadtedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? KID { get; set; }
        public string? TransType { get; set; }
        public decimal? TokenRate { get; set; }
        public string? TokenType { get; set; }
        public decimal? TokenAmount { get; set; }
        public string? TokenId { get; set; }
    }

    public class AppRejFundViewModel
    {
        public int Id { get; set; }
        public string? AuthLoginId { get; set; }
        public int Rfstatus { get; set; }
        public string? Remark { get; set; }
    }

    public class AppUnAppIncomeVideoModel
    {
        public string? AuthLogin { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class AppUnAppFundRequestModel
    {
        public string? AuthLogin { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

}
