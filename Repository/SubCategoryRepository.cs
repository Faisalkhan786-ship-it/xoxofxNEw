using Common;
using Dapper;
using RepositoryContract;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DapperContext _dapperContext;
        public SubCategoryRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;

        //get all sub category for admin by edit id
        public async Task<ResponseViewModel> getByIdSubCategory(Guid subCategoryId)
        {
            var response = new ResponseViewModel();
            try
            {
                var procedureName = Constant.spGetByIdSubCategory;
                var parameters = new DynamicParameters();
                parameters.Add("@subCategoryId", subCategoryId, DbType.Guid);

                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<SubCategoryNew>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    response.statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
                    response.message = result.Any() ? "Data Found" : "Data Not Found";
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                response.statusCode = (int)HttpStatusCode.InternalServerError;
                response.message = "Error: " + ex.Message;
                response.data = null;
            }

            return response;
        }

        //get all sub category for admin
        public async Task<ResponseViewModel> getAllSubCategory()
        {
            var response = new ResponseViewModel();

            try
            {
                var procedureName = Constant.spGetAllSubCategory;

                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<SubCategory>(
                        procedureName, null, commandType: CommandType.StoredProcedure);

                    response.statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound;
                    response.message = result.Any() ? "Data Found" : "Data Not Found";
                    response.data = result;
                }
            }
            catch (Exception ex)
            {
                response.statusCode = (int)HttpStatusCode.InternalServerError;
                response.message = "Error: " + ex.Message;
                response.data = null;
            }

            return response;
        }

        //get All Sub Category for user
        public async Task<ResponseViewModel> getAllSubCategoryForUser()
        {
            var procedureName = Constant.spGetAllSubCategoryForUser;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<SubCategory>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllSubCategory = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllSubCategory;
            }
        }

        //Add Sub Category
        public async Task<ResponseViewModel> addSubCategory(AddSubCategoryViewModel addSubCategory)
        {
            string imagePath = null;
            var file = addSubCategory.image;

            if (file != null && file.Length > 0)
            {
                // Cloudflare Upload using ExtractToken
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = file.OpenReadStream();
                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModel { statusCode = 0, message = "Image upload failed", data = jsonResponse };

                // Cloudflare URL extract karna
                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            }

            // Stored procedure call
            var procedureName = Constant.spAddSubCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", addSubCategory.categoryId, DbType.Guid);
            parameters.Add("@name", addSubCategory.name, DbType.String);
            parameters.Add("@createdBy", addSubCategory.createdBy, DbType.Guid);
            parameters.Add("@image", imagePath, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }

        //Update SUb Category 
        public async Task<ResponseViewModel> updateSubCategory(UpdateSubCategoryViewModel updateSubCategory)
        {
            string imagePath = null;

            // Use addCategory.image
            var file = updateSubCategory.image;

            if (file != null && file.Length > 0)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using var content = new MultipartFormDataContent();
                using var stream = file.OpenReadStream();
                content.Add(new StreamContent(stream), "file", file.FileName);

                var response = await client.PostAsync(
                    $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new ResponseViewModel { statusCode = 0, message = "Image upload failed", data = jsonResponse };

                // Cloudflare URL extract karna
                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            }

            var procedureName = Constant.updateSubCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@subCategoryId", updateSubCategory.subCategoryId, DbType.Guid);
            parameters.Add("@name", updateSubCategory.name, DbType.String);
            parameters.Add("@active", updateSubCategory.active ? 1 : 0, DbType.Boolean);
            parameters.Add("@updatedBy", updateSubCategory.updatedBy, DbType.Guid);
            parameters.Add("@image", imagePath, DbType.String);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                    result.message = result.message;
                }
                else if (result.statusCode == 0)
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                return result;
            }
        }

        //Delete sub category
        public async Task<ResponseViewModel> deleteSubCategory(DeleteSubCategoryViewModel deleteSubCategory)
        {
            var procedureName = Constant.spDeleteSubCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@subCategoryId", deleteSubCategory.subCategoryId, DbType.Guid);
            parameters.Add("@updatedBy", deleteSubCategory.updatedBy, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (result.statusCode == 1)
                {
                    result.statusCode = (int)HttpStatusCode.OK;
                    result.message = result.message;
                }
                else if (result.statusCode == 0)
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                else
                {
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    result.message = result.message;
                }
                return result;
            }
        }

        public async Task<ResponseViewModel> getAllRoboticsAgentsSubCat(Guid? SubcategoryId)
        {
            var procedureName = Constant.allActiveRoboticsSubCat;
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubcategoryId", SubcategoryId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Robots.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Robots."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> getAllAIAgentsSubCat(Guid? SubcategoryId)
        {
            var procedureName = Constant.allActiveAIAgentSubCat;
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubcategoryId", SubcategoryId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Ge Robots.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Robots."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> getAllProjectAgentsSubCat(Guid? SubcategoryId)
        {
            var procedureName = Constant.allActiveProjectSubCat;
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubcategoryId", SubcategoryId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync(procedureName, param, commandType: CommandType.StoredProcedure);

                if (result != null && result.Any())
                {
                    var combinedData = new
                    {
                        Event = result.ToList(),
                    };

                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Get Robots.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Robots."
                    };
                }
            }
        }
    }
}
