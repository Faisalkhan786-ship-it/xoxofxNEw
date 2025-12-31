using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class CommunityViewModel
    {
    }
    public class DirectMemberViewModel
    {
        public Guid URID { get; set; }
        public string? StatusId { get; set; }
        public string? Loginid { get; set; }
    }
    public class PersonalTeamViewModel
    {
        public string AuthLogin { get; set; }
        //public string? uRank { get; set; }
        public string? lvl { get; set; }
        public string? statusId { get; set; }
    }
    public class PersonalTeamReportViewModel
    {
        public string AuthLogin { get; set; }
        //public string? uRank { get; set; }
        public string? lvl { get; set; }
        public string? statusid { get; set; }
    }
}
