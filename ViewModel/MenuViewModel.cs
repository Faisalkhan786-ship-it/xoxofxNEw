
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ViewModel
{
    public class AddMenuViewModel
    {
        [Required]
        public string? menuName { get; set; }
        [Required]
        public int displayOrder { get; set; }
        [Required]
        public Guid createdBy { get; set; }

        [Required]
        public string? menuIcon { get; set; }
        [Required]
        public string? pageName { get; set; }
    }

    //public class AddMenuWithSubMenu
    //{
    //    [Required]
    //    public Guid menuId { get; set; }

    //    [Required]
    //    public string? menuName { get; set; }

    //    public int displayOrder { get; set; }


    //    [Required]
    //    public Guid createdBy { get; set; }
    //    [Required]
    //    public string? menuIcon { get; set; }
    //    [Required]
    //    public string? pageName { get; set; }

    //    [Required]
    //    public Guid subMenuId { get; set; }
    //    [Required]
    //    public string? subMenuName { get; set; }

    //    [Required]
    //    public string? subMenuPageName { get; set; }
    //    [Required]
    //    public int displayOrderSubMenu { get; set; }
    //    [Required]
    //    public Guid appRoleId { get; set; }
    //    [Required]

    //    public Boolean ActiveSubmenu { get; set; }
    //    public Boolean Activemenu { get; set; }

    //}

    //public class AddMenuWithSubMenu
    //{
    //    [Required]
    //    public Guid menuId { get; set; }

    //    [Required]
    //    public string? menuName { get; set; }

    //    public int displayOrder { get; set; }

    //    [Required]
    //    public Guid createdBy { get; set; }

    //    [Required]
    //    public string? menuIcon { get; set; }

    //    [Required]
    //    public string? pageName { get; set; }

    //    [Required]
    //    public Guid subMenuId { get; set; }

    //    [Required]
    //    public string? subMenuName { get; set; }

    //    [Required]
    //    public string? subMenuPageName { get; set; }

    //    [Required]
    //    public int displayOrderSubMenu { get; set; }

    //    [Required]
    //    public Guid appRoleId { get; set; }

    //    [Required]
    //    public bool ActiveSubmenu { get; set; }

    //    [Required]
    //    public bool Activemenu { get; set; }
    //}
    //public class AddMenuWithSubMenu
    //{
    //    public Guid appRoleId { get; set; }
    //    public Guid menuId { get; set; }
    //    public string menuName { get; set; }
    //    public string pageName { get; set; }
    //    public int displayOrder { get; set; }
    //    public Guid createdBy { get; set; }
    //    public string menuIcon { get; set; }
    //    public bool Activemenu { get; set; }
    //    public List<SubMenuModel> subMenuList { get; set; }
    //}

    //public class SubMenuModel
    //{
    //    public Guid subMenuId { get; set; }
    //    public string subMenuName { get; set; }
    //    public string subMenuPageName { get; set; }
    //    public int displayOrderSubMenu { get; set; }
    //    public bool ActiveSubmenu { get; set; }
    //}

    public class AddMenuWithSubMenu
    {
        public Guid appRoleId { get; set; }
        public Guid menuId { get; set; }
        public string menuName { get; set; }
        public string pageName { get; set; }
        public int displayOrder { get; set; }
        public Guid createdBy { get; set; }
        public string menuIcon { get; set; }

        [JsonPropertyName("activemenu")]
        public bool Activemenu { get; set; }

        public List<AddSubMenu> subMenuList { get; set; }
    }

    public class AddSubMenu
    {
        public Guid subMenuId { get; set; }
        public string subMenuName { get; set; }
        public string subMenuPageName { get; set; }
        public int displayOrderSubMenu { get; set; }

        [JsonPropertyName("activeSubmenu")]
        public bool ActiveSubmenu { get; set; }
    }
    public class DeleteMenuViewModel
    {
        [Required]
        public Guid menuId { get; set; }
        [Required]
        public Guid updatedBy { get; set; }
    }
    public class UpdateMenuViewModel
    {
        [Required]
        public Guid menuId { get; set; }
        [Required]
        public string? menuName { get; set; }
        [Required]
        public string? pageName { get; set; }

        [Required]
        public int displayOrder { get; set; }
        [Required]
        public Guid updatedBy { get; set; }

        [Required]
        public string? menuIcon { get; set; }
        public bool active { get; set; }
    }

    public class MenuByUserRoleViewModel
    {
        [Required]
        public string? userName { get; set; }
    }
}
