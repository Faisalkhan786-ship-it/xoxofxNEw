
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ViewModel
{
    public class AddCategoryViewModel
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public IFormFile? image { get; set; }
        [Required]
        public Guid createdBy { get; set; }
    }
    public class DeleteCategoryViewModel
    {
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateCategoryViewModel
    {
        [Required]
        public Guid categoryId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public bool active { get; set; }
        
        public IFormFile? image { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }

    public class AddCloudImages
    {
        public int Id { get; set; }
        public String? ImageName { get; set; }
        [Required]
        public IFormFile? image { get; set; }
        //public bool IsActive { get; set; }
    }
}
