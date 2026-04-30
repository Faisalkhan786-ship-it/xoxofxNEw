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
    public class TransactionsLogService: ITransactionsLogService
    {
        private readonly IRepositoryManager _repositoryManager;
        public TransactionsLogService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> addTransactionsLog(TransactionsLogViewModel transactionsLogViewModel)
        {
            var add = await _repositoryManager.transactionsLogRepository.addTransactionsLog(transactionsLogViewModel);
            return add;
        }
    }
}
