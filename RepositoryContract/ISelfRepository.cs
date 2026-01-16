using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface ISelfRepository
    {
        Result2<UserWalletDetailsMasterViewModel> GenerateWalletAddress(RequestUserWalletDetailsViewModel model);
        public Task<ResponseViewModel> getAllWalletAddress();
        Task<Result2<AddFundModel>> USDTBalanceAsync(RequestWalletAddressModel model);
        Task<Result2<AddFundModel>> SItoBalanceAsync(RequestWalletAddressModel model);
        Task<Result2<resposeAddFundModel>> SendUSDTDepositRequest(RequestDepositusdtModel model);
        Task<Result2<resposeAddFundModel>> SendSITODepositRequest(RequestDepositusdtModel model);
        public Task<ResponseViewModel> getAllWalletAddressByURID(Guid URID);
        public Task<ResponseViewModel> GetSelfDepsiteByURID(Guid URID);
        public Task<ResponseViewModel> GetAllSelfDepositeAdmin();

    }
}
