using Microsoft.AspNetCore.Cors.Infrastructure;
using RepositoryContract;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.CartViewModel;

namespace Service
{
    public class CartService: ICartContract
    {
        private readonly IRepositoryManager _repositoryManager;
        public CartService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> addCart(AddCartViewModel addCartViewModel)
        {
            var add = await _repositoryManager.cartRepository.addCart(addCartViewModel);
            return add;
        }
        public async Task<ResponseViewModel> getCartlist(Guid userId)
        {
            var getCartlist = await _repositoryManager.cartRepository.getCartlist(userId);
            return getCartlist;
        }
        public async Task<ResponseViewModel> removeCart(DeleteCartViewModel deleteCartViewModel)
        {
            var remove = await _repositoryManager.cartRepository.removeCart(deleteCartViewModel);
            return remove;
        }
    }
}
