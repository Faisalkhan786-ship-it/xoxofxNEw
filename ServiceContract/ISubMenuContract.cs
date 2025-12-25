using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface ISubMenuContract
    {
        public Task<ResponseViewModel> getByIdSubMenu(Guid id);
        public Task<ResponseViewModel> getAllSubMenu();
        public Task<ResponseViewModel> addSubMenu(AddSubMenuViewModel addSubMenuViewModel);
        public Task<ResponseViewModel> updateSubMenu(UpdateSubMenuViewModel updateSubMenuViewModel);
        public Task<ResponseViewModel> deleteSubMenu(DeleteSubMenuViewModel deleteSubMenuViewModel);
        public Task<ResponseViewModel> getSubMenubyMenuId(Guid menuId);

    }
}
