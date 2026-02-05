using RepositoryContract;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Service
{
    public class SubMenuService : ISubMenuContract
    {
        private readonly IRepositoryManager _repositoryManager;
        public SubMenuService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ResponseViewModel> getByIdSubMenu(Guid SubMenuId)
        {
            var getByIdSubMenu = await _repositoryManager.subMenuRepository.getByIdSubMenu(SubMenuId);
            return getByIdSubMenu;
        }

        public async Task<ResponseViewModel> getAllSubMenu()
        {
            var getAllSubMenu = await _repositoryManager.subMenuRepository.getAllSubMenu();
            return getAllSubMenu;
        }

        public async Task<ResponseViewModel> addSubMenu(AddSubMenuViewModel addSubMenuViewModel)
        {
            var addSubMenu = await _repositoryManager.subMenuRepository.addSubMenu(addSubMenuViewModel);
            return addSubMenu;
        }

        public async Task<ResponseViewModel> updateSubMenu(UpdateSubMenuViewModel updateSubMenuViewModel)
        {
            var updateSubMenu = await _repositoryManager.subMenuRepository.updateSubMenu(updateSubMenuViewModel);
            return updateSubMenu;
        }

        public async Task<ResponseViewModel> deleteSubMenu(DeleteSubMenuViewModel deleteSubMenuViewModel)
        {
            var deleteSubMenu = await _repositoryManager.subMenuRepository.deleteSubMenu(deleteSubMenuViewModel);
            return deleteSubMenu;
        }

        public async Task<ResponseViewModel> getSubMenubyMenuId(Guid menuId)
        {
            var getSubMenubyMenuId = await _repositoryManager.subMenuRepository.getSubMenubyMenuId(menuId);
            return getSubMenubyMenuId;
        }

    }
}
