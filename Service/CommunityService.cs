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
    public class CommunityService : ICommunityService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CommunityService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> GetDirectMemberDetails(DirectMemberViewModel directMemberViewModel)
        {
            var GetDirectMember = await _repositoryManager.communityRepository.GetDirectMemberDetails(directMemberViewModel);
            return GetDirectMember;
        }
        public async Task<ResponseViewModel> GetPersonalTeam(PersonalTeamViewModel PersonalTeamViewModel)
        {
            var GetPersonalTeam = await _repositoryManager.communityRepository.GetPersonalTeam(PersonalTeamViewModel);
            return GetPersonalTeam;
        }
        public async Task<ResponseViewModel> getPersonalTeamList(PersonalTeamReportViewModel personalTeamReportViewModel)
        {
            var getPersonalTeamList = await _repositoryManager.communityRepository.getPersonalTeamList(personalTeamReportViewModel);
            return getPersonalTeamList;
        }
        public async Task<ResponseViewModel> getAgentLeaseCredit(Guid urid)
        {
            var getAgentLeaseCredit = await _repositoryManager.communityRepository.getAgentLeaseCredit(urid);
            return getAgentLeaseCredit;
        }
        public async Task<ResponseViewModel> getdownLineTreeDetails(Guid URID)
        {
            var getdownLineTreeDetails = await _repositoryManager.communityRepository.getdownLineTreeDetails(URID);
            return getdownLineTreeDetails;
        }
        public async Task<ResponseViewModel> getDownlineLeftRightCount(DownlineLeftRightCountViewModel downlineLeftRightCountViewModel)
        {
            var getDownlineLeftRightCount = await _repositoryManager.communityRepository.getDownlineLeftRightCount(downlineLeftRightCountViewModel);
            return getDownlineLeftRightCount;
        }
        public async Task<ResponseViewModel> getLeftRightdownline(LeftRightdownlineTeamViewModel leftRightdownlineTeamViewModel)
        {
            var getLeftRightdownline = await _repositoryManager.communityRepository.getLeftRightdownline(leftRightdownlineTeamViewModel);
            return getLeftRightdownline;
        }
    }
}
