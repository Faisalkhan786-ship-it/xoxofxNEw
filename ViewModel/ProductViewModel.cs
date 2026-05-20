
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class AddProductViewModel
    {
        [Required]
        public Guid categoryId { get; set; }

        public Guid createdBy { get; set; }

        public string? productName { get; set; }
        public string? tittle { get; set; }
        public string? type { get; set; }
        public decimal? rOI { get; set; }
        public decimal? minInvest { get; set; }
        public decimal? winRate { get; set; }
        public decimal? Traders { get; set; }
        public bool? active { get; set; }

    }
    public class UpdateProductViewModel
    {
        [Required]
        public Guid productId { get; set; }

        [Required]
        public Guid categoryId { get; set; }

        public string productname { get; set; }

        public string? title { get; set; }
        public string? type { get; set; }

        public decimal? rating { get; set; }

        public decimal? price { get; set; }

        public decimal? totalReturn { get; set; }

        public decimal? noOfRating { get; set; }

        public bool active { get; set; }

        public Guid updatedBy { get; set; }
    }
    public class DeleteProductViewModel
    {
        [Required]
        public Guid productId { get; set; }    
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
}
