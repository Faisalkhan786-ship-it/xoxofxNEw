using Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using RepositoryContract;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using ViewModel;
using static Model.ModelType;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _dapperContext;
        public CategoryRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> getByIdCategory(Guid categoryId)
        {
            var procedureName = Constant.spGetByIdCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", categoryId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Category>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdCategory = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdCategory;
            }
        }
        public async Task<ResponseViewModel> getAllCategory()
        {
            var procedureName = Constant.spGetAllCategory;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Category>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllCategory = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllCategory;
            }
        }

        public async Task<ResponseViewModel> getAllCategorytest()
        {
            var procedureName = Constant.SpGetAllCategorytest;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Category>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllCategory = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllCategory;
            }
        }
        public async Task<ResponseViewModel> getAllCategoryForUser()
        {
            var procedureName = Constant.spGetAllCategoryForUser;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Category>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllCategory = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllCategory;
            }
        }


        public async Task<ResponseViewModel> addCategory(AddCategoryViewModel addCategory)
        {
            string imagePath = null;

            // Use addCategory.image
            var file = addCategory.image;

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
            var procedureName = Constant.addCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@name", addCategory.name, DbType.String);
            parameters.Add("@image", imagePath, DbType.String); 
            parameters.Add("@createdBy", addCategory.createdBy, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }      

        public async Task<ResponseViewModel> addCategorytest(AddCategoryViewModel addCategory)
        {
            string imagePath = null;

            // Use addCategory.image
            var file = addCategory.image;

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
            var procedureName = Constant.spAddCategorytest;
            var parameters = new DynamicParameters();
            parameters.Add("@name", addCategory.name, DbType.String);
            parameters.Add("@image", imagePath, DbType.String); 
            parameters.Add("@createdBy", addCategory.createdBy, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );

                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;

                return result;
            }
        }

        public async Task<ResponseViewModel> updateCategory(UpdateCategoryViewModel updateCategory)
        {
            string imagePath = null;

            // Use addCategory.image
            var file = updateCategory.image;

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
            var procedureName = Constant.spUpdateCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", updateCategory.categoryId, DbType.Guid);
            parameters.Add("@name", updateCategory.name, DbType.String);
            parameters.Add("@active", updateCategory.active ? 1 : 0, DbType.Boolean);
            parameters.Add("@image", imagePath, DbType.String);
            parameters.Add("@updatedBy", updateCategory.updatedBy, DbType.Guid);
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
        
        public async Task<ResponseViewModel> deleteCategory(DeleteCategoryViewModel deleteCategory)
        {
            var procedureName = Constant.spDeleteCategory;
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", deleteCategory.categoryId, DbType.Guid);
            parameters.Add("@updatedBy", deleteCategory.updatedBy, DbType.Guid);
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

        public async Task<ResponseViewModel> addCloudImages(AddCloudImages addCloudImages)
        {
            string imagePath = null;
            var file = addCloudImages.image;

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
                var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
            }
            var procedureName = Constant.addcloudImage;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", addCloudImages.Id, DbType.Int32);
            parameters.Add("@ImageName", addCloudImages.ImageName, DbType.String);
            parameters.Add("@ImageURL", imagePath, DbType.String);
            //parameters.Add("@IsActive", addCloudImages.IsActive ? 1 : 0, DbType.Boolean);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );
                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }

        public async Task<ResponseViewModel> getCloudImages()
        {
            var procedureName = Constant.getcloudImage;

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<CloudImageModel>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getCloudImages = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getCloudImages;
            }
        }

        public class CloudImageModel
        {
            public int? Id { get; set; }
            public string? ImageName { get; set; }
            public string? ImageURL { get; set; }
            public string? Status { get; set; }
            public string? CreatedDate { get; set; }
        }

        public async Task<ResponseViewModel> deleteCloudImage(int? Id)
        {
            var procedureName = Constant.deleteCloudImage;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32);
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

    }
}
