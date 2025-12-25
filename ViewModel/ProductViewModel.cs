
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class AddProductViewModel
    {
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public Guid subCategoryId { get; set; }

        //public Guid? subCategoryTypeId { get; set; }
        [Required]
        public Guid sellerId { get; set; }

   
        //public Guid sizeId { get; set; }
        [Required]
        public string? title { get; set; }
        [Required]
        public string? subTitle { get; set; }
        [Required]
        public decimal rating { get; set; }
        [Required]
        public int noOfRating { get; set; }

        [Required]
        public decimal price { get; set; }

        [Required]
        public string? description { get; set; }
        [Required]
        public Guid createdBy { get; set; }

        public bool isAiAgent { get; set; }
        public bool isRobotics { get; set; }
        public bool isTrendingProjects { get; set; }



        public int? PerHour { get; set; }      
        public decimal? Unit { get; set; }      
        public string? Specification { get; set; }      
        public string? task { get; set; }      
        public string? TOATALMONTH { get; set; }      
        public string? NFTurL { get; set; }      
        public decimal? totalReturn { get; set; }      
        public decimal? weeklyReturn { get; set; }      
        public int? AICredite { get; set; }
        public int? month { get; set; }
        public string? TokenId { get; set; }

    }
    public class UpdateProductViewModel
    {
        [Required]
        public Guid productId { get; set; }
        public Guid categoryId { get; set; }
        public Guid subCategoryId { get; set; }
        //public Guid? subCategoryTypeId { get; set; }
        public Guid sellerId { get; set; }

        [Required]
        public string? title { get; set; }
        [Required]
        public string? subTitle { get; set; }
        [Required]
        public decimal rating { get; set; }
        [Required]
        public int noOfRating { get; set; }

        [Required]
        public decimal price { get; set; }

        [Required]
        public string? description { get; set; }
        [Required]
        public bool active { get; set; }
        [Required]
        public Guid updatedBy { get; set; }  
        public bool isAiAgent { get; set; }
        public bool isRobotics { get; set; }
        public bool isTrendingProjects { get; set; }


        public string? NFTurL { get; set; }
        public string? TOATALMONTH { get; set; }
        public int? PerHour { get; set; }
        public decimal? Unit { get; set; }
        public decimal? totalReturn { get; set; }
        public decimal? weeklyReturn { get; set; }
        public int? month { get; set; }
        public string? Specification { get; set; }
        public string? task { get; set; }
        public string? TokenId { get; set; }
        public int? AICredite { get; set; }
    }
    public class DeleteProductViewModel
    {
        [Required]
        public Guid productId { get; set; }
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public Guid subCategoryId { get; set; }
        [Required]
        public Guid subCategoryTypeId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }

    }
    public class AddProductImageViewModel
    {
        [Required]
        public Guid productId { get; set; }
        public string? title { get; set; }
        [Required]
        public List<IFormFile>? image { get; set; }
        [Required]
        public Guid createdBy { get; set; }
    }
    public class UpdateProductImageViewModel
    {
        [Required]
        public Guid productImageId { get; set; }
        [Required]
        public Guid productId { get; set; }
        public string? title { get; set; }
        [Required]
        public List<IFormFile>? image { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class DeleteProductImageViewModel
    {
        [Required]
        public Guid productImageId { get; set; }
        [Required]
        public Guid productId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }

    
    

    public class SearchCommonDataViewModel
    {     
        public string? categoryName { get; set; }
        public string? subCategoryName { get; set; }
        public string? subCategoryTypeName { get; set; }
        public string? stepsName { get; set; }
        public string? typeOfProductName { get; set; }
        public string? sizename { get; set; }
        public string? concernname { get; set; }
        public string? ingredientName { get; set; }
        public string? productname { get; set; }
    }

    public class getAllProductByIdViewModel
    {     
        public string? id { get; set; }
    }

    public class DeletePinCodeShippingViewModel
    {
        [Required]
        public Guid pinCodeShippingId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    
    public class UpdateMetaTagViewModel
    {
        [Required]
        public Guid productId { get; set; }
        public string? metaTitle { get; set; }
        public string? metaDescription { get; set; }
        public string? metaKeyword { get; set; }
    }   

    //public class AgentViewModel
    //{
    //    public Guid ProductId { get; set; }
    //}
}
