using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class SelfServiceViewModel
    {
      
    }
    public class AddAddressResponseModelViewModel
    {
        public string? WalletAddress { get; set; }
        public string? PrivateKey { get; set; }

    }
    public class UserWalletDetailsMasterViewModel
    {
        public string? Id { get; set; }
        public Guid URID { get; set; }
        public string? WalletAddress { get; set; }
        public string? PrivateKey { get; set; }
        public string? Quantity { get; set; }
        public string? IsActive { get; set; }
        public string? Status { get; set; }
        public string? UsedBy { get; set; }
        public string? LoginId { get; set; }
        public string? UnUsedAddressCount { get; set; }
        public List<UserWalletDetailsMasterViewModel> list { get; set; }
    }

    public class RequestUserWalletDetailsViewModel
    {
        public int Quantity { get; set; }
    }
    public class ResposeUserWalletDetailsViewModel
    {
        public string? UnUsedAddressCount { get; set; }
        

    }

    public class Result2<T>
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }

    }
    //----------------------yaha se add hua 
    public class SelfDepositeAdmin
    {
        public string? AuthLogin { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
    public class RequestModel
    {
        public string? LoginId { get; set; }
    }
    public class RequestWalletAddressModel
    {
        public Guid URID { get; set; }
    }
    public class RequestDepositusdtModel
    {
        //public string? LoginId { get; set; }
        public Guid URID { get; set; }
        //public string? WalletAddress { get; set; }
    }
    public class WalletAddressModel
    {
        public string? WalletAddress { get; set; }
    }
    //-------3
    public class resposeAddFundModel
    {

        public string? Transhas { get; set; }
        public decimal? DepositUSDT { get; set; }
    }
    public class AddFundModel
    {
        public string? WalletAddress { get; set; }
        public string? USDTBalance { get; set; }
        public string? SitoBalance { get; set; }
    }
}
