using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AdminMasterViewModel
    {
        public string? username { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
    public class AdminChangeSponsorIdViewModel
    {
        public string AuthLogin { get; set; }
        public string? SponsorAuthLogin { get; set; }
    }
    public class AdminDownloadExcelViewModel
    {
        public string TransType { get; set; }
    }

    public class NewsViewModel
    {
        public string? NewsId { get; set; }
    }

    public class UpdateViewModel
    {
        public string? NewsId { get; set; }
        public string? News { get; set; }
    }

    public class SettinViewModel
    {
        public int? sID { get; set; }  
    }
    public class UpdateSettingViewModel
    {
        public int SId { get; set; }
        public decimal Limits { get; set; }
    }
    public class LeaseStatementViewModel
    {
        public string? AuthLogin { get; set; }
        public string? productName { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
