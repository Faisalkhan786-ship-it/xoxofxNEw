using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AppLoginViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }

    public class SendOtpWithdrawalViewModel
    {
        [Required]
        public string? EmailId { get; set; }

        public string? WalletAddress { get; set; }
    }
    public class SendOtpFundRequestViewModel
    {
        [Required]
        public string? EmailId { get; set; }       
    }


    public class SendOtpViewModel
    {
        [Required]
        public string? EmailId { get; set; }

    }

    public class ValidateOtpViewModel
    {
        [Required]
        public Guid URID { get; set; }
        public string otp { get; set; }
    }

    public class AddAppUserViewModel
    {
        public Guid IntroURID { get; set; }
        public string? Password { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }

        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public int? CountryId { get; set; }
        public string? Address { get; set; }
        //public string? OTPregpage { get; set; }
    }

    public class SendOtpRequestViewModel
    {
        public string? Otp { get; set; }
        public string? UserFullName { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }

    public class ForgotPasswordResult
    {
        public string? AuthLogin { get; set; }
        public string? AuthPass { get; set; }
        public string? Email { get; set; }
    }

    public class TokenDetailsViewModel
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public string? username { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
    }
    public class AdminTokenDetailsViewModel
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public string? username { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
    }
    public class LoggingEvents
    {
        public const int getByIdItem = 1000;
        public const int getAllItem = 1001;
        public const int addItem = 1002;
        public const int updateItem = 1003;
        public const int deleteItem = 1004;
        public const int listItems = 1005;
        public const int testItem = 3000;
        public const int getItemNotFound = 4000;
        public const int updateItemNotFound = 4001;
    }
    public class ResponseViewModel1
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public object chartData { get; set; }
        public object analyticsData { get; set; }
    }
    public class ResponseViewModellogin
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }

        public string? AuthLogin { get; set; }
        public string? AuthPassword { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ResponseViewModel
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ResponseViewModelSendEventEmail
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
        public Guid EventbookingID { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ResponseViewModelProduct
    {
        public int statusCode { get; set; }
        public string? message { get; set; }

        public Guid productId { get; set; }  
        public object? data { get; set; }

    }
    public class ApiLogs
    {
        public string? CreatedOn { get; set; }
        public string? RequestMethod { get; set; }
        public string? RequestUrl { get; set; }
        public string? RequestHeaders { get; set; }
        public int UserId { get; set; }
        public string? ClientIpAddress { get; set; }
        public string? PageUrl { get; set; }
        public string? RequestSource { get; set; }
        public int OrgId { get; set; }
        public string? RequestBody { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionStackTrace { get; set; }
        public int ResponseStatusCode { get; set; }
        public string? ResponseHeaders { get; set; }
        public string? ResponseBody { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? OldPassword { get; set; }
        [Required]
        public string? NewPass { get; set; }
    }
    public class ForgotSellerPasswordViewModel
    {
        [Required]
        public string? LoginId { get; set; }

    }

    public class UpdateUserProfileViewModel
    {
        [Required]
        public string LoginID { get; set; }
        public string? FName { get; set; }
  
        public string? LName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }
        public int? countryid { get; set; }
        public string? WalletBep20 { get; set; }

    }

    public class UpdateUserImageViewModel
    {
        [Required]
        public string LoginID { get; set; }

        [Required]
        public List<IFormFile>? ProfileImage { get; set; }
    }

}
