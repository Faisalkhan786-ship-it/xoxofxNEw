
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

namespace Arbion_Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BSCTronController : ControllerBase
    {
        private Web3 _web3;

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

        private long GetLastBlock()
        {
            try
            {
                if (!System.IO.File.Exists(CONFIG.LastBlockFile))
                    return 0;
                return long.Parse(System.IO.File.ReadAllText(CONFIG.LastBlockFile));
            }
            catch
            {
                return 0;
            }
        }

        private void SaveLastBlock(long block)
        {
            try
            {
                System.IO.File.WriteAllText(CONFIG.LastBlockFile, block.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save block error: {ex.Message}");
            }
        }

        private async Task<DateTime> GetBlockTimestamp(long blockNumber)
        {
            try
            {
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                    new HexBigInteger(blockNumber)
                );

                if (block != null && block.Timestamp != null)
                {
                    long timestamp = (long)block.Timestamp.Value;
                    return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get block timestamp: {ex.Message}");
            }
            return DateTime.Now;
        }

        private async Task<List<FilterLog>> FetchTransactionLogs(long fromBlock, long toBlock)
        {
            try
            {
                var allLogs = new List<FilterLog>();
                long chunkSize = 1000;

                for (long i = fromBlock; i <= toBlock; i += chunkSize)
                {
                    var end = i + chunkSize;
                    if (end > toBlock) end = toBlock;

                    try
                    {
                        var filter = new NewFilterInput
                        {
                            FromBlock = new BlockParameter(new HexBigInteger(i)),
                            ToBlock = new BlockParameter(new HexBigInteger(end)),
                            Address = new[] { CONFIG.Contract }
                        };

                        var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(filter);

                        // FIX CS0428 - logs is List, .Count is property (no parentheses)
                        if (logs != null)
                        {
                            int logCount = logs.Count();  // ← No parentheses!
                            if (logCount > 0)
                            {
                                var transferEventSignature = "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef";
                                var transferLogs = new List<FilterLog>();

                                foreach (var singleLog in logs)
                                {
                                    if (singleLog.Topics != null)
                                    {
                                        int topicCount = singleLog.Topics.Count();  // ← No parentheses!
                                        if (topicCount > 0)
                                        {
                                            string firstTopic = singleLog.Topics[0]?.ToString() ?? "";
                                            if (firstTopic == transferEventSignature)
                                            {
                                                transferLogs.Add(singleLog);
                                            }
                                        }
                                    }
                                }

                                allLogs.AddRange(transferLogs);
                                Console.WriteLine($"Fetched {transferLogs.Count} Transfer logs from blocks {i} to {end}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"BSC chunk failed {i}-{end}: {ex.Message}");
                    }

                    await Task.Delay(100);
                }

                return allLogs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BSC fetch error: {ex.Message}");
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

                // FIX CS0428 - Topics ka .Count property hai, method nahi
                if (log.Topics == null)
                    continue;

                int topicsCount = log.Topics.Count();  // ← No parentheses! Count property hai
                if (topicsCount < 3)                // ← Yahan bhi no parentheses
                    continue;

                try
                {
                    string fromAddressHex = log.Topics[1]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(fromAddressHex) || fromAddressHex.Length < 40)
                        continue;
                    string fromAddress = "0x" + fromAddressHex.Substring(fromAddressHex.Length - 40);

                    string toAddressHex = log.Topics[2]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(toAddressHex) || toAddressHex.Length < 40)
                        continue;
                    string toAddress = "0x" + toAddressHex.Substring(toAddressHex.Length - 40);

                    if (string.IsNullOrEmpty(log.Data))
                        continue;

                    string amountHex = log.Data;
                    if (amountHex.StartsWith("0x"))
                        amountHex = amountHex.Substring(2);

                    if (!BigInteger.TryParse(amountHex, System.Globalization.NumberStyles.HexNumber, null, out BigInteger value))
                        continue;

                    decimal amount = (decimal)value / (decimal)Math.Pow(10, CONFIG.Decimals);

                    if (amount <= 0)
                        continue;

                    long blockNumber = 0;
                    if (log.BlockNumber != null)
                    {
                        blockNumber = (long)log.BlockNumber.Value;
                    }

                    DateTime blockTimestamp = await GetBlockTimestamp(blockNumber);

                    BSCTxRecord record = new BSCTxRecord
                    {
                        chain = "BSC",
                        datetime = blockTimestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                        blockNumber = blockNumber,
                        txHash = log.TransactionHash,
                        fromAddress = fromAddress,
                        toAddress = toAddress,
                        amount = amount.ToString("N2"),
                        rawAmount = amount,
                        tokenSymbol = "USDT",
                        explorerUrl = $"https://bscscan.com/tx/{log.TransactionHash}"
                    };

                    newRecordsList.Add(record);
                    Console.WriteLine($"✅ New BSC TX: {record.txHash} | {record.amount} USDT");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error processing log: {ex.Message}");
                }
            }

            return newRecordsList;
        }

        [HttpPost("Scan")]
        public async Task<IActionResult> ScanBSC()
        {
            try
            {
                Console.WriteLine($"🔄 BSC scan start: {DateTime.Now}");

                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                long currentBlockNumber = (long)currentBlock.Value;
                Console.WriteLine($"📦 Current BSC Block: {currentBlockNumber}");

                List<BSCTxRecord> existingRecords = LoadStore();
                long lastBlockNumber = GetLastBlock();

                long fromBlockNumber;
                if (lastBlockNumber == 0)
                {
                    fromBlockNumber = currentBlockNumber - 1000;
                    if (fromBlockNumber < 0) fromBlockNumber = 1;
                }
                else
                {
                    fromBlockNumber = lastBlockNumber + 1;
                }

                if (fromBlockNumber > currentBlockNumber)
                {
                    var last24HoursData = existingRecords
                        .Where(x => DateTime.TryParse(x.datetime, out DateTime dt) && dt >= DateTime.Now.AddHours(-24))
                        .OrderByDescending(x => x.datetime)
                        .ToList();

                    return Ok(new
                    {
                        status = "success",
                        chain = "BSC",
                        message = last24HoursData.Count > 0
                            ? $"BSC: Last 24 hours mein {last24HoursData.Count} USDT transactions"
                            : "BSC: Last 24 hours mein koi USDT transaction nahi",
                        totalRecords = existingRecords.Count,
                        newRecords = 0,
                        last24HoursRecords = last24HoursData.Count,
                        fromBlock = fromBlockNumber,
                        toBlock = currentBlockNumber,
                        fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                        toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        data = last24HoursData.Take(100)
                    });
                }

                Console.WriteLine($"📊 Scanning from block {fromBlockNumber} to {currentBlockNumber}");

                List<FilterLog> logs = await FetchTransactionLogs(fromBlockNumber, currentBlockNumber);
                List<BSCTxRecord> newRecords = await ProcessTransactions(logs, existingRecords);

                Console.WriteLine($"🔥 New BSC Records: {newRecords.Count}");

                if (newRecords.Count > 0)
                {
                    existingRecords.InsertRange(0, newRecords);
                    SaveStore(existingRecords);
                    SaveLastBlock(currentBlockNumber);
                }

                List<BSCTxRecord> allRecordsSorted = existingRecords.OrderByDescending(x => x.datetime).ToList();

                var last24HoursFilteredData = allRecordsSorted
                    .Where(x => DateTime.TryParse(x.datetime, out DateTime dt) && dt >= DateTime.Now.AddHours(-24))
                    .ToList();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = last24HoursFilteredData.Count > 0
                        ? $"✅ Found {last24HoursFilteredData.Count} USDT transactions in last 24 hours"
                        : "❌ No USDT transactions found in last 24 hours",
                    totalRecords = allRecordsSorted.Count,
                    newRecords = newRecords.Count,
                    last24HoursRecords = last24HoursFilteredData.Count,
                    fromBlock = fromBlockNumber,
                    toBlock = currentBlockNumber,
                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = last24HoursFilteredData.Take(100)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    status = "error",
                    chain = "BSC",
                    message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("GetData")]
        public IActionResult GetBSCData()
        {
            try
            {
                List<BSCTxRecord> existingRecords = LoadStore();

                if (existingRecords == null || existingRecords.Count == 0)
                {
                    return Ok(new
                    {
                        status = "no_data",
                        message = "BSC ka koi data nahi aaya! Please pehle /Scan API call karein",
                        chain = "BSC",
                        totalRecords = 0,
                        last24HoursRecords = 0,
                        data = new List<BSCTxRecord>()
                    });
                }

                List<BSCTxRecord> sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

                var last24HoursFilteredData = sortedRecords
                    .Where(x => DateTime.TryParse(x.datetime, out DateTime dt) && dt >= DateTime.Now.AddHours(-24))
                    .ToList();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = last24HoursFilteredData.Count > 0
                        ? $"✅ Found {last24HoursFilteredData.Count} USDT transactions in last 24 hours"
                        : "❌ No USDT transactions found in last 24 hours",
                    totalRecords = sortedRecords.Count,
                    last24HoursRecords = last24HoursFilteredData.Count,
                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = last24HoursFilteredData
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

        [HttpPost("ForceScan")]
        public async Task<IActionResult> ForceScan()
        {
            try
            {
                var currentBlock = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                long currentBlockNumber = (long)currentBlock.Value;
                long lastBlockNumber = GetLastBlock();

                long fromBlockNumber = lastBlockNumber == 0 ? currentBlockNumber - 100 : lastBlockNumber + 1;
                if (fromBlockNumber < 0) fromBlockNumber = 1;

                Console.WriteLine($"🔍 Force scan from {fromBlockNumber} to {currentBlockNumber}");

                List<FilterLog> logs = await FetchTransactionLogs(fromBlockNumber, currentBlockNumber);
                List<BSCTxRecord> existingRecords = LoadStore();
                List<BSCTxRecord> newRecords = await ProcessTransactions(logs, existingRecords);

                if (newRecords.Count > 0)
                {
                    existingRecords.InsertRange(0, newRecords);
                    SaveStore(existingRecords);
                    SaveLastBlock(currentBlockNumber);
                }

                List<BSCTxRecord> sortedRecords = existingRecords.OrderByDescending(x => x.datetime).ToList();

                int last24HoursCount = sortedRecords
                    .Where(x => DateTime.TryParse(x.datetime, out DateTime dt) && dt >= DateTime.Now.AddHours(-24))
                    .Count();

                return Ok(new
                {
                    status = "success",
                    chain = "BSC",
                    message = newRecords.Count > 0
                        ? $"✅ Force scan complete! {newRecords.Count} new USDT transactions found"
                        : "❌ Force scan complete! No new USDT transactions",
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