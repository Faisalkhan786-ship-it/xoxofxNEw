using RepositoryContract;
using ServiceContract;
using System.ComponentModel.DataAnnotations;
using ViewModel;

namespace Service
{
   public class SellerService : ISellerContract
    {
        private readonly IRepositoryManager _repositoryManager;
        public SellerService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<ResponseViewModel> getByIdSeller(Guid sellerId)
        {
            var getByIdSeller = await _repositoryManager.sellerRepository.getByIdSeller(sellerId);
            return getByIdSeller;
        }

        public async Task<ResponseViewModel> getAllSeller()
        {
            var getAllSeller = await _repositoryManager.sellerRepository.getAllSeller();
            return getAllSeller;
        }

        public async Task<ResponseViewModel> getAllSellerForUser()
        {
            var getAllSeller = await _repositoryManager.sellerRepository.getAllSellerForUser();
            return getAllSeller;
        }

        public async Task<ResponseViewModel> addSeller(AddSellerViewModel addSeller)
        {
            var add = await _repositoryManager.sellerRepository.addSeller(addSeller);
            return add;
        }

        public async Task<ResponseViewModel> updateSeller(UpdateSellerViewModel updateSeller)
        {
            var update = await _repositoryManager.sellerRepository.updateSeller(updateSeller);
            return update;
        }

        public async Task<ResponseViewModel> deleteSeller(DeleteSellerViewModel deleteSeller)
        {
            var delete = await _repositoryManager.sellerRepository.deleteSeller(deleteSeller);
            return delete;
        }
       
        public async Task<ResponseViewModel> forgotPassword(ForgotSellerPasswordViewModel forgotSellerPasswordViewModel)
        {
            return await _repositoryManager.sellerRepository.forgotPassword(forgotSellerPasswordViewModel);
        }

        public async Task<ResponseViewModel> sellerIsActive(Guid sellerId)
        {
            var sellerIsActive = await _repositoryManager.sellerRepository.sellerIsActive(sellerId);
            return sellerIsActive;
        }

        public async Task<ResponseViewModel> SellerLogin(SellerLoginViewModel SellerLoginViewModel)
        {
            var appLoginDetails = await _repositoryManager.sellerRepository.SellerLogin(SellerLoginViewModel);
            return appLoginDetails;
        }
    }
}

