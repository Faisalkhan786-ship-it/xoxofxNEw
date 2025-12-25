using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.CartViewModel;

namespace RepositoryContract
{
    public interface ICartRepository
    {
        public Task<ResponseViewModel> addCart(AddCartViewModel addCartViewModel);
        public Task<ResponseViewModel> getCartlist(Guid userId);
        public Task<ResponseViewModel> removeCart(DeleteCartViewModel deleteCartViewModel);

    }
}
