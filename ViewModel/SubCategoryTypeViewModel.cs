
using System.ComponentModel.DataAnnotations;


namespace ViewModel
{
    public class AddSubCategoryTypeViewModel
    {
        [Required]
        public Guid categoryId { get; set; }
        public Guid subCategoryId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public Guid createdBy { get; set; }
    }
    public class DeleteSubCategoryTypeViewModel
    {
        [Required]
        public Guid subCategoryTypeId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateSubCategoryTypeViewModel
    {
        [Required]
        public Guid subCategoryTypeId { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public bool active { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
}
