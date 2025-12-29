using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RepositoryContract
{
    public interface ICommunityRepository
    {
        public Task<ResponseViewModel> GetDirectMemberDetails(DirectMemberViewModel directMemberViewModel);
        public Task<ResponseViewModel> GetPersonalTeam(PersonalTeamViewModel PersonalTeamViewModel);
        public Task<ResponseViewModel> getPersonalTeamList(PersonalTeamReportViewModel personalTeamReportViewModel);
        public Task<ResponseViewModel> getAgentLeaseCredit(Guid urid);

    }
}
