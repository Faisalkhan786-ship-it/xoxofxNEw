using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TransactionsLogViewModel
    {
        public string NetworkChain { get; set; }
        public string TransactionHash { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TokenSymbol { get; set; }
    }   
}
