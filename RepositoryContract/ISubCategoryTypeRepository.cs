
using ViewModel;

namespace RepositoryContract
{
    public interface ISubCategoryTypeRepository
    {
        public Task<ResponseViewModel> getByIdSubCategoryType(Guid subCategoryTypeId);
        public Task<ResponseViewModel> getAllSubCategoryType();
        public Task<ResponseViewModel> getAllSubCategoryTypeForUser();
        public Task<ResponseViewModel> addSubCategoryType(AddSubCategoryTypeViewModel addSubCategoryType);
        public Task<ResponseViewModel> updateSubCategoryType(UpdateSubCategoryTypeViewModel updateSubCategoryType);
        public Task<ResponseViewModel> deleteSubCategoryType(DeleteSubCategoryTypeViewModel deleteSubCategoryType);
    }
}
