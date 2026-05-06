

//using Microsoft.AspNetCore.Mvc;
//using Nethereum.Hex.HexTypes;
//using Nethereum.RPC.Eth.DTOs;
//using Nethereum.Util;
//using Nethereum.Web3;
//using Newtonsoft.Json;
//using RepositoryContract;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Numerics;
//using System.Threading.Tasks;
//using ViewModel;

//namespace Arbion_Apis.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class EthereumController : ControllerBase
//    {
//        private readonly ITransactionsLogRepository _transactionsLogRepository;
//        private readonly HttpClient _httpClient;
//        private Web3 _web3;

//        // ✅ SINGLE CONSTRUCTOR (Fixed)
//        public EthereumController(ITransactionsLogRepository transactionsLogRepository)
//        {
//            _transactionsLogRepository = transactionsLogRepository;
//            _httpClient = new HttpClient();
//            _web3 = new Web3(CONFIG.RpcUrl);
//        }

//        // ================= CONFIG =================
//        private static class CONFIG
//        {
//            public static string RpcUrl = "https://eth-mainnet.public.blastapi.io";
//            public static string Contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7"; // USDT Contract
//            public static int Decimals = 6;
//            public static string OutputFile = Path.Combine(Directory.GetCurrentDirectory(), "ethereum_transactions.json");
//            public static string LastBlockFile = Path.Combine(Directory.GetCurrentDirectory(), "ethereum_last_block.txt");
//            public static int MaxRecords = 10000;
//            public static int BlocksToScan = 20000;
//        }

//        // ================= MODEL =================
//        public class EthereumTxRecord
//        {
//            public string chain { get; set; }
//            public string datetime { get; set; }
//            public string txHash { get; set; }
//            public string fromAddress { get; set; }
//            public string toAddress { get; set; }
//            public string amount { get; set; }
//            public decimal rawAmount { get; set; }
//            public string tokenSymbol { get; set; }
//            public string explorerUrl { get; set; }
//        }

//        // ================= JSON STORAGE =================
//        private List<EthereumTxRecord> LoadStore()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.OutputFile))
//                {
//                    Console.WriteLine("⚠️ Ethereum file exist nahi karti");
//                    return new List<EthereumTxRecord>();
//                }

//                var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
//                if (string.IsNullOrWhiteSpace(json)) return new List<EthereumTxRecord>();

//                var records = JsonConvert.DeserializeObject<List<EthereumTxRecord>>(json);
//                Console.WriteLine($"✅ Ethereum: {records?.Count ?? 0} records load hue");
//                return records ?? new List<EthereumTxRecord>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Ethereum Load error: {ex.Message}");
//                return new List<EthereumTxRecord>();
//            }
//        }

//        private void SaveStore(List<EthereumTxRecord> records)
//        {
//            var trimmed = records
//                .GroupBy(x => x.txHash)
//                .Select(g => g.First())
//                .Take(CONFIG.MaxRecords)
//                .ToList();

//            var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
//            System.IO.File.WriteAllText(CONFIG.OutputFile, json);
//            Console.WriteLine($"✅ Ethereum: {trimmed.Count} records saved");
//        }

//        private BigInteger GetLastBlock()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.LastBlockFile))
//                    return 0;
//                return BigInteger.Parse(System.IO.File.ReadAllText(CONFIG.LastBlockFile));
//            }
//            catch
//            {
//                return 0;
//            }
//        }

//        private void SaveLastBlock(BigInteger block)
//        {
//            try
//            {
//                System.IO.File.WriteAllText(CONFIG.LastBlockFile, block.ToString());
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Save block error: {ex.Message}");
//            }
//        }

//        private string Now()
//        {
//            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//        }

//        // ================= FETCH TRANSACTIONS =================
//        private async Task<List<FilterLog>> FetchTransactionLogs(BigInteger fromBlock, BigInteger toBlock)
//        {
//            try
//            {
//                var allLogs = new List<FilterLog>();
//                int chunkSize = 500;

//                for (BigInteger i = fromBlock; i <= toBlock; i += chunkSize)
//                {
//                    var end = i + chunkSize;
//                    if (end > toBlock) end = toBlock;

//                    try
//                    {
//                        var filter = new Nethereum.RPC.Eth.DTOs.NewFilterInput
//                        {
//                            FromBlock = new BlockParameter(new HexBigInteger(i)),
//                            ToBlock = new BlockParameter(new HexBigInteger(end)),
//                            Address = new[] { CONFIG.Contract },
//                            Topics = new object[]
//                            {
//                                "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef" // Transfer event
//                            }
//                        };

//                        var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filter);
//                        allLogs.AddRange(logs);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"❌ Ethereum chunk failed {i}-{end}: {ex.Message}");
//                    }
//                }

//                return allLogs;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Ethereum fetch error: {ex.Message}");
//                return new List<FilterLog>();
//            }
//        }

//        // ================= PROCESS TRANSACTIONS =================
//        private List<EthereumTxRecord> ProcessTransactions(List<FilterLog> logs, List<EthereumTxRecord> existingRecords)
//        {
//            var existingHashes = new HashSet<string>(existingRecords.Select(r => r.txHash));
//            var newRecords = new List<EthereumTxRecord>();

//            foreach (var log in logs)
//            {
//                if (existingHashes.Contains(log.TransactionHash))
//                    continue;

//                if (log.Topics == null || log.Topics.Length < 3)
//                    continue;

//                var fromAddress = "0x" + log.Topics[1].ToString().Substring(26);
//                var toAddress = "0x" + log.Topics[2].ToString().Substring(26);

//                if (!BigInteger.TryParse(log.Data.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out var value))
//                    continue;

//                var amount = UnitConversion.Convert.FromWei(value, CONFIG.Decimals);

//                if (amount <= 0)
//                    continue;

//                var record = new EthereumTxRecord
//                {
//                    chain = "Ethereum",
//                    datetime = Now(),
//                    txHash = log.TransactionHash,
//                    fromAddress = fromAddress,
//                    toAddress = toAddress,
//                    amount = amount.ToString("N2"),
//                    rawAmount = (decimal)amount,
//                    tokenSymbol = "USDT",
//                    explorerUrl = $"https://etherscan.io/tx/{log.TransactionHash}"
//                };

//                newRecords.Add(record);
//                Console.WriteLine($"✅ New Ethereum TX: {record.txHash} | {record.amount} USDT");
//            }

//            return newRecords;
//        }

//        // ================= API 1: Scan & Save Ethereum =================
//        [HttpPost("Scan")]
//        public async Task<IActionResult> ScanEthereum()
//        {
//            try
//            {
//                Console.WriteLine($"🔄 Ethereum scan start: {DateTime.Now}");

//                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
//                Console.WriteLine($"📦 Current Ethereum Block: {currentBlock.Value}");

//                var existing = LoadStore();

//                var lastBlock = GetLastBlock();
//                var fromBlock = lastBlock == 0
//                    ? currentBlock.Value - CONFIG.BlocksToScan
//                    : lastBlock + 1;

//                if (fromBlock > currentBlock.Value)
//                {
//                    return Ok(new
//                    {
//                        status = "success",
//                        chain = "Ethereum",
//                        message = "Koi naye block nahi hai",
//                        totalRecords = existing.Count,
//                        newRecords = 0
//                    });
//                }

//                Console.WriteLine($"📊 Scanning from block {fromBlock} to {currentBlock.Value}");

//                var logs = await FetchTransactionLogs(fromBlock, currentBlock.Value);
//                var newRecords = ProcessTransactions(logs, existing);

//                Console.WriteLine($"🔥 New Ethereum Records: {newRecords.Count}");

//                if (newRecords.Count > 0)
//                {
//                    existing.InsertRange(0, newRecords);
//                    SaveStore(existing);
//                    SaveLastBlock(currentBlock.Value);
//                }

//                var last24HoursData = existing
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Ethereum",
//                    message = last24HoursData.Count > 0
//                        ? $"Ethereum: Last 24 ghante mein {last24HoursData.Count} USDT transactions"
//                        : "Ethereum: Last 24 ghante mein koi USDT transaction nahi",
//                    totalRecords = existing.Count,
//                    newRecords = newRecords.Count,
//                    last24HoursRecords = last24HoursData.Count,
//                    fromBlock = fromBlock,
//                    toBlock = currentBlock.Value,
//                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                    data = last24HoursData.Take(100)
//                });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error: {ex.Message}");
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "Ethereum",
//                    message = $"Error aaya: {ex.Message}"
//                });
//            }
//        }

//        // ================= API 2: Sirf Get Data (No Scan) with DB Save =================
//        [HttpGet("GetData")]
//        public async Task<IActionResult> GetEthereumData()
//        {
//            try
//            {
//                var existing = LoadStore();

//                if (existing == null || existing.Count == 0)
//                {
//                    return Ok(new
//                    {
//                        status = "no_data",
//                        message = "Ethereum ka koi data nahi aaya! Please pehle /Scan API call karein",
//                        chain = "Ethereum",
//                        totalRecords = 0,
//                        last24HoursRecords = 0,
//                        data = new List<EthereumTxRecord>()
//                    });
//                }

//                var last24Hours = existing
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                // ✅ SAVE TO DB - Ethereum ke liye
//                int successCount = 0;
//                int duplicateCount = 0;
//                int errorCount = 0;

//                foreach (var tx in last24Hours)
//                {
//                    try
//                    {
//                        var model = new TransactionsLogViewModel
//                        {
//                            NetworkChain = "Ethereum",
//                            TransactionHash = tx.txHash,
//                            DateTime = DateTime.Now,
//                            Amount = tx.rawAmount,
//                            FromAddress = tx.fromAddress,
//                            ToAddress = tx.toAddress,
//                            TokenSymbol = tx.tokenSymbol ?? "USDT"
//                        };

//                        var result = await _transactionsLogRepository.addTransactionsLog(model);

//                        if (result.statusCode == 200)
//                        {
//                            successCount++;
//                            Console.WriteLine($"✅ Inserted: {model.TransactionHash}");
//                        }
//                        else if (result.statusCode == 409)
//                        {
//                            duplicateCount++;
//                            Console.WriteLine($"⚠️ Duplicate: {model.TransactionHash}");
//                        }
//                        else
//                        {
//                            errorCount++;
//                            Console.WriteLine($"❌ Failed: {model.TransactionHash} - {result.message}");
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        errorCount++;
//                        Console.WriteLine($"❌ DB Error {tx.txHash}: {ex.Message}");
//                    }
//                }

//                Console.WriteLine($"📊 Ethereum Summary - Success: {successCount}, Duplicate: {duplicateCount}, Error: {errorCount}");

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Ethereum",
//                    message = last24Hours.Count > 0
//                        ? $"Ethereum: Last 24 ghante mein {last24Hours.Count} USDT transactions"
//                        : "Ethereum: Last 24 ghante mein koi USDT transaction nahi",
//                    totalRecords = existing.Count,
//                    last24HoursRecords = last24Hours.Count,
//                    dbInsertSummary = new
//                    {
//                        total = last24Hours.Count,
//                        inserted = successCount,
//                        duplicate = duplicateCount,
//                        failed = errorCount
//                    },
//                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                    data = last24Hours
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "Ethereum",
//                    message = ex.Message
//                });
//            }
//        }

//        // ================= API 3: Force Live Scan =================
//        [HttpPost("ForceScan")]
//        public async Task<IActionResult> ForceScan()
//        {
//            try
//            {
//                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
//                var lastBlock = GetLastBlock();
//                var fromBlock = lastBlock == 0 ? currentBlock.Value - 1000 : lastBlock + 1;

//                var logs = await FetchTransactionLogs(fromBlock, currentBlock.Value);
//                var existing = LoadStore();
//                var newRecords = ProcessTransactions(logs, existing);

//                if (newRecords.Count > 0)
//                {
//                    existing.InsertRange(0, newRecords);
//                    SaveStore(existing);
//                    SaveLastBlock(currentBlock.Value);
//                }

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Ethereum",
//                    message = newRecords.Count > 0
//                        ? $"Force scan complete! {newRecords.Count} new USDT transactions found"
//                        : "Force scan complete! No new USDT transactions",
//                    newRecords = newRecords.Count,
//                    totalRecords = existing.Count,
//                    data = newRecords.Take(50)
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "Ethereum",
//                    message = ex.Message
//                });
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoryContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModel;

namespace Arbion_Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EthereumController : ControllerBase
    {
        private readonly ITransactionsLogRepository _repo;
        private readonly HttpClient _http;

        public EthereumController(ITransactionsLogRepository repo)
        {
            _repo = repo;
            _http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30) // ✅ timeout fix
            };
        }

        // ================= CONFIG =================
        private static class CONFIG
        {
            //public static string ApiKey = "ND92YAICDVZ9Y1CAEM9WBDB1H5SWZ8CBHK";
            public static string ApiKey = "VV7YA58UFJQTQWE9BFTG22QTQUE6W4DSXP";
            public static string Contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7";
            public static string File = Path.Combine(Directory.GetCurrentDirectory(), "eth_data.json");
            public static int MaxRecords = 10000;
        }

        // ================= MODEL =================
        public class Tx
        {
            public string txHash { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public decimal amount { get; set; }
            public string datetime { get; set; }
        }

        // ================= FILE LOAD =================
        private List<Tx> Load()
        {
            if (!System.IO.File.Exists(CONFIG.File))
                return new List<Tx>();

            var json = System.IO.File.ReadAllText(CONFIG.File);
            return string.IsNullOrWhiteSpace(json)
                ? new List<Tx>()
                : JsonConvert.DeserializeObject<List<Tx>>(json) ?? new List<Tx>();
        }

        // ================= FILE SAVE =================
        private void Save(List<Tx> data)
        {
            var clean = data
                .GroupBy(x => x.txHash)
                .Select(x => x.First())
                .Take(CONFIG.MaxRecords)
                .ToList();

            System.IO.File.WriteAllText(CONFIG.File,
                JsonConvert.SerializeObject(clean, Formatting.Indented));
        }

        // ================= FETCH (SAFE + LIMITED) =================
        private async Task<List<Tx>> FetchUSDT()
        {
            var list = new List<Tx>();

            try
            {
                int page = 1;
                int offset = 100;
                int maxPages = 1; // ✅ LIMIT to avoid loading

                while (page <= maxPages)
                {
                    string url = $"https://api.etherscan.io/v2/api" +
                                 $"?chainid=1" +
                                 $"&module=account&action=tokentx" +
                                 $"&contractaddress={CONFIG.Contract}" +
                                 $"&page={page}&offset={offset}&sort=desc" +
                                 $"&apikey={CONFIG.ApiKey}";

                    Console.WriteLine($"📡 Fetch Page: {page}");

                    var res = await _http.GetStringAsync(url);
                    dynamic json = JsonConvert.DeserializeObject(res);

                    if (json.status != "1") break;

                    int count = 0;

                    foreach (var tx in json.result)
                    {
                        count++;

                        decimal amount = Convert.ToDecimal(tx.value.ToString()) / 1000000;
                        if (amount <= 0) continue;

                        list.Add(new Tx
                        {
                            txHash = tx.hash,
                            from = tx.from,
                            to = tx.to,
                            amount = amount,
                            datetime = DateTimeOffset
                                .FromUnixTimeSeconds((long)tx.timeStamp)
                                .LocalDateTime
                                .ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }

                    Console.WriteLine($"Page {page} Count: {count}");

                    if (count < offset) break;

                    page++;
                    await Task.Delay(300); // ✅ rate limit safe
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Fetch Error: " + ex.Message);
            }

            return list;
        }

        // ================= SCAN =================
        [HttpPost("Scan")]
        public async Task<IActionResult> Scan()
        {
            try
            {
                var existing = Load();
                var fetched = await FetchUSDT();

                Console.WriteLine($"Fetched Total: {fetched.Count}");

                // ✅ LAST HASH LOGIC (best)
                var lastHash = existing.FirstOrDefault()?.txHash;

                List<Tx> newData;

                if (!string.IsNullOrEmpty(lastHash))
                {
                    int index = fetched.FindIndex(x => x.txHash == lastHash);

                    newData = index != -1
                        ? fetched.Take(index).ToList()
                        : fetched;
                }
                else
                {
                    newData = fetched;
                }

                int inserted = 0, duplicate = 0, failed = 0;

                if (newData.Any())
                {
                    existing.InsertRange(0, newData);
                    Save(existing);

                    foreach (var tx in newData)
                    {
                        try
                        {
                            var res = await _repo.addTransactionsLog(new TransactionsLogViewModel
                            {
                                NetworkChain = "Ethereum",
                                TransactionHash = tx.txHash,
                                DateTime = DateTime.Now,
                                Amount = tx.amount,
                                FromAddress = tx.from,
                                ToAddress = tx.to,
                                TokenSymbol = "USDT"
                            });

                            if (res.statusCode == 200) inserted++;
                            else if (res.statusCode == 409) duplicate++;
                            else failed++;
                        }
                        catch
                        {
                            failed++;
                        }
                    }
                }

                return Ok(new
                {
                    status = "success",
                    fetched = fetched.Count,
                    newRecords = newData.Count,
                    db = new { inserted, duplicate, failed }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        // ================= GET =================
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var data = Load();

            return Ok(new
            {
                total = data.Count,
                data = data.Take(100)
            });
        }
    }
}