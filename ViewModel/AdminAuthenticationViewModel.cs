
using System.ComponentModel.DataAnnotations;
namespace ViewModel
{
    public class AdminUserLoginViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
    //public class AdminTokenDetailsViewModel
    //{
    //    [Required]
    //    public string? name { get; set; }
    //    [Required]
    //    public string? username { get; set; }
    //    [Required]
    //    public string? email { get; set; }
    //    [Required]
    //    public string? phoneNumber { get; set; }
    //}
    public class AddAdminUserViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? fname { get; set; }

        [Required]
        public string? lname { get; set; }


        [Required]
        public string? type { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
    }

    public class AdminUserGuidViewModel
    {
        public Guid adminUserId { get; set; }
        public string username { get; set; }
    }

    public class AdminSendOtpViewModel
    {
        [Required]
        public string? username { get; set; }
    }
    public class AdminVerifyOtpViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? otp { get; set; }
    }
    public class AdminSendOtpRequestViewModel
    {
        public string? Otp { get; set; }
        public string? UserFullName { get; set; }
    }
    public class AdminForgotPasswordViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }

    public class BulkRegsitrationViewModel
    {
        [Required]
        public Guid? IntroURID { get; set; }
  
        public string? IntroSide { get; set; }
        [Required]
        public string? FName { get; set; }
        [Required]
        public string? LName { get; set; }
        [Required]
        public string? Mobile { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public int? NoOfId { get; set; }
        [Required]
        public string? CountryId { get; set; }
    }

    public class UpdateAdminProfileViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? firstName { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
        [Required]
        public string? email { get; set; }        
    }
}
