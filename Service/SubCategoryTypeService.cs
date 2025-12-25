using RepositoryContract;
using ServiceContract;
using ViewModel;

namespace Service
{
 
    public class SubCategoryTypeService : ISubCategoryTypeContract
    {
        private readonly IRepositoryManager _repositoryManager;
        public SubCategoryTypeService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ResponseViewModel> getByIdSubCategoryType(Guid subCategoryTypeId)
        {
            var getByIdSubCategoryType = await _repositoryManager.subCategoryTypeRepository.getByIdSubCategoryType(subCategoryTypeId);
            return getByIdSubCategoryType;
        }

        public async Task<ResponseViewModel> getAllSubCategoryType()
        {
            var getAllSubCategoryType = await _repositoryManager.subCategoryTypeRepository.getAllSubCategoryType();
            return getAllSubCategoryType;
        }

        public async Task<ResponseViewModel> getAllSubCategoryTypeForUser()
        {
            var getAllSubCategoryType = await _repositoryManager.subCategoryTypeRepository.getAllSubCategoryTypeForUser();
            return getAllSubCategoryType;
        }

        public async Task<ResponseViewModel> addSubCategoryType(AddSubCategoryTypeViewModel addSubCategoryType)
        {
            var add = await _repositoryManager.subCategoryTypeRepository.addSubCategoryType(addSubCategoryType);
            return add;
        }

        public async Task<ResponseViewModel> updateSubCategoryType(UpdateSubCategoryTypeViewModel updateSubCategoryType)
        {
            var update = await _repositoryManager.subCategoryTypeRepository.updateSubCategoryType(updateSubCategoryType);
            return update;
        }

        public async Task<ResponseViewModel> deleteSubCategoryType(DeleteSubCategoryTypeViewModel deleteSubCategoryType)
        {
            var delete = await _repositoryManager.subCategoryTypeRepository.deleteSubCategoryType(deleteSubCategoryType);
            return delete;
        }
    }
}
