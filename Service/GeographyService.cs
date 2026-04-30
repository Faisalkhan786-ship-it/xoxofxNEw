using RepositoryContract;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.GeographyViewModel;

namespace Service
{
    public class GeographyService: IGeographyContract
    {
        private readonly IRepositoryManager _repositoryManager;
        public GeographyService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task<ResponseViewModel> getAllCountryMethod()
        {
            var getAllCountryMethod = await _repositoryManager.geographyRepository.getAllCountryMethod();
            return getAllCountryMethod;
        }
        public async Task<ResponseViewModel> getAllStateMethod(int Fk_CountryId)
        {
            var getAllStateMethod = await _repositoryManager.geographyRepository.getAllStateMethod(Fk_CountryId);
            return getAllStateMethod;
        }
        public async Task<ResponseViewModel> getAllCityMethod(int Fk_StateId)
        {
            var getAllCityMethod = await _repositoryManager.geographyRepository.getAllCityMethod(Fk_StateId);
            return getAllCityMethod;
        }

        public async Task<ResponseViewModel> getAllContacUs()
        {
            var getAllContacUs = await _repositoryManager.geographyRepository.getAllContacUs();
            return getAllContacUs;
        }
        public async Task<ResponseViewModel> getAllCareerType()
        {
            var getAllCareerType = await _repositoryManager.geographyRepository.getAllCareerType();
            return getAllCareerType;
        }

        public async Task<ResponseViewModel> addContactUs(ContactUsViewModel contactUsViewModel)
        {
            var add = await _repositoryManager.geographyRepository.addContactUs(contactUsViewModel);
            return add;
        }
    }
}
