
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class AddSubCategoryViewModel
    {
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public Guid createdBy { get; set; }
        [Required]
        public IFormFile? image { get; set; }
    }
    public class DeleteSubCategoryViewModel
    {
        [Required]
        public Guid subCategoryId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateSubCategoryViewModel
    {
        [Required]
        public Guid subCategoryId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public bool active { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
        public IFormFile? image { get; set; }
    }
}
