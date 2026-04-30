using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.GeographyViewModel;

namespace ServiceContract
{
    public interface IGeographyContract
    {
        public Task<ResponseViewModel> getAllCountryMethod();
        public Task<ResponseViewModel> getAllStateMethod(int Fk_CountryId);
        public Task<ResponseViewModel> getAllCityMethod(int Fk_StateId);
   
    }
}
