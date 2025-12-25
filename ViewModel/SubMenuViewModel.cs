using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AddSubMenuViewModel
    {
        [Required]
        public Guid menuId { get; set; }
        [Required]
        public string? subMenuName { get; set; }
        [Required]
        public string? subMenuPageName { get; set; }
        [Required]
        public int displayOrder { get; set; }
        [Required]
        public Guid createdBy { get; set; }
    }
    public class DeleteSubMenuViewModel
    {
        [Required]
        public Guid subMenuId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateSubMenuViewModel
    {
        [Required]
        public Guid subMenuId { get; set; }
        [Required]
        public Guid menuId { get; set; }
        [Required]
        public string? subMenuName { get; set; }
        [Required]
        public string? subMenuPageName { get; set; }
        [Required]
        public int displayOrder { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
        public bool active { get; set; }

    }
}
