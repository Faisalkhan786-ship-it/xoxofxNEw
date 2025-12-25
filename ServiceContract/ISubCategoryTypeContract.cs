using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceContract
{
   public interface ISubCategoryTypeContract
    {
        public Task<ResponseViewModel> getByIdSubCategoryType(Guid subCategoryTypeId);
        public Task<ResponseViewModel> getAllSubCategoryType();
        public Task<ResponseViewModel> getAllSubCategoryTypeForUser();
        public Task<ResponseViewModel> addSubCategoryType(AddSubCategoryTypeViewModel addSubCategoryType);
        public Task<ResponseViewModel> updateSubCategoryType(UpdateSubCategoryTypeViewModel updateSubCategoryType);
        public Task<ResponseViewModel> deleteSubCategoryType(DeleteSubCategoryTypeViewModel deleteSubCategoryType);
    }
}
