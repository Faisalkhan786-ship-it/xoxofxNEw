using Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using RepositoryContract;
using System.ComponentModel.DataAnnotations;
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

        public async Task<ResponseViewModel> getByIdProduct(getAllProductByIdViewModel getAllProductById)
        {
            Guid productId;
            bool isGuid = Guid.TryParse(getAllProductById.id, out productId);
            if (!isGuid)
            {
                var procedureProductById = Constant.spGetAllProductById;
                var parametersId = new DynamicParameters();
                parametersId.Add("@id", getAllProductById.id.ToString(), DbType.String);

                using (var connection = _dapperContext.createConnection())
                {
                    productId = connection.QueryFirstOrDefault<Guid>(
                        procedureProductById,
                        parametersId,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            var procedureName = Constant.spGetByIdProduct;
            var procedureFAQ = Constant.spGetAllProductFAQbyProductId;
            var SkinInsight = Constant.spGetAllSkinInsightByProductId;
            var Ingredient = Constant.spGetAllProductFAQIngredientbyProductId;
            var FaqWithProduct = Constant.spGetAllProductFAQWithProductbyProductId;
            var AllSimilarProduct = Constant.spGetAllSimilarProductByProductId;
            var procedureImage = Constant.spGetAllSimilarProductByProductIdImage;


            var parameters = new DynamicParameters();
            parameters.Add("@productId", productId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                // 1) Fetch main product details
                var result = (await connection
                    .QueryAsync<Productdetails>(procedureName, parameters, commandType: CommandType.StoredProcedure))
                    .ToList();

                // 2) Fetch FAQs
                var faqList = (await connection
                    .QueryAsync<Faq>(procedureFAQ, parameters, commandType: CommandType.StoredProcedure))
                    .ToList();

                // 3) Fetch Ingredients
                var Ingre = (await connection
                    .QueryAsync<FaqIngredient>(Ingredient, parameters, commandType: CommandType.StoredProcedure))
                    .ToList();

                // 4) Fetch FAQs With Product
                var FaqProduct = (await connection
                    .QueryAsync<FaqWithProduct>(FaqWithProduct, parameters, commandType: CommandType.StoredProcedure))
                    .ToList();

                // 5) Fetch Similar Products and filter invalid ones
                var SimilarPsRaw = (await connection
                    .QueryAsync<SimilarProducts>(AllSimilarProduct, parameters, commandType: CommandType.StoredProcedure))
                    .ToList();



                var SimilarPs = SimilarPsRaw
                    .Where(x =>
                        x.SimilarProductId != Guid.Empty &&
                        x.ProductId != Guid.Empty &&
                        x.SubProductId != Guid.Empty &&
                        x.description != string.Empty &&
                        !string.IsNullOrWhiteSpace(x.ProductName) &&
                        x.Price > 0 &&
                        x.discountPrice > 0 &&
                        x.totalReturn > 0 &&
                        x.weeklyReturn > 0 &&
                        x.month > 0 &&
                        x.MRP != string.Empty
                    )
                    .ToList();
                foreach (var item in SimilarPs)
                {
                    DynamicParameters imgParam = new DynamicParameters();
                    imgParam.Add("@productid", item.SubProductId);

                    var images = await connection.QueryAsync<string>(
                        procedureImage, imgParam, commandType: CommandType.StoredProcedure);

                    item.images = images.ToList();
                }



                // 6) Prepare product data
                var product = result
                    .Select(p => new
                    {
                        id = p.Id,
                        productId = p.ProductId.ToString(),
                        categoryId = p.CategoryId.ToString(),
                        categoryName = p.CategoryName,
                        subCategoryId = p.SubCategoryId.ToString(),
                        subCategoryName = p.SubCategoryName,
                        subCategoryTypeId = p.SubCategoryTypeId.ToString(),
                        subCategoryTypeName = p.SubCategoryTypeName,
                        sellerId = p.SellerId.ToString(),
                        sellerName = p.SellerName,
                        productName = p.ProductName,
                        subName = p.SubName,
                        description = p.Description,
                        rating = p.Rating,
                        noOfRating = p.NoOfRating,
                        stock = p.Stock,
                        price = p.Price,
                        discountPrice = p.DiscountPrice,
                        createdDate = p.CreatedDate,
                        updatedDate = p.UpdatedDate,
                        status = p.Status,
                        active = p.Active,
                        MRP = p.MRP,
                        metaTitle = p.metaTitle,
                        metaDescription = p.metaDescription,
                        metakeyword = p.metakeyword,
                        imageUrls = result.Select(x => x.ImageUrl).ToList(),
                        PerHour = p.PerHour,
                        Unit = p.Unit,
                        Specification = p.Specification,
                        task = p.task,
                        totalReturn = p.totalReturn,
                        weeklyReturn = p.weeklyReturn,
                        month = p.month,
                        NFTurL = p.NFTurL,
                        TOATALMONTH = p.TOATALMONTH,
                        TokenId = p.TokenId,
                    })
                    .FirstOrDefault();

                //// 7) Fetch Skin Insights
                //var skin = (await connection
                //    .QueryAsync<AllSkinInsightProduct>(SkinInsight, parameters, commandType: CommandType.StoredProcedure))
                //    .ToList();

                // 8) Final response
                var response = new ResponseViewModel
                {
                    statusCode = product != null ? 200 : 404,
                    message = product != null ? "Data Found" : "Data Not Found",
                    data = product != null
                        ? new
                        {
                            productDetail = product,
                            FAQ = faqList,
                            FaqIngredient = Ingre,
                            FaqWithProduct = FaqProduct,
                            SimilarProduct = SimilarPs,
                            //skin = skin
                        }
                        : null
                };

                return response;
            }
        }

        public async Task<ResponseViewModel> getAllProduct()
        {
            var procedureName = Constant.spGetAllProduct;

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
            var procedureName = Constant.spGetBestSeller;
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
            var procedureName = Constant.spGetisRecommended;
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
            var procedureName = Constant.spGetIsNewArrial;
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
                var result = await connection.QueryAsync<Product>(procedureName, null, commandType: CommandType.StoredProcedure);
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
                parameters.Add("@subCategoryId", addProduct.subCategoryId == Guid.Empty ? (object)DBNull.Value : addProduct.subCategoryId, DbType.Guid);
                //parameters.Add("@subCategoryTypeId", addProduct.subCategoryTypeId == Guid.Empty ? (object)DBNull.Value : addProduct.subCategoryTypeId, DbType.Guid);
                parameters.Add("@sellerId", addProduct.sellerId == Guid.Empty ? (object)DBNull.Value : addProduct.sellerId, DbType.Guid);
                parameters.Add("@createdBy", addProduct.createdBy == Guid.Empty ? (object)DBNull.Value : addProduct.createdBy, DbType.Guid);


                //  Boolean and String parameters
                parameters.Add("@title", addProduct.title ?? string.Empty, DbType.String);
                parameters.Add("@subTitle", addProduct.subTitle ?? string.Empty, DbType.String);
                parameters.Add("@description", addProduct.description ?? string.Empty, DbType.String);
                parameters.Add("@rating", addProduct.rating, DbType.Decimal);
                parameters.Add("@noOfRating", addProduct.noOfRating, DbType.Int32);
                parameters.Add("@price", addProduct.price, DbType.Decimal);

                parameters.Add("@isAiAgent", addProduct.isAiAgent ? 1 : 0, DbType.Boolean);
                parameters.Add("@isRobotics", addProduct.isRobotics ? 1 : 0, DbType.Boolean);
                parameters.Add("@isTrendingProjects", addProduct.isTrendingProjects ? 1 : 0, DbType.Boolean);

                parameters.Add("@PerHour", addProduct.PerHour, DbType.Int32);
                parameters.Add("@Unit", addProduct.Unit, DbType.Decimal);
                parameters.Add("@Specification", addProduct.Specification ?? string.Empty, DbType.String);
                parameters.Add("@task", addProduct.task ?? string.Empty, DbType.String);
                parameters.Add("@totalReturn", addProduct.totalReturn, DbType.Int32);
                parameters.Add("@weeklyReturn", addProduct.weeklyReturn, DbType.Decimal);
                parameters.Add("@month", addProduct.month, DbType.Decimal);
                parameters.Add("@TOATALMONTH", addProduct.TOATALMONTH, DbType.String);
                parameters.Add("@NFTurL", addProduct.NFTurL, DbType.String);
                parameters.Add("@TokenId", addProduct.TokenId, DbType.String);
                parameters.Add("@AICredite", addProduct.AICredite, DbType.Int32);

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
            parameters.Add("@subCategoryId", updateProduct.subCategoryId, DbType.Guid);
            //parameters.Add("@subCategoryTypeId", updateProduct.subCategoryTypeId, DbType.Guid);
            parameters.Add("@sellerId", updateProduct.sellerId, DbType.Guid);
            parameters.Add("@title", updateProduct.title, DbType.String);
            parameters.Add("@subTitle", updateProduct.subTitle, DbType.String);
            parameters.Add("@rating", updateProduct.rating, DbType.Decimal);
            parameters.Add("@noOfRating", updateProduct.noOfRating, DbType.Int32);
            parameters.Add("@price", updateProduct.price, DbType.Decimal);
            parameters.Add("@description", updateProduct.description, DbType.String);
            parameters.Add("@active", updateProduct.active ? 1 : 0, DbType.Boolean);
            parameters.Add("@updatedBy", updateProduct.updatedBy, DbType.Guid);
            parameters.Add("@isAiAgent", updateProduct.isAiAgent ? 1 : 0, DbType.Boolean);
            parameters.Add("@isRobotics", updateProduct.isRobotics ? 1 : 0, DbType.Boolean);
            parameters.Add("@isTrendingProjects", updateProduct.isTrendingProjects ? 1 : 0, DbType.Boolean);
            parameters.Add("@PerHour", updateProduct.PerHour, DbType.Int32);
            parameters.Add("@Unit", updateProduct.Unit, DbType.Decimal);
            parameters.Add("@Specification", updateProduct.Specification ?? string.Empty, DbType.String);
            parameters.Add("@task", updateProduct.task ?? string.Empty, DbType.String);
            parameters.Add("@totalReturn", updateProduct.totalReturn, DbType.Decimal);
            parameters.Add("@weeklyReturn", updateProduct.weeklyReturn, DbType.Decimal);
            parameters.Add("@month", updateProduct.month, DbType.Int32);
            parameters.Add("@NFTurL", updateProduct.NFTurL, DbType.String);
            parameters.Add("@TOATALMONTH", updateProduct.TOATALMONTH, DbType.String);
            parameters.Add("@TokenId", updateProduct.TokenId, DbType.String);
            parameters.Add("@AICredite", updateProduct.AICredite, DbType.Int32);

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
            parameters.Add("@categoryId", deleteProduct.categoryId, DbType.Guid);
            parameters.Add("@subCategoryId", deleteProduct.subCategoryId, DbType.Guid);
            parameters.Add("@subCategoryTypeId", deleteProduct.subCategoryTypeId, DbType.Guid);
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
            var procedureName = Constant.searchByProduct;
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
            var procedureName = Constant.spUpdateMetaTagByProductId;
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

        public async Task<ResponseViewModel> getAllRoboticsAgents(Guid? ProductId)
        {
            var procedureName = Constant.getAllActiveRobotics;
            DynamicParameters param = new DynamicParameters();
            param.Add("ProductId", ProductId, DbType.Guid);
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
        public async Task<ResponseViewModel> getAllAIAgents(Guid? ProductId)
        {
            var procedureName = Constant.getAllActiveAIAgent;
            DynamicParameters param = new DynamicParameters();
            param.Add("@ProductId", ProductId, DbType.Guid);
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

        public async Task<ResponseViewModel> getAllProjectAgents(Guid? ProductId)
        {
            var procedureName = Constant.getAllActiveProject;
            DynamicParameters param = new DynamicParameters();
            param.Add("ProductId", ProductId,DbType.Guid);
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

