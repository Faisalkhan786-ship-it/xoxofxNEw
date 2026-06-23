using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AdminManageViewModel
    {
        public string? Fullname { get; set; }
        public string? AuthLogin { get; set; }
        public string? Active { get; set; }
        public string? PhoneNo { get; set; }
        public string? Email { get; set; }
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
