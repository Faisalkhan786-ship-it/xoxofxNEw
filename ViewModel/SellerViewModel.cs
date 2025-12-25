using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class AddSellerViewModel
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public string? @mobile { get; set; }
        [Required]
        public string? @email { get; set; }
        [Required]
        public string? @streetAddress { get; set; }
        [Required]
        public string? @state { get; set; }
        [Required]
        public string? @city { get; set; }
        [Required]
        public string? @pincode { get; set; }
        [Required]
        public string? @country { get; set; }
        [Required]
        public string? @description { get; set; }
        [Required]
        public Guid createdBy { get; set; }

        [Required]
        public string? @userName { get; set; }
        [Required]
        public string? @userPassword { get; set; }
     
        public string? @otp { get; set; }
    }
    public class DeleteSellerViewModel
    {
        [Required]
        public Guid sellerId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateSellerViewModel
    {
        [Required]
        public Guid sellerId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public string? mobile { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? streetAddress { get; set; }
        [Required]
        public string? state { get; set; }
        [Required]
        public string? city { get; set; }
        [Required]
        public string? pincode { get; set; }
        [Required]
        public string? country { get; set; }
        [Required]
        public string? description { get; set; }
        [Required]
        public bool active { get; set; }
        [Required]
        public Guid updatedBy { get; set; }

        [Required]
        public string? @userPassword { get; set; }
        public string? @otp { get; set; }

    }

    public class CityViewModel
    {
        [Required]
        public int Fk_StateId { get; set; }
        public int Pk_CityId { get; set; }
        public string? CityName { get; set; }
    }
    public class StateViewModel
    {
        [Required]
        public int Pk_StateId { get; set; }
        public int Fk_CountryId { get; set; }
        public string? StateName { get; set; }
    }

    public class CountryViewModel
    {
        [Required]
        public int Country_Id { get; set; }
        [Required]
        public int Country_Code { get; set; }

        [Required]
        public int phonecode { get; set; }
        [Required]
        public string? Country_Name { get; set; }
        public bool active { get; set; }
    }
    public class SellerLoginViewModel
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
}

