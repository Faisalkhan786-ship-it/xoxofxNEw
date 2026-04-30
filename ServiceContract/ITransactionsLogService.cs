using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
    public interface ITransactionsLogService
    {
        public Task<ResponseViewModel> addTransactionsLog(TransactionsLogViewModel transactionsLogViewModel);
    }
}
