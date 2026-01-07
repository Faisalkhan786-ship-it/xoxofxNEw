using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AdminManageViewModel
    {
        public string? AuthLogin { get; set; }
        public string? Fname { get; set; }
        public string? Active { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Kid { get; set; }
        public string? Walletid { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
    public class AppUnApprentViewModel
    {
        public string? AuthLogin { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
