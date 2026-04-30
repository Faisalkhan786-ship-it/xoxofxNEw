using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface ITransactionsLogRepository
    {
        public Task<ResponseViewModel> addTransactionsLog(TransactionsLogViewModel transactionsLogViewModel);
    }
}
