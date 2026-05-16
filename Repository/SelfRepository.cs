using Common;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Util;
using Nethereum.Web3;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;




namespace Repository
{
    public class SelfRepository : ISelfRepository
    {
        private readonly DapperContext _dapperContext;

        public SelfRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
    
        EncryptDecrypt encryptDecrypt = new EncryptDecrypt();
       
        //generate Wallet Address and private key
        public Result2<UserWalletDetailsMasterViewModel> GenerateWalletAddress(RequestUserWalletDetailsViewModel model)
        {
            var result = new Result2<UserWalletDetailsMasterViewModel>();
            var responseModel = new ResposeUserWalletDetailsViewModel();
            var walletList = new List<UserWalletDetailsMasterViewModel>();

            try
            {
                if (model.Quantity <= 0)
                {
                    result.status = "Failed";
                    result.message = "Please enter a valid quantity.";
                    return result;
                }

                using (var conn = _dapperContext.createConnection())
                {
                    conn.Open();

                    walletList = conn.Query<UserWalletDetailsMasterViewModel>(
                        Constant.getUserWalletAddressListForAdmin,
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    for (int i = 0; i < model.Quantity; i++)
                    {
                        var wallet = GenerateWalletKey.CreateAccount();

                        if (wallet != null && !string.IsNullOrWhiteSpace(wallet.WalletAddress))
                        {
                            var param = new DynamicParameters();
                            param.Add("@WalletAddress", wallet.WalletAddress);
                            param.Add("@PrivateKey", GenerateWalletKey.Encrypt(wallet.PrivateKey));

                            int newID = conn.QuerySingleOrDefault<int>(
                                Constant.addWalletAddress,
                                param,
                                commandType: CommandType.StoredProcedure
                            );

                            if (newID != 1)
                            {
                                result.status = "Failed";
                                result.message = "Server error during wallet creation.";
                                return result;
                            }
                        }
                    }
                    int unusedCount = walletList.Count(m => m.IsActive != "1");
                    responseModel.UnUsedAddressCount = unusedCount.ToString();
                    result.status = "Succeed";
                    result.message = $"{model.Quantity} wallet address(es) created successfully.";
                    result.data = responseModel;
                }
            }
            catch (Exception ex)
            {
                result.status = "Error";
                result.message = "Exception occurred: " + ex.Message;
            }

            return result;
        }

        //Check USDT balance on Waller Address
        public async Task<Result2<AddFundModel>> USDTBalanceAsync(RequestWalletAddressModel model)
        {
            Result2<AddFundModel> result = new Result2<AddFundModel>();
            AddFundModel modelObj = new AddFundModel();

            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(model.URID)))
                {
                    result.status = "failed";
                    result.message = "WalletAddress is required.";
                    return result;
                }

                string? privateKey = "0";
                Guid WalletAddress = model.URID;

                using (var conn = _dapperContext.createConnection())
                {
                    // Step 1: Get URID by WalletAddress
                    var uridParams = new DynamicParameters();
                    uridParams.Add("@URID", model.URID);
                    Guid? URID = await conn.ExecuteScalarAsync<Guid?>("SpGetUserWalletDetails", uridParams, commandType: CommandType.StoredProcedure);

                    if (URID == null || URID == Guid.Empty)
                    {
                        result.status = "failed";
                        result.message = "You are not authorized. Please authorize first.";
                        return result;
                    }

                    // Step 2: Get Wallet Details by URID (Pass Guid directly, not Int64)
                    var detailsParams = new DynamicParameters();
                    detailsParams.Add("@URID", URID);  // FIXED here

                    var walletDetails = await conn.QueryAsync("SpGetUserWalletDetails", detailsParams, commandType: CommandType.StoredProcedure);

                    foreach (var row in walletDetails)
                    {
                        modelObj.WalletAddress = row.WalletAddress;
                        privateKey = GenerateWalletKey.Decrypt(row.PrivateKey);
                    }

                    // Step 3: Get USDT Balance
                    string? usdtBalance = await encryptDecrypt.GetUSDTBalance(modelObj.WalletAddress, privateKey);
                    modelObj.USDTBalance = usdtBalance;

                    // Step 4: Check USDT Balance and BNB top-up
                    if (Convert.ToDouble(modelObj.USDTBalance) > 4)
                    {
                        string userBNBBalance = await encryptDecrypt.GetBNBBalance(modelObj.WalletAddress);

                        if (Convert.ToDouble(userBNBBalance) < 0.0012)
                        {
                            await encryptDecrypt.TransferBNBToAWallet(modelObj.WalletAddress, "0.0014");
                            await Task.Delay(2000); // avoid using Thread.Sleep
                        }
                    }

                    result.status = "succeed";
                    result.message = "User wallet details";
                    result.data = modelObj;
                }
            }
            catch (Exception ex)
            {
                result.status = "failed";
                result.message = "Exception in method: " + ex.Message;
            }

            return result;
        }


        //BNB Withdrawal Reqeuest 
        //public async Task<Result2<resposeAddFundModel>> SendUSDTDepositRequest(RequestDepositusdtModel model)
        //{
        //    GetWalletBalance resModel = new GetWalletBalance();
        //    Result2<resposeAddFundModel> result = new Result2<resposeAddFundModel>();
        //    resposeAddFundModel modelObj = new resposeAddFundModel();

        //    using (var conn = _dapperContext.createConnection())
        //    {
        //        string sql = "SpGetUserWalletDetails";
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@URID", model.URID);

        //        Guid? URID = await conn.ExecuteScalarAsync<Guid?>(sql, parameters, commandType: CommandType.StoredProcedure);

        //        if (URID.HasValue)
        //        {
        //            string WalletAddress = "";
        //            string privateKey = "";

        //            var detailsParams = new DynamicParameters();
        //            detailsParams.Add("@URID", URID.Value);

        //            var userDetails = await conn.QueryAsync("SpGetUserWalletDetails", detailsParams, commandType: CommandType.StoredProcedure);

        //            foreach (var row in userDetails)
        //            {
        //                WalletAddress = row.WalletAddress;
        //                privateKey = GenerateWalletKey.Decrypt(row.PrivateKey);
        //            }

        //            resModel.USDTBalance = await encryptDecrypt.GetUSDTBalance(WalletAddress, privateKey);

        //            if (Convert.ToDouble(resModel.USDTBalance) > 0)
        //            {
        //                string userBNBBalance = await encryptDecrypt.GetBNBBalance(WalletAddress);

        //                //if (Convert.ToDouble(userBNBBalance) < 0.00012)
        //                //{
        //                //    await encryptDecrypt.TransferBNBToAWallet(WalletAddress, "0.00009");
        //                //    await Task.Delay(2000);
        //                //}

        //                if (Convert.ToDouble(userBNBBalance) <= 0.00012)
        //                {
        //                    await encryptDecrypt.TransferBNBToAWallet(WalletAddress, "0.00009");
        //                    await Task.Delay(2000);
        //                }


        //                string trx = await encryptDecrypt.TransferUSDT(privateKey, resModel.USDTBalance.ToString());

        //                if (string.IsNullOrEmpty(trx) || trx.Length < 20)
        //                {
        //                    result.status = "failed";
        //                    result.message = "Server busy! Transfer Rentelligence fees, please try again.";
        //                }
        //                else
        //                {
        //                    var param = new DynamicParameters();
        //                    param.Add("@URID", URID.Value);
        //                    param.Add("@Usdtvalue", resModel.USDTBalance);
        //                    param.Add("@Walletaddress", WalletAddress);
        //                    param.Add("@TranshHash", trx);
        //                    param.Add("@TokenType", 2);

        //                    int res = await conn.QuerySingleAsync<int>("SpAddTempFundDepositRecords", param, commandType: CommandType.StoredProcedure);

        //                    modelObj.Transhas = trx;
        //                    modelObj.DepositUSDT = resModel.USDTBalance;

        //                    result.status = "Succeed";
        //                    result.message = "Payment detected. We are waiting for blockchain network approval. Your request will be processed shortly.";
        //                    result.data = modelObj;
        //                }
        //            }
        //            else
        //            {
        //                result.status = "403";
        //                result.message = "Sorry, the minimum required deposit is $10. Please adjust your amount to continue.";
        //            }
        //        }
        //        else
        //        {
        //            result.status = "failed";
        //            result.message = "You are not authorized, please try again.";
        //        }
        //    }

        //    return result;
        //}


        public async Task<Result2<resposeAddFundModel>> SendUSDTDepositRequest(RequestDepositusdtModel model)
        {
            GetWalletBalance resModel = new GetWalletBalance();
            Result2<resposeAddFundModel> result = new Result2<resposeAddFundModel>();
            resposeAddFundModel modelObj = new resposeAddFundModel();

            using (var conn = _dapperContext.createConnection())
            {
                string sql = "SpGetUserWalletDetails";
                var parameters = new DynamicParameters();
                parameters.Add("@URID", model.URID);

                Guid? URID = await conn.ExecuteScalarAsync<Guid?>(sql, parameters, commandType: CommandType.StoredProcedure);

                if (URID.HasValue)
                {
                    string WalletAddress = "";
                    string privateKey = "";

                    var detailsParams = new DynamicParameters();
                    detailsParams.Add("@URID", URID.Value);

                    var userDetails = await conn.QueryAsync("SpGetUserWalletDetails", detailsParams, commandType: CommandType.StoredProcedure
                    );

                    foreach (var row in userDetails)
                    {
                        WalletAddress = row.WalletAddress;
                        privateKey = GenerateWalletKey.Decrypt(row.PrivateKey);
                    }

                    resModel.USDTBalance = await encryptDecrypt.GetUSDTBalance(WalletAddress, privateKey);

                    if (Convert.ToDouble(resModel.USDTBalance) > 0.01)
                    {
                        string userBNBBalance = await encryptDecrypt.GetBNBBalance(WalletAddress);

                        if (Convert.ToDouble(userBNBBalance) <= 0.00007)
                        {
                            await encryptDecrypt.TransferBNBToAWallet(WalletAddress, "0.00009");
                            await Task.Delay(2000);
                        }

                        string trx = await encryptDecrypt.TransferUSDT(
                            privateKey,
                            resModel.USDTBalance.ToString()
                        );

                        if (string.IsNullOrEmpty(trx) || trx.Length < 20)
                        {
                            result.status = "failed";
                            result.message = "Server busy! Transfer Rentelligence fees, please try again.";
                        }
                        else
                        {
                            //  ONLY DECIMAL FIX START
                            decimal usdtValue = Convert.ToDecimal(resModel.USDTBalance);


                            var param = new DynamicParameters();
                            param.Add("@URID", URID.Value);
                            param.Add("@Usdtvalue", usdtValue, DbType.Decimal);
                            param.Add("@Walletaddress", WalletAddress);
                            param.Add("@TranshHash", trx);
                            param.Add("@TokenType", 2);

                            await conn.ExecuteAsync(
                                "SpAddTempFundDepositRecords",
                                param,
                                commandType: CommandType.StoredProcedure
                            );

                            modelObj.Transhas = trx;

                            result.status = "Succeed";
                            result.message = "Payment detected. We are waiting for blockchain network approval. Your request will be processed shortly.";
                            result.data = modelObj;
                        }
                    }
                    else
                    {
                        result.status = "403";
                        result.message = "Sorry, the minimum required deposit is $10. Please adjust your amount to continue.";
                    }
                }
                else
                {
                    result.status = "failed";
                    result.message = "You are not authorized, please try again.";
                }
            }

            return result;
        }

        //public async Task<Result2<resposeAddFundModel>> SendUSDTDepositRequest(RequestDepositusdtModel model)
        //{
        //    GetWalletBalance resModel = new GetWalletBalance();
        //    Result2<resposeAddFundModel> result = new Result2<resposeAddFundModel>();
        //    resposeAddFundModel modelObj = new resposeAddFundModel();

        //    using (var conn = _dapperContext.createConnection())
        //    {
        //        Guid? URID = await conn.ExecuteScalarAsync<Guid?>(
        //            "SpGetUserWalletDetails",
        //            new { URID = model.URID },
        //            commandType: CommandType.StoredProcedure);

        //        if (!URID.HasValue)
        //        {
        //            result.status = "failed";
        //            result.message = "You are not authorized, please try again.";
        //            return result;
        //        }

        //        // Fetch wallet + private key
        //        string WalletAddress = "";
        //        string privateKey = "";

        //        var userDetails = await conn.QueryAsync(
        //            "SpGetUserWalletDetails",
        //            new { URID = URID.Value },
        //            commandType: CommandType.StoredProcedure);

        //        foreach (var row in userDetails)
        //        {
        //            WalletAddress = row.WalletAddress;
        //            privateKey = GenerateWalletKey.Decrypt(row.PrivateKey);
        //        }

        //        // Get USDT balance
        //        resModel.USDTBalance = await encryptDecrypt.GetUSDTBalance(WalletAddress, privateKey);

        //        if (Convert.ToDouble(resModel.USDTBalance) <= 0)
        //        {
        //            result.status = "403";
        //            result.message = "Sorry, the minimum required deposit is $10. Please adjust your amount to continue.";
        //            return result;
        //        }

        //        // Check BNB for gas fee
        //        string userBNBBalance = await encryptDecrypt.GetBNBBalance(WalletAddress);

        //        if (Convert.ToDouble(userBNBBalance) < 0.00012)
        //        {
        //            // Fixed working version (exact amount transfer)
        //            await encryptDecrypt.TransferBNBToAWallet(WalletAddress, "0.00009");
        //            await Task.Delay(2000);
        //        }

        //        // Send USDT
        //        string trx = await encryptDecrypt.TransferUSDT(privateKey, resModel.USDTBalance.ToString());

        //        if (string.IsNullOrEmpty(trx) || trx.Length < 20)
        //        {
        //            result.status = "failed";
        //            result.message = "Server busy! Transfer Rentelligence fees, please try again.";
        //            return result;
        //        }

        //        // Save to DB
        //        var param = new DynamicParameters();
        //        param.Add("@URID", URID.Value);
        //        param.Add("@Usdtvalue", resModel.USDTBalance);
        //        param.Add("@Walletaddress", WalletAddress);
        //        param.Add("@TranshHash", trx);
        //        param.Add("@TokenType", 2);

        //        int res = await conn.QuerySingleAsync<int>(
        //            "SpAddTempFundDepositRecords",
        //            param,
        //            commandType: CommandType.StoredProcedure);

        //        modelObj.Transhas = trx;
        //        modelObj.DepositUSDT = resModel.USDTBalance;

        //        result.status = "Succeed";
        //        result.message = "Payment detected. We are waiting for blockchain network approval. Your request will be processed shortly.";
        //        result.data = modelObj;
        //    }

        //    return result;
        //}


        //get All Wallet Address (Admin site)
        public async Task<ResponseViewModel> getAllWalletAddress()
        {
            var procedureName = Constant.getUserWalletAddressListForAdmin;
            var parameters = new DynamicParameters();

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<WalletInfoModel>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                ResponseViewModel returnData = new ResponseViewModel();

                if (result != null && result.Any())
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Success",
                        data = result
                    };
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.OK,
                        message = "Data Not Found",
                        data = new List<WalletInfoModel>()
                    };
                }
                return returnData;
            }
        }

        //get All Wallet Address By URID
        public async Task<ResponseViewModel> getAllWalletAddressByURID(Guid URID)
        {
            var procedureName = Constant.getWalletAddresByURID;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<WalletInfoModelBYURID>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                ResponseViewModel returnData;

                if (result != null && result.Any())
                {
                    var validation = result.First();
                    if (validation.statuscode == 1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = validation.message,
                            data = result
                        };
                    }
                    else if (validation.statuscode == 0)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else if (validation.statuscode == -1)
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.Conflict,
                            message = validation.message
                        };
                    }
                    else
                    {
                        returnData = new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.BadRequest,
                            message = validation.message
                        };
                    }
                }
                else
                {
                    returnData = new ResponseViewModel
                    {
                        statusCode = (int)HttpStatusCode.NotFound,
                        message = "Something went to wrong with server error."
                    };
                }

                return returnData;
            }
        }

        //get USDT details lis By URID
        public async Task<ResponseViewModel> GetSelfDepsiteByURID(Guid URID)
        {
            var procedureName = Constant.getSelfDepsiteByURID;
            var parameters = new DynamicParameters();
            parameters.Add("@URID", URID);

            using (var connection = _dapperContext.createConnection())
            {
                var result = await connection.QueryAsync<WalletInfoModelDetailsBYURID>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null && result.Any())
                {
                    var firstRecord = result.First();

                    if (firstRecord.statuscode == 1)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.OK,
                            message = firstRecord.message,
                            data = result
                        };
                    }
                    else if (firstRecord.statuscode == 0)
                    {
                        return new ResponseViewModel
                        {
                            statusCode = (int)HttpStatusCode.NotFound,
                            message = "Data not found"
                        };
                    }
                }

                return new ResponseViewModel
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    message = "Data not found"
                };
            }
        }
        
        public class WalletInfoModelBYURID
        {
            public int statuscode { get; set; }
            public string? message { get; set; }
            public string? WalletAddress { get; set; }
        }
           
        public class WalletInfoModel
        {
            public int messagecode { get; set; }
            public string? message { get; set; }
            public int Id { get; set; }
            public Guid URID { get; set; }
            public string? WalletAddress { get; set; }
            public string? PrivateKey { get; set; }
            public string? CreatedDate { get; set; }
            public bool IsActive { get; set; }
            public string? Status { get; set; }
            public string? Authlogin { get; set; }
            public string? Name { get; set; }
        }
        public class WalletResponseModel
        {
            public string? WalletAddress { get; set; }
            public string? ELTButtonst { get; set; }
            public string? USDTButtonst { get; set; }
            public string? ELTBalance { get; set; }
            public string? USDTBalance { get; set; }
            public string? QrCodeUri { get; set; }
        }

        public class GetWalletBalance
        {
            public string? ELTBalance { get; set; }
            public string? USDTBalance { get; set; }
        }

        public class WalletInfoModelDetailsBYURID
        {
            public int statuscode { get; set; }
            public string? message { get; set; }
            public string? WalletAddress { get; set; }
            public decimal? USDAmount { get; set; }
            public string? TransHash { get; set; }
            public string? CreadtedDate { get; set; }
            public string? Status { get; set; }
        }
    }
}

