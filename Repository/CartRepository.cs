using Common;
using Dapper;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using static ViewModel.CartViewModel;

namespace Repository
{
    public class CartRepository: ICartRepository
    {
        private readonly DapperContext _dapperContext;
        public CartRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;
        public async Task<ResponseViewModel> addCart(AddCartViewModel addCartViewModel)
        {           
            var procedureName = Constant.addProductInCart;
            var parameters = new DynamicParameters();
            parameters.Add("@userId", addCartViewModel.userId, DbType.Guid);
            parameters.Add("@productId", addCartViewModel.productId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );
                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }
        public async Task<ResponseViewModel> getCartlist(Guid userId)
        {
            var procedureName = Constant.getCartList;
            DynamicParameters param = new DynamicParameters();
            param.Add("@userId", userId, DbType.Guid);
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
                        message = "Get Cart Data.",
                        data = combinedData
                    };
                }
                else
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "No Cart Data."
                    };
                }
            }
        }
        public async Task<ResponseViewModel> removeCart(DeleteCartViewModel deleteCartViewModel)
        {
            var procedureName = Constant.removeProductInCart;
            var parameters = new DynamicParameters();
            parameters.Add("@userId", deleteCartViewModel.userId, DbType.Guid);
            parameters.Add("@productId", deleteCartViewModel.productId, DbType.Guid);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                    procedureName, parameters, commandType: CommandType.StoredProcedure
                );
                result.statusCode = result.statusCode == 1 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.ExpectationFailed;
                return result;
            }
        }
    }
}
