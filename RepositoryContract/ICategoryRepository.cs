
using ViewModel;

namespace RepositoryContract
{
    public interface ICategoryRepository
    {
        public Task<ResponseViewModel> getByIdCategory(Guid categoryId);
        public Task<ResponseViewModel> getAllCategory();
        public Task<ResponseViewModel> getAllCategoryForUser();
        public Task<ResponseViewModel> addCategory(AddCategoryViewModel addCategory);
        public Task<ResponseViewModel> addCategorytest(AddCategoryViewModel addCategory);
        public Task<ResponseViewModel> getAllCategorytest();


        public Task<ResponseViewModel> updateCategory(UpdateCategoryViewModel updateCategory);
        public Task<ResponseViewModel> deleteCategory(DeleteCategoryViewModel deleteCategory);
        public Task<ResponseViewModel> addCloudImages(AddCloudImages addCloudImages);
        public Task<ResponseViewModel> getCloudImages();
        public Task<ResponseViewModel> deleteCloudImage(int? Id);

    }
}
