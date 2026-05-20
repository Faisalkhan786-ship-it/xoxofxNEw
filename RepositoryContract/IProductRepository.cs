
using ViewModel;

namespace RepositoryContract
{
   public interface IProductRepository
    {
        //public Task<ResponseViewModel> getByIdProduct(Guid productId);
        public Task<ResponseViewModel> getByIdProduct(getAllProductByIdViewModel getAllProductById);

        public Task<ResponseViewModel> getAllProduct();
        public Task<ResponseViewModel> getBestSeller();
        public Task<ResponseViewModel> getIsRecommended();
        public Task<ResponseViewModel> getIsNewArrial();
        public Task<ResponseViewModel> getAllProductForUser();
        public Task<ResponseViewModel> getAllProductDetails(Int32 id);
        public Task<ResponseViewModelProduct> addProduct(AddProductViewModel addProduct);
        public Task<ResponseViewModel> updateProduct(UpdateProductViewModel updateProduct);
        public Task<ResponseViewModel> deleteProduct(DeleteProductViewModel deleteProduct);


        public Task<ResponseViewModel> getByIdProductImage(Guid productImageId);
        public Task<ResponseViewModel> getAllProductImage(Guid productId);
        public Task<ResponseViewModel> getAllProductImageForUser(Guid productId);
        public Task<ResponseViewModel> addProductImage(AddProductImageViewModel addProductImage);
        public Task<ResponseViewModel> updateProductImage(UpdateProductImageViewModel updateProductImage);
        public Task<ResponseViewModel> deleteProductImage(DeleteProductImageViewModel deleteProductImage);
        public Task<ResponseViewModel> getByIdImage(Guid productId);
       
       
        public Task<ResponseViewModel> searchProductNew(string commonTypeSearch);

        public Task<ResponseViewModel> updateMetaTagsByProductId(UpdateMetaTagViewModel updateMetaTagViewModel);

    }
}
