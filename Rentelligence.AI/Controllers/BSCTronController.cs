using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;

namespace Arbion_Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BSCTronController : ControllerBase
    {
        private Web3 _web3;

        // ================= BSC CONFIG =================
        private static class CONFIG
        {
            public static string RpcUrl = "https://bsc-dataseed.binance.org/";
            public static string Contract = "0x55d398326f99059ff775485246999027b3197955";
            public static int Decimals = 18;
            public static string OutputFile = Path.Combine(Directory.GetCurrentDirectory(), "bsc_transactions.json");
            public static string LastBlockFile = Path.Combine(Directory.GetCurrentDirectory(), "bsc_last_block.txt");
            public static int MaxRecords = 10000;
            public static int BlocksToScan = 20000;
        }

        public class BSCTxRecord
        {
            public string chain { get; set; }
            public string datetime { get; set; }
            public long blockNumber { get; set; }
            public string txHash { get; set; }
            public string fromAddress { get; set; }
            public string toAddress { get; set; }
            public string amount { get; set; }
            public decimal rawAmount { get; set; }
            public string tokenSymbol { get; set; }
            public string explorerUrl { get; set; }
        }

        public BSCTronController()
        {
            _web3 = new Web3(CONFIG.RpcUrl);
        }

        private List<BSCTxRecord> LoadStore()
        {
            try
            {
                if (!System.IO.File.Exists(CONFIG.OutputFile))
                    return new List<BSCTxRecord>();

                var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
                if (string.IsNullOrWhiteSpace(json)) return new List<BSCTxRecord>();

                var records = JsonConvert.DeserializeObject<List<BSCTxRecord>>(json);
                return records ?? new List<BSCTxRecord>();
            }
            catch
            {
                return new List<BSCTxRecord>();
            }
        }

        private void SaveStore(List<BSCTxRecord> records)
        {
            var trimmed = records
                .GroupBy(x => x.txHash)
                .Select(g => g.First())
                .Take(CONFIG.MaxRecords)
                .ToList();

            var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
            System.IO.File.WriteAllText(CONFIG.OutputFile, json);
        }

        private BigInteger GetLastBlock()
        {
            try
            {
                if (!System.IO.File.Exists(CONFIG.LastBlockFile))
                    return 0;
                return BigInteger.Parse(System.IO.File.ReadAllText(CONFIG.LastBlockFile));
            }
            catch
            {
                return 0;
            }
        }

        private void SaveLastBlock(BigInteger block)
        {
            try
            {
                System.IO.File.WriteAllText(CONFIG.LastBlockFile, block.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Save block error: {ex.Message}");
            }
        }

        private async Task<DateTime> GetBlockTimestamp(BigInteger blockNumber)
        {
            try
            {
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(
                    new BlockParameter(new HexBigInteger(blockNumber))
                );

                if (block != null && block.Timestamp != null)
                {
                    var timestamp = (long)block.Timestamp.Value;
                    return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to get block timestamp: {ex.Message}");
            }
            return DateTime.Now;
        }

        private async Task<List<FilterLog>> FetchTransactionLogs(BigInteger fromBlock, BigInteger toBlock)
        {
            try
            {
                var allLogs = new List<FilterLog>();
                int chunkSize = 500;

                for (BigInteger i = fromBlock; i <= toBlock; i += chunkSize)
                {
                    var end = i + chunkSize;
                    if (end > toBlock) end = toBlock;

                    try
                    {
                        var filter = new NewFilterInput
                        {
                            FromBlock = new BlockParameter(new HexBigInteger(i)),
                            ToBlock = new BlockParameter(new HexBigInteger(end)),
                            Address = new[] { CONFIG.Contract },
                            Topics = new object[]
                            {
                                "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef"
                            }
                        };

                        var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filter);
                        allLogs.AddRange(logs);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ BSC chunk failed {i}-{end}: {ex.Message}");
                    }
                }

                return allLogs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ BSC fetch error: {ex.Message}");
                return new List<FilterLog>();
            }
        }

        private async Task<List<BSCTxRecord>> ProcessTransactions(List<FilterLog> logs, List<BSCTxRecord> existingRecords)
        {
            var existingHashes = new HashSet<string>(existingRecords.Select(r => r.txHash));
            var newRecordsList = new List<BSCTxRecord>();

            foreach (var log in logs)
            {
                if (existingHashes.Contains(log.TransactionHash))
                    continue;

                if (log.Topics == null || log.Topics.Length < 3)
                    continue;

                // Fixed CS8602 - Added null check
                if (log.Topics[1] == null || log.Topics[2] == null)
                    continue;

                var fromAddress = "0x" + log.Topics[1].ToString().Substring(26);
                var toAddress = "0x" + log.Topics[2].ToString().Substring(26);

                if (string.IsNullOrEmpty(log.Data))
                    continue;

                if (!BigInteger.TryParse(log.Data.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out var value))
                    continue;

                var amount = UnitConversion.Convert.FromWei(value, CONFIG.Decimals);

                if (amount <= 0)
                    continue;

                var blockNumber = (BigInteger)log.BlockNumber;
                var blockTimestamp = await GetBlockTimestamp(blockNumber);

                var record = new BSCTxRecord
                {
                    chain = "BSC",
                    datetime = blockTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    blockNumber = (long)blockNumber,
                    txHash = log.TransactionHash,
                    fromAddress = fromAddress,
                    toAddress = toAddress,
                    amount = amount.ToString("N2"),
                    rawAmount = (decimal)amount,
                    tokenSymbol = "USDT",
                    explorerUrl = $"https://bscscan.com/tx/{log.TransactionHash}"
                };

                newRecordsList.Add(record);
                Console.WriteLine($"✅ New BSC TX: {record.txHash} | {record.amount} USDT");
            }

            return newRecordsList;
        }

        // ================= API 1: SCAN =================
        [HttpPost("Scan")]
        public async Task<IActionResult> ScanBSC()
        {
            try
            {
                Console.WriteLine($"🔄 BSC scan start: {DateTime.Now}");

                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                Console.WriteLine($"📦 Current BSC Block: {currentBlock.Value}");

                var existingRecords = LoadStore();
                var lastBlock = GetLastBlock();
                var fromBlock = lastBlock == 0
                    ? currentBlock.Value - CONFIG.BlocksToScan
                    : lastBlock + 1;

                // FIXED CS0136 - Changed variable name to 'filteredData'
                if (fromBlock > currentBlock.Value)
                {
                    var filteredData = existingRecords
                        .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                        .OrderByDescending(x => x.datetime)
                        .ToList();

                    return Ok(new
                    {
                        status = "success",
                        chain = "BSC",
                        message = filteredData.Count > 0
                            ? $"BSC: Last 24 ghante mein {filteredData.Count} USDT transactions"
                            : "BSC: Last 24 ghante mein koi USDT transaction nahi",
                        totalRecords = existingRecords.Count,
                        newRecords = 0,
                        last24HoursRecords = filteredData.Count,
                        fromBlock = fromBlock,
                        toBlock = currentBlock.Value,
                        fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                        toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        data = filteredData.Take(100)
                    });
                }

                Console.WriteLine($"📊 Scanning from block {fromBlock} to {currentBlock.Value}");

                var logs = await FetchTransactionLogs(fromBlock, currentBlock.Value);
                var newRecords = await ProcessTransactions(logs, existingRecords);

                Console.WriteLine($"🔥 New BSC Records: {newRecords.Count}");

                if (newRecords.Count > 0)
                {
                    existingRecords.InsertRange(0, newRecords);
                    SaveStore(existingRecords);
                    SaveLastBlock(currentBlock.Value);
                }

                var allRecordsSorted = existingRecords.OrderByDescending(x => x.datetime).ToList();

                // FIXED CS0136 - Changed variable name to 'last24HoursData'
                var last24HoursData = allRecordsSorted
                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                    .OrderByDescending(x => x.datetime)
                    .ToList();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = last24HoursData.Count > 0
                        ? $"BSC: Last 24 ghante mein {last24HoursData.Count} USDT transactions"
                        : "BSC: Last 24 ghante mein koi USDT transaction nahi",
                    totalRecords = allRecordsSorted.Count,
                    newRecords = newRecords.Count,
                    last24HoursRecords = last24HoursData.Count,
                    fromBlock = fromBlock,
                    toBlock = currentBlock.Value,
                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = last24HoursData.Take(100)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return StatusCode(500, new
                {
                    status = "error",
                    chain = "BSC",
                    message = $"Error aaya: {ex.Message}"
                });
            }
        }

        // ================= API 2: GET DATA =================
        [HttpGet("GetData")]
        public IActionResult GetBSCData()
        {
            try
            {
                var existingRecords = LoadStore();

                if (existingRecords == null || existingRecords.Count == 0)
                {
                    return Ok(new
                    {
                        status = "no_data",
                        message = "BSC ka koi data nahi aaye! Please pehle /Scan API call karein",
                        chain = "BSC",
                        totalRecords = 0,
                        last24HoursRecords = 0,
                        data = new List<BSCTxRecord>()
                    });
                }

                var sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

                var last24HoursData = sortedRecords
                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                    .OrderByDescending(x => x.datetime)
                    .ToList();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = last24HoursData.Count > 0
                        ? $"BSC: Last 24 ghante mein {last24HoursData.Count} USDT transactions"
                        : "BSC: Last 24 ghante mein koi USDT transaction nahi",
                    totalRecords = sortedRecords.Count,
                    last24HoursRecords = last24HoursData.Count,
                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = last24HoursData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    chain = "BSC",
                    message = ex.Message
                });
            }
        }

        // ================= API 3: FORCE SCAN =================
        [HttpPost("ForceScan")]
        public async Task<IActionResult> ForceScan()
        {
            try
            {
                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                var lastBlock = GetLastBlock();
                var fromBlock = lastBlock == 0 ? currentBlock.Value - 1000 : lastBlock + 1;

                var logs = await FetchTransactionLogs(fromBlock, currentBlock.Value);
                var existingRecords = LoadStore();
                var newRecords = await ProcessTransactions(logs, existingRecords);

                if (newRecords.Count > 0)
                {
                    existingRecords.InsertRange(0, newRecords);
                    SaveStore(existingRecords);
                    SaveLastBlock(currentBlock.Value);
                }

                var sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

                var last24HoursCount = sortedRecords
                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                    .Count();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = newRecords.Count > 0
                        ? $"Force scan complete! {newRecords.Count} new USDT transactions found"
                        : "Force scan complete! No new USDT transactions",
                    newRecords = newRecords.Count,
                    totalRecords = sortedRecords.Count,
                    last24HoursRecords = last24HoursCount,
                    data = newRecords.Take(50)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    chain = "BSC",
                    message = ex.Message
                });
            }
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Numerics;
//using System.Threading.Tasks;
//using Nethereum.Web3;
//using Nethereum.RPC.Eth.DTOs;
//using Nethereum.Hex.HexTypes;

//namespace Arbion_Apis.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class BSCTronController : ControllerBase
//    {
//        private Web3 _web3;

//        private static class CONFIG
//        {
//            public static string RpcUrl = "https://bsc-dataseed.binance.org/";
//            public static string Contract = "0x55d398326f99059ff775485246999027b3197955";
//            public static int Decimals = 18;
//            public static string OutputFile = Path.Combine(Directory.GetCurrentDirectory(), "bsc_transactions.json");
//            public static string LastBlockFile = Path.Combine(Directory.GetCurrentDirectory(), "bsc_last_block.txt");
//            public static int MaxRecords = 10000;
//            public static int BlocksToScan = 20000;
//        }

//        public class BSCTxRecord
//        {
//            public string chain { get; set; }
//            public string datetime { get; set; }
//            public long blockNumber { get; set; }
//            public string txHash { get; set; }
//            public string fromAddress { get; set; }
//            public string toAddress { get; set; }
//            public string amount { get; set; }
//            public decimal rawAmount { get; set; }
//            public string tokenSymbol { get; set; }
//            public string explorerUrl { get; set; }
//        }

//        public BSCTronController()
//        {
//            _web3 = new Web3(CONFIG.RpcUrl);
//        }

//        private List<BSCTxRecord> LoadStore()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.OutputFile))
//                    return new List<BSCTxRecord>();

//                var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
//                if (string.IsNullOrWhiteSpace(json)) return new List<BSCTxRecord>();

//                var records = JsonConvert.DeserializeObject<List<BSCTxRecord>>(json);
//                return records ?? new List<BSCTxRecord>();
//            }
//            catch
//            {
//                return new List<BSCTxRecord>();
//            }
//        }

//        private void SaveStore(List<BSCTxRecord> records)
//        {
//            var trimmed = records
//                .GroupBy(x => x.txHash)
//                .Select(g => g.First())
//                .Take(CONFIG.MaxRecords)
//                .ToList();

//            var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
//            System.IO.File.WriteAllText(CONFIG.OutputFile, json);
//        }

//        private long GetLastBlock()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.LastBlockFile))
//                    return 0;
//                return long.Parse(System.IO.File.ReadAllText(CONFIG.LastBlockFile));
//            }
//            catch
//            {
//                return 0;
//            }
//        }

//        private void SaveLastBlock(long block)
//        {
//            try
//            {
//                System.IO.File.WriteAllText(CONFIG.LastBlockFile, block.ToString());
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Save block error: {ex.Message}");
//            }
//        }

//        private async Task<DateTime> GetBlockTimestamp(long blockNumber)
//        {
//            try
//            {
//                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(
//                    new BlockParameter(new HexBigInteger(blockNumber))
//                );

//                if (block != null && block.Timestamp != null)
//                {
//                    var timestamp = (long)block.Timestamp.Value;
//                    return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Failed to get block timestamp: {ex.Message}");
//            }
//            return DateTime.Now;
//        }

//        private async Task<List<FilterLog>> FetchTransactionLogs(long fromBlock, long toBlock)
//        {
//            try
//            {
//                var allLogs = new List<FilterLog>();
//                long chunkSize = 2000;

//                for (long i = fromBlock; i <= toBlock; i += chunkSize)
//                {
//                    var end = i + chunkSize;
//                    if (end > toBlock) end = toBlock;

//                    try
//                    {
//                        var filter = new NewFilterInput
//                        {
//                            FromBlock = new BlockParameter(new HexBigInteger(i)),
//                            ToBlock = new BlockParameter(new HexBigInteger(end)),
//                            Address = new[] { CONFIG.Contract }
//                        };

//                        var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filter);

//                        // Filter Transfer events
//                        var transferEventSignature = "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef";
//                        var transferLogs = logs.Where(l => l.Topics != null && l.Topics.Count > 0 && l.Topics[0].ToString() == transferEventSignature).ToList();

//                        allLogs.AddRange(transferLogs);
//                        Console.WriteLine($"Fetched {transferLogs.Count} Transfer logs from blocks {i} to {end}");
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"BSC chunk failed {i}-{end}: {ex.Message}");
//                    }
//                }

//                return allLogs;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"BSC fetch error: {ex.Message}");
//                return new List<FilterLog>();
//            }
//        }

//        private async Task<List<BSCTxRecord>> ProcessTransactions(List<FilterLog> logs, List<BSCTxRecord> existingRecords)
//        {
//            var existingHashes = new HashSet<string>(existingRecords.Select(r => r.txHash));
//            var newRecordsList = new List<BSCTxRecord>();

//            foreach (var log in logs)
//            {
//                if (existingHashes.Contains(log.TransactionHash))
//                    continue;

//                if (log.Topics == null || log.Topics.Count < 3)
//                    continue;

//                try
//                {
//                    // Extract from address
//                    var fromAddressHex = log.Topics[1].ToString();
//                    var fromAddress = "0x" + fromAddressHex.Substring(fromAddressHex.Length - 40);

//                    // Extract to address
//                    var toAddressHex = log.Topics[2].ToString();
//                    var toAddress = "0x" + toAddressHex.Substring(toAddressHex.Length - 40);

//                    // Parse amount from data
//                    if (string.IsNullOrEmpty(log.Data))
//                        continue;

//                    var amountHex = log.Data;
//                    if (amountHex.StartsWith("0x"))
//                        amountHex = amountHex.Substring(2);

//                    if (!BigInteger.TryParse(amountHex, System.Globalization.NumberStyles.HexNumber, null, out var value))
//                        continue;

//                    // Convert to human readable amount
//                    var amount = (decimal)value / (decimal)Math.Pow(10, CONFIG.Decimals);

//                    if (amount <= 0)
//                        continue;

//                    var blockNumber = (long)log.BlockNumber;
//                    var blockTimestamp = await GetBlockTimestamp(blockNumber);

//                    var record = new BSCTxRecord
//                    {
//                        chain = "BSC",
//                        datetime = blockTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),
//                        blockNumber = blockNumber,
//                        txHash = log.TransactionHash,
//                        fromAddress = fromAddress,
//                        toAddress = toAddress,
//                        amount = amount.ToString("N2"),
//                        rawAmount = amount,
//                        tokenSymbol = "USDT",
//                        explorerUrl = $"https://bscscan.com/tx/{log.TransactionHash}"
//                    };

//                    newRecordsList.Add(record);
//                    Console.WriteLine($"New BSC TX: {record.txHash} | {record.amount} USDT");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error processing log: {ex.Message}");
//                }
//            }

//            return newRecordsList;
//        }

//        [HttpPost("Scan")]
//        public async Task<IActionResult> ScanBSC()
//        {
//            try
//            {
//                Console.WriteLine($"BSC scan start: {DateTime.Now}");

//                var currentBlockBigInt = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
//                long currentBlockNumber = (long)currentBlockBigInt.Value;
//                Console.WriteLine($"Current BSC Block: {currentBlockNumber}");

//                var existingRecords = LoadStore();
//                var lastBlockNumber = GetLastBlock();

//                long fromBlockNumber;
//                if (lastBlockNumber == 0)
//                {
//                    fromBlockNumber = currentBlockNumber - CONFIG.BlocksToScan;
//                    if (fromBlockNumber < 0) fromBlockNumber = 1;
//                }
//                else
//                {
//                    fromBlockNumber = lastBlockNumber + 1;
//                }

//                if (fromBlockNumber > currentBlockNumber)
//                {
//                    var last24HoursRecords = existingRecords
//                        .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                        .OrderByDescending(x => x.datetime)
//                        .ToList();

//                    return Ok(new
//                    {
//                        status = "success",
//                        chain = "BSC",
//                        message = last24HoursRecords.Count > 0
//                            ? $"BSC: Last 24 hours mein {last24HoursRecords.Count} USDT transactions"
//                            : "BSC: Last 24 hours mein koi USDT transaction nahi",
//                        totalRecords = existingRecords.Count,
//                        newRecords = 0,
//                        last24HoursRecords = last24HoursRecords.Count,
//                        fromBlock = fromBlockNumber,
//                        toBlock = currentBlockNumber,
//                        fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//                        toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                        data = last24HoursRecords.Take(100)
//                    });
//                }

//                Console.WriteLine($"Scanning from block {fromBlockNumber} to {currentBlockNumber}");

//                var logs = await FetchTransactionLogs(fromBlockNumber, currentBlockNumber);
//                var newRecords = await ProcessTransactions(logs, existingRecords);

//                Console.WriteLine($"New BSC Records: {newRecords.Count}");

//                if (newRecords.Count > 0)
//                {
//                    existingRecords.InsertRange(0, newRecords);
//                    SaveStore(existingRecords);
//                    SaveLastBlock(currentBlockNumber);
//                }

//                var allRecordsSorted = existingRecords.OrderByDescending(x => x.datetime).ToList();

//                var last24HoursRecordsData = allRecordsSorted
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "BSC",
//                    message = last24HoursRecordsData.Count > 0
//                        ? $"BSC: Last 24 hours mein {last24HoursRecordsData.Count} USDT transactions"
//                        : "BSC: Last 24 hours mein koi USDT transaction nahi",
//                    totalRecords = allRecordsSorted.Count,
//                    newRecords = newRecords.Count,
//                    last24HoursRecords = last24HoursRecordsData.Count,
//                    fromBlock = fromBlockNumber,
//                    toBlock = currentBlockNumber,
//                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                    data = last24HoursRecordsData.Take(100)
//                });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error: {ex.Message}");
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "BSC",
//                    message = $"Error: {ex.Message}"
//                });
//            }
//        }

//        [HttpGet("GetData")]
//        public IActionResult GetBSCData()
//        {
//            try
//            {
//                var existingRecords = LoadStore();

//                if (existingRecords == null || existingRecords.Count == 0)
//                {
//                    return Ok(new
//                    {
//                        status = "no_data",
//                        message = "BSC ka koi data nahi aaya! Please pehle /Scan API call karein",
//                        chain = "BSC",
//                        totalRecords = 0,
//                        last24HoursRecords = 0,
//                        data = new List<BSCTxRecord>()
//                    });
//                }

//                var sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

//                var last24HoursRecordsData = sortedRecords
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "BSC",
//                    message = last24HoursRecordsData.Count > 0
//                        ? $"BSC: Last 24 hours mein {last24HoursRecordsData.Count} USDT transactions"
//                        : "BSC: Last 24 hours mein koi USDT transaction nahi",
//                    totalRecords = sortedRecords.Count,
//                    last24HoursRecords = last24HoursRecordsData.Count,
//                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                    data = last24HoursRecordsData
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "BSC",
//                    message = ex.Message
//                });
//            }
//        }

//        [HttpPost("ForceScan")]
//        public async Task<IActionResult> ForceScan()
//        {
//            try
//            {
//                var currentBlockBigInt = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
//                long currentBlockNumber = (long)currentBlockBigInt.Value;
//                var lastBlockNumber = GetLastBlock();

//                long fromBlockNumber = lastBlockNumber == 0 ? currentBlockNumber - 1000 : lastBlockNumber + 1;
//                if (fromBlockNumber < 0) fromBlockNumber = 1;

//                var logs = await FetchTransactionLogs(fromBlockNumber, currentBlockNumber);
//                var existingRecords = LoadStore();
//                var newRecords = await ProcessTransactions(logs, existingRecords);

//                if (newRecords.Count > 0)
//                {
//                    existingRecords.InsertRange(0, newRecords);
//                    SaveStore(existingRecords);
//                    SaveLastBlock(currentBlockNumber);
//                }

//                var sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

//                var last24HoursCount = sortedRecords
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .Count();

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "BSC",
//                    message = newRecords.Count > 0
//                        ? $"Force scan complete! {newRecords.Count} new USDT transactions found"
//                        : "Force scan complete! No new USDT transactions",
//                    newRecords = newRecords.Count,
//                    totalRecords = sortedRecords.Count,
//                    last24HoursRecords = last24HoursCount,
//                    data = newRecords.Take(50)
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    status = "error",
//                    chain = "BSC",
//                    message = ex.Message
//                });
//            }
//        }
//    }
//}