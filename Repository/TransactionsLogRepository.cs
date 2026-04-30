using Dapper;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Repository
{
    public class TransactionsLogRepository: ITransactionsLogRepository
    {                             

        private readonly DapperContext _dapperContext;
        public TransactionsLogRepository(DapperContext dapperContext) =>
            _dapperContext = dapperContext;

        public async Task<ResponseViewModel> addTransactionsLog(TransactionsLogViewModel transactionsLogViewModel)
        {
            var procedureName = "SPInsertAPITransactionsLog";

            var parameters = new DynamicParameters();
            parameters.Add("@NetworkChain", transactionsLogViewModel.NetworkChain, DbType.String);
            parameters.Add("@TransactionHash", transactionsLogViewModel.TransactionHash, DbType.String);
            parameters.Add("@DateTime", transactionsLogViewModel.DateTime, DbType.DateTime);
            parameters.Add("@Amount", transactionsLogViewModel.Amount, DbType.Decimal);
            parameters.Add("@FromAddress", transactionsLogViewModel.FromAddress, DbType.String);
            parameters.Add("@ToAddress", transactionsLogViewModel.ToAddress, DbType.String);
            parameters.Add("@TokenSymbol", transactionsLogViewModel.TokenSymbol, DbType.String);

            using (var connection = _dapperContext.createConnection())
            {
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ResponseViewModel>(
                        procedureName, parameters, commandType: CommandType.StoredProcedure);

                    if (result == null)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = "Failed to insert transaction - no response from SP",
                            data = null
                        };
                    }

                    // SP returns statusCode = 1 for success, 0 for duplicate
                    if (result.statusCode == 1)
                    {
                        result.statusCode = (int)HttpStatusCode.OK;  // 200
                        result.message = result.message ?? "Log Inserted successfully";
                    }
                    else if (result.statusCode == 0)
                    {
                        result.statusCode = (int)HttpStatusCode.Conflict;  // 409
                        result.message = result.message ?? "Transaction already exists";
                    }
                    else
                    {
                        result.statusCode = (int)HttpStatusCode.BadRequest;
                        result.message = result.message ?? "Unknown error";
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError,
                        message = $"Database error: {ex.Message}",
                        data = null
                    };
                }
            }
        }
    }
}
