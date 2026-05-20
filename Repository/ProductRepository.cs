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
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _dapperContext;
        public ProductRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
       
        public class SimilarProducts
        {
            public int id { get; set; }
            public Guid SimilarProductId { get; set; }
            public Guid ProductId { get; set; }
            public Guid SubProductId { get; set; }
            public string? ProductName { get; set; }
            public string? image { get; set; }
            public string? description { get; set; }
            public string? MRP { get; set; }
            public decimal Price { get; set; }
            public decimal discountPrice { get; set; }
            public int? Unit { get; set; }
            public string? Specification { get; set; }
            public string? task { get; set; }
            public string? TOATALMONTH { get; set; }
            public string? NFTurL { get; set; }
            public decimal totalReturn { get; set; }
            public decimal weeklyReturn { get; set; }
            public int month { get; set; }
            public String? TokenId { get; set; }
            public List<string>? images { get; set; } = new List<string>();           
        }

        public async Task<ResponseViewModel> getByIdProduct(Guid productId)
        {
            var procedureName = Constant.getAllProByproduId;
            var parameters = new DynamicParameters();
            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    parameters.Add("@productId", productId);
                    var result = await connection.QueryAsync<AllProduct>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    var getAllProduct = new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Data Found" : "Data Not Found",
                        data = result
                    };
                    return getAllProduct;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Error occurred: {ex.Message}",
                    data = null
                };
            }
        }

        public async Task<ResponseViewModel> getAllProduct()
        {
            var procedureName = Constant.spGetAllProduct;
            var parameters = new DynamicParameters();
            try
            {
                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryAsync<AllProduct>(procedureName, null, commandType: CommandType.StoredProcedure);
                    var getAllProduct = new ResponseViewModel
                    {
                        statusCode = result.Any() ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                        message = result.Any() ? "Data Found" : "Data Not Found",
                        data = result
                    };
                    return getAllProduct;
                }
            }
            catch (Exception ex)
            {
                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    message = $"Error occurred: {ex.Message}",
                    data = null
                };
            }
        }


        public async Task<ResponseViewModel> getBestSeller()
        {
            var procedureName = "";
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Product>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllProduct = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProduct;
            }
        }

        public async Task<ResponseViewModel> getIsRecommended()
        {
            var procedureName = "";
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Product>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllProduct = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProduct;
            }
        }

        public async Task<ResponseViewModel> getIsNewArrial()
        {
            var procedureName = "";
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<Product>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllProduct = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProduct;
            }
        }
        public async Task<ResponseViewModel> getAllProductForUser()
        {
            var procedureName = Constant.getAllProductForUser;
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<AllProduct>(procedureName, null, commandType: CommandType.StoredProcedure);
                var getAllProductForUser = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProductForUser;
            }
        }
        public async Task<ResponseViewModel> getAllProductDetails(Int32 id)
        {
            var procedureName = Constant.spGetAllProductDetails;
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int32);
                var result = await connection.QueryAsync<ProductDetails>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getAllProductForUser = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProductForUser;
            }
        }

        public async Task<ResponseViewModelProduct> addProduct(AddProductViewModel addProduct)
        {
            var response = new ResponseViewModelProduct();
            var procedureName = Constant.spAddProduct;
            var parameters = new DynamicParameters();


            try
            {

                // Guid Parameters
                parameters.Add("@categoryId", addProduct.categoryId == Guid.Empty ? (object)DBNull.Value : addProduct.categoryId, DbType.Guid);
                parameters.Add("@createdBy", addProduct.createdBy == Guid.Empty ? (object)DBNull.Value : addProduct.createdBy, DbType.Guid);
                //  Boolean and String parameters
                parameters.Add("@productName", addProduct.productName ?? string.Empty, DbType.String);
                parameters.Add("@tittle", addProduct.tittle ?? string.Empty, DbType.String);
                parameters.Add("@type", addProduct.type ?? string.Empty, DbType.String);
                parameters.Add("@rOI", addProduct.rOI, DbType.Decimal);
                parameters.Add("@minInvest", addProduct.minInvest, DbType.Decimal);
                parameters.Add("@winRate", addProduct.winRate, DbType.Decimal);
                parameters.Add("@Traders", addProduct.Traders, DbType.Decimal);
                parameters.Add("@active", addProduct.active, DbType.Boolean);
               

                using (var connection = _dapperContext.createConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModelProduct>(
                        procedureName,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null && result.statusCode == 1)
                    {
                        result.statusCode = (int)HttpStatusCode.OK;
                        result.message = result.message;
                        result.data = new OrderResponseData
                        {
                            productId = result.productId
                        };
                    }
                    else
                    {
                        result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                //  Error Catch Block
                response.statusCode = (int)HttpStatusCode.ExpectationFailed;
                response.message = ex.Message;
                response.data = null;

                return response;
            }
        }

        public class OrderResponseData
        {
            public Guid productId { get; set; }
        }

        public async Task<ResponseViewModel> updateProduct(UpdateProductViewModel updateProduct)
        {
            var procedureName = Constant.spUpdateProduct;
            var parameters = new DynamicParameters();
            parameters.Add("@productId", updateProduct.productId, DbType.Guid);
            parameters.Add("@categoryId", updateProduct.categoryId, DbType.Guid);
            parameters.Add("@productName", updateProduct.productname, DbType.String);
            parameters.Add("@tittle", updateProduct.tittle, DbType.String);
            parameters.Add("@type", updateProduct.type, DbType.String);
            parameters.Add("@rOI", updateProduct.rOI, DbType.Decimal);
            parameters.Add("@minInvest", updateProduct.minInvest, DbType.Decimal);
            parameters.Add("@winRate", updateProduct.winRate, DbType.Decimal);
            parameters.Add("@Traders", updateProduct.Traders, DbType.Decimal);
            parameters.Add("@active", updateProduct.active, DbType.Boolean);
            parameters.Add("@updatedBy", updateProduct.updatedBy, DbType.Guid);

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
        public async Task<ResponseViewModel> deleteProduct(DeleteProductViewModel deleteProduct)
        {
            var procedureName = Constant.spDeleteProduct;
            var parameters = new DynamicParameters();
            parameters.Add("@productId", deleteProduct.productId, DbType.Guid);
            parameters.Add("@updatedBy", deleteProduct.updatedBy, DbType.Guid);

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
        public async Task<ResponseViewModel> getByIdProductImage(Guid productImageId)
        {
            var procedureName = Constant.spGetByIdProductImage;
            var parameters = new DynamicParameters();
            parameters.Add("@productImageId", productImageId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<ProductImage>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getbyIdProductImage = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getbyIdProductImage;
            }
        }
        public async Task<ResponseViewModel> getAllProductImage(Guid productId)
        {
            var procedureName = Constant.spGetAllProductImage;
            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<ProductImage>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getAllProductImage = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProductImage;
            }
        }
        public async Task<ResponseViewModel> getAllProductImageForUser(Guid productId)
        {
            var procedureName = Constant.spGetAllProductImageForUser;
            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId, DbType.Guid);
            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<ProductImage>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getAllProductImageForUser = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProductImageForUser;
            }
        }
    
        //Add Product Image
        public async Task<ResponseViewModel> addProductImage(AddProductImageViewModel addProductImage)
        {
            var uploadedImageUrls = new List<string>();
            var procedureName = Constant.spAddProductImage;
            ResponseViewModel finalResult = new ResponseViewModel();

            if (addProductImage.image != null && addProductImage.image.Count > 0)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using (var connection = _dapperContext.createConnection())
                {
                    foreach (var file in addProductImage.image)
                    {
                        // 🔹 Upload to Cloudflare
                        using var content = new MultipartFormDataContent();
                        using var stream = file.OpenReadStream();
                        content.Add(new StreamContent(stream), "file", file.FileName);

                        var response = await client.PostAsync(
                            $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                        );

                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            finalResult.statusCode = 0;
                            finalResult.message = $"Image upload failed for {file.FileName}";
                            finalResult.data = jsonResponse;
                            return finalResult;
                        }

                        var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                        var imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
                        uploadedImageUrls.Add(imagePath);

                        // 🔹 Insert into DB (each image = new row)
                        var parameters = new DynamicParameters();
                        parameters.Add("@productId", addProductImage.productId, DbType.Guid);
                        parameters.Add("@title", addProductImage.title, DbType.String);
                        parameters.Add("@image", imagePath, DbType.String);
                        parameters.Add("@createdBy", addProductImage.createdBy, DbType.Guid);

                        var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                            procedureName, parameters, commandType: CommandType.StoredProcedure
                        );

                        // just last result ko rakh rahe hain (for overall response)
                        finalResult = result;
                    }
                }
            }

            finalResult.statusCode = (int)HttpStatusCode.OK;
            finalResult.message = "All images uploaded and saved successfully.";
            finalResult.data = uploadedImageUrls;
            return finalResult;
        }


        //Update Product Image 
        public async Task<ResponseViewModel> updateProductImage(UpdateProductImageViewModel updateProductImage)
        {
            var uploadedImageUrls = new List<string>();
            var procedureName = Constant.spUpdateProductImage;
            ResponseViewModel finalResult = new ResponseViewModel();

            if (updateProductImage.image != null && updateProductImage.image.Count > 0)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExtractToken.ApiToken);

                using (var connection = _dapperContext.createConnection())
                {
                    foreach (var file in updateProductImage.image)
                    {
                        // Upload to Cloudflare
                        using var content = new MultipartFormDataContent();
                        using var stream = file.OpenReadStream();
                        content.Add(new StreamContent(stream), "file", file.FileName);

                        var response = await client.PostAsync(
                            $"https://api.cloudflare.com/client/v4/accounts/{ExtractToken.AccountId}/images/v1", content
                        );

                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            finalResult.statusCode = 0;
                            finalResult.message = $"Image upload failed for {file.FileName}";
                            finalResult.data = jsonResponse;
                            return finalResult;
                        }

                        var json = System.Text.Json.JsonDocument.Parse(jsonResponse);
                        var imagePath = json.RootElement.GetProperty("result").GetProperty("variants")[0].GetString();
                        uploadedImageUrls.Add(imagePath);

                        // 🔹 Insert into DB (each image = new row)
                        var parameters = new DynamicParameters();
                        parameters.Add("@productImageId", updateProductImage.productImageId, DbType.Guid);
                        parameters.Add("@productId", updateProductImage.productId, DbType.Guid);
                        parameters.Add("@title", updateProductImage.title, DbType.String);
                        parameters.Add("@image", imagePath, DbType.String);
                        parameters.Add("@updatedBy", updateProductImage.updatedBy, DbType.Guid);


                        var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                            procedureName, parameters, commandType: CommandType.StoredProcedure
                        );

                        // just last result ko rakh rahe hain (for overall response)
                        finalResult = result;
                    }
                }
            }

            finalResult.statusCode = (int)HttpStatusCode.OK;
            finalResult.message = "All images uploaded and saved successfully.";
            finalResult.data = uploadedImageUrls;
            return finalResult;
        }

        //delete Product Image 
        public async Task<ResponseViewModel> deleteProductImage(DeleteProductImageViewModel deleteProductImage)
        {
            var procedureName = Constant.spDeleteProductImage;
            var parameters = new DynamicParameters();
            parameters.Add("@productImageId", deleteProductImage.productImageId, DbType.Guid);
            parameters.Add("@productId", deleteProductImage.productId, DbType.Guid);
            parameters.Add("@updatedBy", deleteProductImage.updatedBy, DbType.Guid);
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
                    result.message = result.message;
                    result.statusCode = (int)HttpStatusCode.ExpectationFailed;
                }
                return result;
            }
        }

        //Get Product Image by Id
        public async Task<ResponseViewModel> getByIdImage(Guid productId)
        {
            var procedureName = Constant.spGetAllImageById;
            using (var connection = _dapperContext.createConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@productId", productId, DbType.Guid);
                var result = await connection.QueryAsync<ProductbyIdImage>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var getAllProductForUser = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProductForUser;
            }
        }
       
        private class SPResponseModel
        {
            public int statusCode { get; set; }
            public string message { get; set; }
        }

        //Get Product Image by Id
        public async Task<ResponseViewModel> searchProductNew(string commonTypeSearch)
        {
            var procedureName = "";
            using (var connection = _dapperContext.createConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@commonTypeSearch", commonTypeSearch);
                var result = await connection.QueryAsync<searchProductNew>(procedureName, param, null, commandType: CommandType.StoredProcedure);
                var getAllProduct = new ResponseViewModel
                {
                    statusCode = result.Count() == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
                    message = result.Count() == 0 ? "Data Not Found" : "Data Found",
                    data = result
                };
                return getAllProduct;
            }
        }
   
        public async Task<ResponseViewModel> updateMetaTagsByProductId(UpdateMetaTagViewModel updateMetaTagViewModel)
        {
            var procedureName = "";
            var parameters = new DynamicParameters();
            parameters.Add("@productId", updateMetaTagViewModel.productId, DbType.Guid);
            parameters.Add("@metaTitle", updateMetaTagViewModel.metaTitle, DbType.String);
            parameters.Add("@metaDescription", updateMetaTagViewModel.metaDescription, DbType.String);
            parameters.Add("@metakeyword", updateMetaTagViewModel.metaKeyword, DbType.String);

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

