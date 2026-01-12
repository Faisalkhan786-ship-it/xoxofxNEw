
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QRCoder;
using System;
using System;
using System;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO;
using System.IO;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ViewModel;

namespace Common
{
    public class EncryptDecrypt
    {
        private string pvtKeyUSDT = "0xf58236467310aa5be46e6a3e79ba1e99b29ed37e9cbd184df43d0978c21fd53c"; //ye change karte hai 
        private string pvtKeyBNB = "0xf58236467310aa5be46e6a3e79ba1e99b29ed37e9cbd184df43d0978c21fd53c"; //same upar neche 
        private string pvtKeyECLAT = "";
        private string DepositAddress = "0x162757Ddea75bD3Aa41d89f354933b36CeE8AB4f"; //ye change karte hai 

        private string ContractAddressUSDT = "0x55d398326f99059fF775485246999027B3197955";
        private string ContractAddressECLAT = "0x564bb8396308Fc4f2B59e55588cA024a08159232";//sito
        //usdt
        private string ABIUSDT = @"[{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'owner','type':'address'},{'indexed':true,'internalType':'address','name':'spender','type':'address'},{'indexed':false,'internalType':'uint256','name':'value','type':'uint256'}],'name':'Approval','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'previousOwner','type':'address'},{'indexed':true,'internalType':'address','name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'from','type':'address'},{'indexed':true,'internalType':'address','name':'to','type':'address'},{'indexed':false,'internalType':'uint256','name':'value','type':'uint256'}],'name':'Transfer','type':'event'},{'constant':true,'inputs':[],'name':'_decimals','outputs':[{'internalType':'uint8','name':'','type':'uint8'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'_name','outputs':[{'internalType':'string','name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'_symbol','outputs':[{'internalType':'string','name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[{'internalType':'address','name':'owner','type':'address'},{'internalType':'address','name':'spender','type':'address'}],'name':'allowance','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'approve','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'internalType':'address','name':'account','type':'address'}],'name':'balanceOf','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'burn','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'decimals','outputs':[{'internalType':'uint8','name':'','type':'uint8'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'subtractedValue','type':'uint256'}],'name':'decreaseAllowance','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'getOwner','outputs':[{'internalType':'address','name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'addedValue','type':'uint256'}],'name':'increaseAllowance','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'mint','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'name','outputs':[{'internalType':'string','name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'internalType':'address','name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'renounceOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'symbol','outputs':[{'internalType':'string','name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'totalSupply','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'recipient','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'transfer','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'sender','type':'address'},{'internalType':'address','name':'recipient','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'transferFrom','outputs':[{'internalType':'bool','name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'internalType':'address','name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'}]";
        //sito
        private string ABIECLAT = @"[{'inputs':[{'internalType':'uint256','name':'initialSupply','type':'uint256'}],'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'owner','type':'address'},{'indexed':true,'internalType':'address','name':'spender','type':'address'},{'indexed':false,'internalType':'uint256','name':'value','type':'uint256'}],'name':'Approval','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'previousOwner','type':'address'},{'indexed':true,'internalType':'address','name':'newOwner','type':'address'}],'name':'OwnershipTransferred','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'internalType':'address','name':'from','type':'address'},{'indexed':true,'internalType':'address','name':'to','type':'address'},{'indexed':false,'internalType':'uint256','name':'value','type':'uint256'}],'name':'Transfer','type':'event'},{'inputs':[{'internalType':'address','name':'owner','type':'address'},{'internalType':'address','name':'spender','type':'address'}],'name':'allowance','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'approve','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'account','type':'address'}],'name':'balanceOf','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'burn','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'decimals','outputs':[{'internalType':'uint8','name':'','type':'uint8'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'subtractedValue','type':'uint256'}],'name':'decreaseAllowance','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'spender','type':'address'},{'internalType':'uint256','name':'addedValue','type':'uint256'}],'name':'increaseAllowance','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'name','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'owner','outputs':[{'internalType':'address','name':'','type':'address'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'renounceOwnership','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[],'name':'symbol','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'totalSupply','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'address','name':'to','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'transfer','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'from','type':'address'},{'internalType':'address','name':'to','type':'address'},{'internalType':'uint256','name':'amount','type':'uint256'}],'name':'transferFrom','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'newOwner','type':'address'}],'name':'transferOwnership','outputs':[],'stateMutability':'nonpayable','type':'function'}]"; //sito


        public async Task<string> GetUSDTBalance(string walletAddress, string privateKey)
        {
            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, "https://bsc-dataseed.binance.org/");
                var contract = web3.Eth.GetContract(ABIUSDT, ContractAddressUSDT);
                var getBalance = contract.GetFunction("balanceOf");
                var balance = await getBalance.CallAsync<BigInteger>(walletAddress);
                return Web3.Convert.FromWeiToBigDecimal(balance).ToString();
            }
            catch
            {
                return "0";
            }
        }
        public async Task<string> GetSITOBalance(string walletAddress, string privateKey)
        {
            try
            {
                var account = new Account(privateKey);
                var web3 = new Web3(account, "https://bsc-dataseed.binance.org/");
                var contract = web3.Eth.GetContract(ABIECLAT, ContractAddressECLAT);
                var getBalance = contract.GetFunction("balanceOf");
                var balance = await getBalance.CallAsync<BigInteger>(walletAddress);
                return Web3.Convert.FromWeiToBigDecimal(balance).ToString();
            }
            catch
            {
                return "0";
            }
        }

        public async Task<string> GetBNBBalance(string walletAddress)
        {
            try
            {
                //var web3 = new Web3("https://bsc-dataseed1.binance.org");
                var web3 = new Web3("https://bsc-dataseed1.binance.org");
                var balance = await web3.Eth.GetBalance.SendRequestAsync(walletAddress);
                return Web3.Convert.FromWei(balance.Value).ToString();
            }
            catch
            {
                return "0";
            }
        }


        public async Task<int> TransferBNBToAWallet(string walletAddress, string amount)
        {
            try
            {
                var account = new Account(pvtKeyBNB);
                var web3 = new Web3(account, "https://bsc-dataseed.binance.org/");
                web3.TransactionManager.UseLegacyAsDefault = true;

                // Convert decimal amount to Wei
                var valueInBNB = Convert.ToDecimal(amount);
                var valueInWei = Web3.Convert.ToWei(valueInBNB);

                // Send transfer
                var txnReceipt = await web3.Eth.GetEtherTransferService()
                                    .TransferEtherAndWaitForReceiptAsync(walletAddress, valueInBNB);

                return txnReceipt != null && !string.IsNullOrEmpty(txnReceipt.TransactionHash) ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<string> TransferUSDT(string PkeyUSSDT, string amountoftoken)
        {

            string transHash = "";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                string Contractaddress = ContractAddressUSDT;
                string ABI = ABIUSDT;
                var privateKey = "";

                privateKey = PkeyUSSDT;


                var account = new Account(privateKey);

                var fromAccount = account.Address;

                var web3 = new Web3(account, "https://bsc-dataseed.binance.org/");

                web3.TransactionManager.UseLegacyAsDefault = true;

                Contract smartContract = web3.Eth.GetContract(ABI, Contractaddress);

                string toAddress = DepositAddress;
                string amountOfToken = amountoftoken;
                var wei = Web3.Convert.ToWei(amountOfToken);

                object[] parameters = new object[2] { toAddress, wei };

                Function transfer = smartContract.GetFunction("transfer");

                HexBigInteger estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                transHash = transferResult.TransactionHash;
            }
            catch (Exception ex)
            {

                transHash = "";
            }

            return transHash;
        }

        public async Task<string> TransferSITO(string PkeyUSSDT, string amountoftoken)
        {

            string transHash = "";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                string Contractaddress = ContractAddressECLAT;
                string ABI = ABIECLAT;
                var privateKey = "";

                privateKey = PkeyUSSDT;


                var account = new Account(privateKey);

                var fromAccount = account.Address;

                var web3 = new Web3(account, "https://bsc-dataseed.binance.org/");

                web3.TransactionManager.UseLegacyAsDefault = true;

                Contract smartContract = web3.Eth.GetContract(ABI, Contractaddress);

                string toAddress = DepositAddress;
                string amountOfToken = amountoftoken;
                var wei = Web3.Convert.ToWei(amountOfToken);

                object[] parameters = new object[2] { toAddress, wei };

                Function transfer = smartContract.GetFunction("transfer");

                HexBigInteger estimatedGas = await transfer.EstimateGasAsync(account.Address, null, null, parameters);

                TransactionReceipt transferResult = await transfer.SendTransactionAndWaitForReceiptAsync(account.Address, estimatedGas, null, null, parameters);

                transHash = transferResult.TransactionHash;
            }
            catch (Exception ex)
            {

                transHash = "";
            }

            return transHash;
        }

        public static string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (Exception ex)
            {
                string excep = ex.Message;
                decrypted = "";
            }
            return decrypted;
        }
        public static string RandomString()
        {
            string possibleChars = "123456789abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            int num = 15;
            var result = new char[num];
            while (num-- > 0)
            {
                result[num] = possibleChars[random.Next(possibleChars.Length)];
            }
            return new string(result);
        }
    }
}
