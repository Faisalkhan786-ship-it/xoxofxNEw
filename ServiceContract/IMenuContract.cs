using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface IMenuContract
    {
        public Task<ResponseViewModel> getByIdMenu(Guid id);
        public Task<ResponseViewModel> getAllMenu(Guid adminUserId);
        public Task<ResponseViewModel> addMenu(AddMenuViewModel addMenuViewModel);
        public Task<ResponseViewModel> updateMenu(UpdateMenuViewModel updateMenuViewModel);
        public Task<ResponseViewModel> deleteMenu(DeleteMenuViewModel deleteMenuViewModel);
        public Task<ResponseViewModel> getMenuByUserRole(string userName);
        public Task<ResponseViewModel> getAllMenuOfSubMenu(Guid menuId);

        public Task<ResponseViewModel> menuAndSubMenuPermisiom(Guid appRoleId);
        //public Task<ResponseViewModel> addMenuWithSubMenu(AddMenuWithSubMenu addMenuWithSubMenu);
        //public Task<ResponseViewModel> addMenuWithSubMenuBatch(List<AddMenuWithSubMenu> menuList);
        Task<ResponseViewModel> addMenuWithSubMenuBatch(List<AddMenuWithSubMenu> menuList);

        public Task<ResponseViewModel> getAllAdminListbyPermission();


    }
}
