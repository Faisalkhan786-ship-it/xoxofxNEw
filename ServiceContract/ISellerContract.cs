using ViewModel;

namespace ServiceContract
{
    public interface ISellerContract
    {
        public Task<ResponseViewModel> getByIdSeller(Guid sellerId);
        public Task<ResponseViewModel> getAllSeller();
        public Task<ResponseViewModel> getAllSellerForUser();
        public Task<ResponseViewModel> addSeller(AddSellerViewModel addSeller);
        public Task<ResponseViewModel> updateSeller(UpdateSellerViewModel updateSeller);
        public Task<ResponseViewModel> deleteSeller(DeleteSellerViewModel deleteSeller);
        public Task<ResponseViewModel> forgotPassword(ForgotSellerPasswordViewModel forgotSellerPasswordViewModel);
        public Task<ResponseViewModel> sellerIsActive(Guid sellerId);
        public Task<ResponseViewModel> SellerLogin(SellerLoginViewModel SellerLoginViewModel);

    }
      
}
