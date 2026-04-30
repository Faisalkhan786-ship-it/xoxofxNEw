//using Microsoft.AspNetCore.Mvc;
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
//    public class SolanaController : ControllerBase
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ITransactionsLogRepository _transactionsLogRepository;
//        public SolanaController(
//        ITransactionsLogRepository transactionsLogRepository)
//        {

//            _transactionsLogRepository = transactionsLogRepository;
//        }


//        // ================= CONFIG =================
//        private static class CONFIG
//        {
//            public static string ApiKey = "4ff38f25-abc4-46a3-b609-c1021c49f4b0"; // 🔴 Replace with your key
//            public static string BaseUrl = "https://api.helius.xyz/v0";
//            public static string WalletToTrack = "Es9vMFrzaCERmJfrF4H2FYD4KCoNkY11McCe8BenwNYB"; // USDT mint
//            public static string OutputFile = Path.Combine(Directory.GetCurrentDirectory(), "solana_transactions.json");
//            public static string LastSignatureFile = Path.Combine(Directory.GetCurrentDirectory(), "solana_last_signature.txt");
//            public static int MaxRecords = 10000;

//        }

//        public SolanaController()
//        {
//            _httpClient = new HttpClient();
//        }

//        // ================= MODEL =================
//        public class SolanaTxRecord
//        {
//            public string chain { get; set; }
//            public string datetime { get; set; }
//            public string signature { get; set; }
//            public string fromAddress { get; set; }
//            public string toAddress { get; set; }
//            public string amount { get; set; }
//            public decimal rawAmount { get; set; }
//            public string tokenSymbol { get; set; }
//            public string solscanUrl { get; set; }
//        }

//        public class HeliusTransaction
//        {
//            public string signature { get; set; }
//            public List<HeliusTokenTransfer> tokenTransfers { get; set; }
//            public string timestamp { get; set; }
//        }

//        public class HeliusTokenTransfer
//        {
//            public string mint { get; set; }
//            public string tokenAmount { get; set; }
//            public string fromUserAccount { get; set; }
//            public string toUserAccount { get; set; }
//        }

//        // ================= JSON STORAGE =================
//        private List<SolanaTxRecord> LoadStore()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.OutputFile))
//                {
//                    Console.WriteLine("⚠️ Solana file exist nahi karti");
//                    return new List<SolanaTxRecord>();
//                }

//                var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
//                if (string.IsNullOrWhiteSpace(json)) return new List<SolanaTxRecord>();

//                var records = JsonConvert.DeserializeObject<List<SolanaTxRecord>>(json);
//                Console.WriteLine($"✅ Solana: {records?.Count ?? 0} records load hue");
//                return records ?? new List<SolanaTxRecord>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Solana Load error: {ex.Message}");
//                return new List<SolanaTxRecord>();
//            }
//        }

//        private void SaveStore(List<SolanaTxRecord> records)
//        {
//            var trimmed = records
//                .GroupBy(x => x.signature)
//                .Select(g => g.First())
//                .Take(CONFIG.MaxRecords)
//                .ToList();

//            var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
//            System.IO.File.WriteAllText(CONFIG.OutputFile, json);
//            Console.WriteLine($"✅ Solana: {trimmed.Count} records saved");
//        }

//        private string GetLastSignature()
//        {
//            try
//            {
//                if (!System.IO.File.Exists(CONFIG.LastSignatureFile))
//                    return null;
//                return System.IO.File.ReadAllText(CONFIG.LastSignatureFile);
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private void SaveLastSignature(string signature)
//        {
//            try
//            {
//                System.IO.File.WriteAllText(CONFIG.LastSignatureFile, signature);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Save signature error: {ex.Message}");
//            }
//        }

//        private string Now()
//        {
//            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//        }

//        // ================= FETCH FROM HELIUS =================
//        private async Task<List<HeliusTransaction>> FetchTransactions()
//        {
//            try
//            {
//                var url = $"{CONFIG.BaseUrl}/addresses/{CONFIG.WalletToTrack}/transactions?api-key={CONFIG.ApiKey}";
//                var response = await _httpClient.GetAsync(url);

//                if (!response.IsSuccessStatusCode)
//                {
//                    Console.WriteLine($"❌ Helius API error: {response.StatusCode}");
//                    return new List<HeliusTransaction>();
//                }

//                var json = await response.Content.ReadAsStringAsync();
//                var transactions = JsonConvert.DeserializeObject<List<HeliusTransaction>>(json);

//                return transactions ?? new List<HeliusTransaction>();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Fetch error: {ex.Message}");
//                return new List<HeliusTransaction>();
//            }
//        }

//        // ================= PROCESS TRANSACTIONS =================
//        private List<SolanaTxRecord> ProcessTransactions(List<HeliusTransaction> transactions, List<SolanaTxRecord> existingRecords)
//        {
//            var existingSignatures = new HashSet<string>(existingRecords.Select(r => r.signature));
//            var newRecords = new List<SolanaTxRecord>();

//            foreach (var tx in transactions)
//            {
//                // Skip agar already exist karta hai
//                if (existingSignatures.Contains(tx.signature))
//                    continue;

//                // Check karo token transfers hai ya nahi
//                if (tx.tokenTransfers == null || !tx.tokenTransfers.Any())
//                    continue;

//                foreach (var transfer in tx.tokenTransfers)
//                {
//                    // Sirf USDT filter karo
//                    if (transfer.mint != CONFIG.WalletToTrack)
//                        continue;

//                    // Parse amount (string se decimal mein)
//                    if (!decimal.TryParse(transfer.tokenAmount, out var amount))
//                        continue;

//                    var record = new SolanaTxRecord
//                    {
//                        chain = "Solana",
//                        datetime = Now(),
//                        signature = tx.signature,
//                        fromAddress = transfer.fromUserAccount,
//                        toAddress = transfer.toUserAccount,
//                        amount = amount.ToString("N2"),
//                        rawAmount = amount,
//                        tokenSymbol = "USDT",
//                        solscanUrl = $"https://solscan.io/tx/{tx.signature}"
//                    };

//                    newRecords.Add(record);
//                    Console.WriteLine($"✅ New Solana TX: {record.signature} | {record.amount} USDT");
//                }
//            }

//            return newRecords;
//        }

//        // ================= API 1: Scan & Save Solana =================
//        [HttpPost("Scan")]
//        public async Task<IActionResult> ScanSolana()
//        {
//            try
//            {
//                Console.WriteLine($"🔄 Solana scan start: {DateTime.Now}");

//                // Existing data load karo
//                var existing = LoadStore();

//                // Naye transactions fetch karo
//                var transactions = await FetchTransactions();

//                // Process karo
//                var newRecords = ProcessTransactions(transactions, existing);

//                Console.WriteLine($"🔥 New Solana Records: {newRecords.Count}");

//                // Save karo agar koi new records hain
//                if (newRecords.Count > 0)
//                {
//                    existing.InsertRange(0, newRecords);
//                    SaveStore(existing);

//                    // Last signature save karo
//                    if (newRecords.FirstOrDefault()?.signature != null)
//                    {
//                        SaveLastSignature(newRecords.First().signature);
//                    }
//                }

//                // Last 24 hours data filter karo
//                var last24HoursData = existing
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Solana",
//                    message = last24HoursData.Count > 0
//                        ? $"Solana: Last 24 ghante mein {last24HoursData.Count} USDT transactions"
//                        : "Solana: Last 24 ghante mein koi USDT transaction nahi",
//                    totalRecords = existing.Count,
//                    newRecords = newRecords.Count,
//                    last24HoursRecords = last24HoursData.Count,
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
//                    message = $"Error aaya: {ex.Message}"
//                });
//            }
//        }


//        // ================= API 2: Sirf Get Data (No Scan) =================
//        [HttpGet("GetData")]
//        public async Task<IActionResult> GetSolanaData()
//        {
//            try
//            {
//                var existing = LoadStore();

//                if (existing == null || existing.Count == 0)
//                {
//                    return Ok(new
//                    {
//                        status = "no_data",
//                        message = "Solana ka koi data nahi aaya! Please pehle /Scan API call karein",
//                        chain = "Solana",
//                        totalRecords = 0,
//                        last24HoursRecords = 0,
//                        data = new List<SolanaTxRecord>()
//                    });
//                }

//                var last24Hours = existing
//                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//                    .OrderByDescending(x => x.datetime)
//                    .ToList();

//                // ✅ SAVE TO DB - Yaha sahi se lagaya
//                foreach (var tx in last24Hours)  // 'existing' nahi, 'last24Hours' use kiya
//                {
//                    try
//                    {
//                        var model = new TransactionsLogViewModel
//                        {
//                            NetworkChain = tx.chain ?? "Solana",
//                            TransactionHash = tx.toAddress,
//                            DateTime = DateTime.TryParse(tx.datetime, out var dt) ? dt : DateTime.Now,
//                            Amount = tx.rawAmount,
//                            FromAddress = tx.fromAddress,
//                            ToAddress = tx.toAddress,
//                            TokenSymbol = tx.tokenSymbol ?? "USDT"
//                        };

//                        await _transactionsLogRepository.addTransactionsLog(model);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"❌ DB Error {tx.toAddress}: {ex.Message}");
//                    }
//                }

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Solana",
//                    message = last24Hours.Count > 0
//                        ? $"Solana: Last 24 ghante mein {last24Hours.Count} USDT transactions"
//                        : "Solana: Last 24 ghante mein koi USDT transaction nahi",
//                    totalRecords = existing.Count,
//                    last24HoursRecords = last24Hours.Count,
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
//                    message = ex.Message
//                });
//            }
//        }

//        //[HttpGet("GetData")]
//        //public IActionResult GetSolanaData()
//        //{
//        //    try
//        //    {
//        //        var existing = LoadStore();

//        //        if (existing == null || existing.Count == 0)
//        //        {
//        //            return Ok(new
//        //            {
//        //                status = "no_data",
//        //                message = "Solana ka koi data nahi aaye! Please pehle /Scan API call karein",
//        //                chain = "Solana",
//        //                totalRecords = 0,
//        //                last24HoursRecords = 0,
//        //                data = new List<SolanaTxRecord>()
//        //            });
//        //        }

//        //        var last24Hours = existing
//        //            .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
//        //            .OrderByDescending(x => x.datetime)
//        //            .ToList();

//        //        return Ok(new
//        //        {
//        //            status = "success",
//        //            chain = "Solana",
//        //            message = last24Hours.Count > 0
//        //                ? $"Solana: Last 24 ghante mein {last24Hours.Count} USDT transactions"
//        //                : "Solana: Last 24 ghante mein koi USDT transaction nahi",
//        //            totalRecords = existing.Count,
//        //            last24HoursRecords = last24Hours.Count,
//        //            fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
//        //            toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//        //            data = last24Hours
//        //        });

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return StatusCode(500, new
//        //        {
//        //            status = "error",
//        //            message = ex.Message
//        //        });
//        //    }
//        //}

//        // ================= API 3: Force Live Scan (Ek Baar Fetch Karega) =================
//        [HttpPost("ForceScan")]
//        public async Task<IActionResult> ForceScan()
//        {
//            try
//            {
//                var transactions = await FetchTransactions();
//                var existing = LoadStore();
//                var newRecords = ProcessTransactions(transactions, existing);

//                if (newRecords.Count > 0)
//                {
//                    existing.InsertRange(0, newRecords);
//                    SaveStore(existing);
//                }

//                return Ok(new
//                {
//                    status = "success",
//                    chain = "Solana",
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
//                    message = ex.Message
//                });
//            }
//        }

//        // ================= API 4: Background Live Tracking (Polling) =================
//        [HttpPost("StartLiveTracking")]
//        public async Task<IActionResult> StartLiveTracking()
//        {
//            try
//            {
//                // Background task start karo (har 5 minute)
//                _ = Task.Run(async () =>
//                {
//                    while (true)
//                    {
//                        try
//                        {
//                            Console.WriteLine($"🔄 Solana live tracking: {DateTime.Now}");
//                            var transactions = await FetchTransactions();
//                            var existing = LoadStore();
//                            var newRecords = ProcessTransactions(transactions, existing);

//                            if (newRecords.Count > 0)
//                            {
//                                existing.InsertRange(0, newRecords);
//                                SaveStore(existing);
//                                Console.WriteLine($"✅ Live tracking: {newRecords.Count} new records added");
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine($"❌ Live tracking error: {ex.Message}");
//                        }

//                        // Har 5 minute mein check karega
//                        await Task.Delay(TimeSpan.FromMinutes(5));
//                    }
//                });

//                return Ok(new
//                {
//                    status = "success",
//                    message = "Solana live tracking started! Har 5 minute mein naye transactions check honge.",
//                    chain = "Solana"
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new
//                {
//                    status = "error",
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
using static Arbion_Apis.Controllers.EthereumController;

namespace Arbion_Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolanaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ITransactionsLogRepository _transactionsLogRepository;

        // ================= CONFIG =================
        private static class CONFIG
        {
            public static string ApiKey = "4ff38f25-abc4-46a3-b609-c1021c49f4b0"; // 🔴 Replace with your key
            public static string BaseUrl = "https://api.helius.xyz/v0";
            public static string WalletToTrack = "Es9vMFrzaCERmJfrF4H2FYD4KCoNkY11McCe8BenwNYB"; // USDT mint
            public static string OutputFile = Path.Combine(Directory.GetCurrentDirectory(), "solana_transactions.json");
            public static string LastSignatureFile = Path.Combine(Directory.GetCurrentDirectory(), "solana_last_signature.txt");
            public static int MaxRecords = 10000;
        }

        // ✅ SINGLE CONSTRUCTOR (Fixed)
        public SolanaController(ITransactionsLogRepository transactionsLogRepository)
        {
            _transactionsLogRepository = transactionsLogRepository;
            _httpClient = new HttpClient(); // ✅ HttpClient initialized
        }

        // ================= MODEL =================
        public class SolanaTxRecord
        {
            public string chain { get; set; }
            public string datetime { get; set; }
            public string signature { get; set; }
            public string fromAddress { get; set; }
            public string toAddress { get; set; }
            public string amount { get; set; }
            public decimal rawAmount { get; set; }
            public string tokenSymbol { get; set; }
            public string solscanUrl { get; set; }
        }

        public class HeliusTransaction
        {
            public string signature { get; set; }
            public List<HeliusTokenTransfer> tokenTransfers { get; set; }
            public string timestamp { get; set; }
        }

        public class HeliusTokenTransfer
        {
            public string mint { get; set; }
            public string tokenAmount { get; set; }
            public string fromUserAccount { get; set; }
            public string toUserAccount { get; set; }
        }

        // ================= JSON STORAGE =================
        private List<SolanaTxRecord> LoadStore()
        {
            try
            {
                if (!System.IO.File.Exists(CONFIG.OutputFile))
                {
                    Console.WriteLine("⚠️ Solana file exist nahi karti");
                    return new List<SolanaTxRecord>();
                }

                var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
                if (string.IsNullOrWhiteSpace(json)) return new List<SolanaTxRecord>();

                var records = JsonConvert.DeserializeObject<List<SolanaTxRecord>>(json);
                Console.WriteLine($"✅ Solana: {records?.Count ?? 0} records load hue");
                return records ?? new List<SolanaTxRecord>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Solana Load error: {ex.Message}");
                return new List<SolanaTxRecord>();
            }
        }

        private void SaveStore(List<SolanaTxRecord> records)
        {
            var trimmed = records
                .GroupBy(x => x.signature)
                .Select(g => g.First())
                .Take(CONFIG.MaxRecords)
                .ToList();

            var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
            System.IO.File.WriteAllText(CONFIG.OutputFile, json);
            Console.WriteLine($"✅ Solana: {trimmed.Count} records saved");
        }

        private string GetLastSignature()
        {
            try
            {
                if (!System.IO.File.Exists(CONFIG.LastSignatureFile))
                    return null;
                return System.IO.File.ReadAllText(CONFIG.LastSignatureFile);
            }
            catch
            {
                return null;
            }
        }

        private void SaveLastSignature(string signature)
        {
            try
            {
                System.IO.File.WriteAllText(CONFIG.LastSignatureFile, signature);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Save signature error: {ex.Message}");
            }
        }

        private string Now()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // ================= FETCH FROM HELIUS =================
        private async Task<List<HeliusTransaction>> FetchTransactions()
        {
            try
            {
                var url = $"{CONFIG.BaseUrl}/addresses/{CONFIG.WalletToTrack}/transactions?api-key={CONFIG.ApiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Helius API error: {response.StatusCode}");
                    return new List<HeliusTransaction>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var transactions = JsonConvert.DeserializeObject<List<HeliusTransaction>>(json);

                return transactions ?? new List<HeliusTransaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fetch error: {ex.Message}");
                return new List<HeliusTransaction>();
            }
        }

        // ================= PROCESS TRANSACTIONS =================
        private List<SolanaTxRecord> ProcessTransactions(List<HeliusTransaction> transactions, List<SolanaTxRecord> existingRecords)
        {
            var existingSignatures = new HashSet<string>(existingRecords.Select(r => r.signature));
            var newRecords = new List<SolanaTxRecord>();

            foreach (var tx in transactions)
            {
                if (existingSignatures.Contains(tx.signature))
                    continue;

                if (tx.tokenTransfers == null || !tx.tokenTransfers.Any())
                    continue;

                foreach (var transfer in tx.tokenTransfers)
                {
                    if (transfer.mint != CONFIG.WalletToTrack)
                        continue;

                    if (!decimal.TryParse(transfer.tokenAmount, out var amount))
                        continue;

                    var record = new SolanaTxRecord
                    {
                        chain = "Solana",
                        datetime = Now(),
                        signature = tx.signature,
                        fromAddress = transfer.fromUserAccount,
                        toAddress = transfer.toUserAccount,
                        amount = amount.ToString("N2"),
                        rawAmount = amount,
                        tokenSymbol = "USDT",
                        solscanUrl = $"https://solscan.io/tx/{tx.signature}"
                    };

                    newRecords.Add(record);
                    Console.WriteLine($"✅ New Solana TX: {record.signature} | {record.amount} USDT");
                }
            }

            return newRecords;
        }

        // ================= API 1: Scan & Save Solana =================
        [HttpPost("Scan")]
        public async Task<IActionResult> ScanSolana()
        {
            try
            {
                Console.WriteLine($"🔄 Solana scan start: {DateTime.Now}");

                var existing = LoadStore();
                var transactions = await FetchTransactions();
                var newRecords = ProcessTransactions(transactions, existing);

                Console.WriteLine($"🔥 New Solana Records: {newRecords.Count}");

                if (newRecords.Count > 0)
                {
                    existing.InsertRange(0, newRecords);
                    SaveStore(existing);

                    if (newRecords.FirstOrDefault()?.signature != null)
                    {
                        SaveLastSignature(newRecords.First().signature);
                    }
                }

                var last24HoursData = existing
                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                    .OrderByDescending(x => x.datetime)
                    .ToList();

                return Ok(new
                {
                    status = "success",
                    chain = "Solana",
                    message = last24HoursData.Count > 0
                        ? $"Solana: Last 24 ghante mein {last24HoursData.Count} USDT transactions"
                        : "Solana: Last 24 ghante mein koi USDT transaction nahi",
                    totalRecords = existing.Count,
                    newRecords = newRecords.Count,
                    last24HoursRecords = last24HoursData.Count,
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
                    message = $"Error aaya: {ex.Message}"
                });
            }
        }

        // ================= API 2: Sirf Get Data (No Scan) =================

        // ================= API 2: Sirf Get Data (No Scan) =================
        [HttpGet("GetData")]
        public async Task<IActionResult> GetSolanaData()
        {
            try
            {
                var existing = LoadStore();

                if (existing == null || existing.Count == 0)
                {
                    return Ok(new
                    {
                        status = "no_data",
                        message = "Solana ka koi data nahi aaya! Please pehle /Scan API call karein",
                        chain = "Solana",
                        totalRecords = 0,
                        last24HoursRecords = 0,
                        data = new List<SolanaTxRecord>()
                    });
                }

                var last24Hours = existing
                    .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
                    .OrderByDescending(x => x.datetime)
                    .ToList();

                // ✅ SAVE TO DB - Sirf ek baar call karo
                int successCount = 0;
                int duplicateCount = 0;
                int errorCount = 0;

                foreach (var tx in last24Hours)
                {
                    try
                    {
                        var model = new TransactionsLogViewModel
                        {
                            NetworkChain = "Solana",
                            TransactionHash = tx.signature,
                            DateTime = DateTime.Now,
                            Amount = tx.rawAmount,
                            FromAddress = tx.fromAddress,
                            ToAddress = tx.toAddress,
                            TokenSymbol = "USDT"
                        };

                        // ✅ Sirf EK baar call karo
                        var result = await _transactionsLogRepository.addTransactionsLog(model);

                        if (result.statusCode == 200)
                        {
                            successCount++;
                            Console.WriteLine($"✅ Inserted: {tx.signature}");
                        }
                        else if (result.statusCode == 409)
                        {
                            duplicateCount++;
                            Console.WriteLine($"⚠️ Duplicate: {tx.signature}");
                        }
                        else
                        {
                            errorCount++;
                            Console.WriteLine($"❌ Failed: {tx.signature} - {result.message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"❌ DB Error {tx.signature}: {ex.Message}");
                    }
                }

                Console.WriteLine($"📊 Summary - Success: {successCount}, Duplicate: {duplicateCount}, Error: {errorCount}");

                return Ok(new
                {
                    status = "success",
                    chain = "Solana",
                    message = last24Hours.Count > 0
                        ? $"Solana: Last 24 ghante mein {last24Hours.Count} USDT transactions"
                        : "Solana: Last 24 ghante mein koi USDT transaction nahi",
                    totalRecords = existing.Count,
                    last24HoursRecords = last24Hours.Count,
                    dbInsertSummary = new
                    {
                        total = last24Hours.Count,
                        inserted = successCount,
                        duplicate = duplicateCount,
                        failed = errorCount
                    },
                    fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
                    toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = last24Hours
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
        //[HttpGet("GetData")]
        //public async Task<IActionResult> GetSolanaData()
        //{
        //    try
        //    {
        //        var existing = LoadStore();

        //        if (existing == null || existing.Count == 0)
        //        {
        //            return Ok(new
        //            {
        //                status = "no_data",
        //                message = "Solana ka koi data nahi aaya! Please pehle /Scan API call karein",
        //                chain = "Solana",
        //                totalRecords = 0,
        //                last24HoursRecords = 0,
        //                data = new List<SolanaTxRecord>()
        //            });
        //        }

        //        var last24Hours = existing
        //            .Where(x => DateTime.TryParse(x.datetime, out var dt) && dt >= DateTime.Now.AddHours(-24))
        //            .OrderByDescending(x => x.datetime)
        //            .ToList();

        //        // ✅ SAVE TO DB - Sirf ek baar call karo
        //        int successCount = 0;
        //        int duplicateCount = 0;
        //        int errorCount = 0;

        //        foreach (var tx in last24Hours)
        //        {
        //            try
        //            {
        //                var model = new TransactionsLogViewModel
        //                {
        //                    NetworkChain = "Solana",
        //                    TransactionHash = tx.signature,
        //                    DateTime = DateTime.Now,
        //                    Amount = tx.rawAmount,
        //                    FromAddress = tx.fromAddress,
        //                    ToAddress = tx.toAddress,
        //                    TokenSymbol = "USDT"
        //                };

        //                // ✅ Sirf EK baar call karo
        //                var result = await _transactionsLogRepository.addTransactionsLog(model);

        //                if (result.statusCode == 200)
        //                {
        //                    successCount++;
        //                    Console.WriteLine($"✅ Inserted: {tx.signature}");
        //                }
        //                else if (result.statusCode == 409)
        //                {
        //                    duplicateCount++;
        //                    Console.WriteLine($"⚠️ Duplicate: {tx.signature}");
        //                }
        //                else
        //                {
        //                    errorCount++;
        //                    Console.WriteLine($"❌ Failed: {tx.signature} - {result.message}");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                errorCount++;
        //                Console.WriteLine($"❌ DB Error {tx.signature}: {ex.Message}");
        //            }
        //        }

        //        Console.WriteLine($"📊 Summary - Success: {successCount}, Duplicate: {duplicateCount}, Error: {errorCount}");

        //        return Ok(new
        //        {
        //            status = "success",
        //            chain = "Solana",
        //            message = last24Hours.Count > 0
        //                ? $"Solana: Last 24 ghante mein {last24Hours.Count} USDT transactions"
        //                : "Solana: Last 24 ghante mein koi USDT transaction nahi",
        //            totalRecords = existing.Count,
        //            last24HoursRecords = last24Hours.Count,
        //            dbInsertSummary = new
        //            {
        //                total = last24Hours.Count,
        //                inserted = successCount,
        //                duplicate = duplicateCount,
        //                failed = errorCount
        //            },
        //            fromTime = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"),
        //            toTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //            data = last24Hours
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = "error",
        //            message = ex.Message
        //        });
        //    }
        //}
        // ================= API 3: Force Live Scan =================
        [HttpPost("ForceScan")]
        public async Task<IActionResult> ForceScan()
        {
            try
            {
                var transactions = await FetchTransactions();
                var existing = LoadStore();
                var newRecords = ProcessTransactions(transactions, existing);

                if (newRecords.Count > 0)
                {
                    existing.InsertRange(0, newRecords);
                    SaveStore(existing);
                }

                return Ok(new
                {
                    status = "success",
                    chain = "Solana",
                    message = newRecords.Count > 0
                        ? $"Force scan complete! {newRecords.Count} new USDT transactions found"
                        : "Force scan complete! No new USDT transactions",
                    newRecords = newRecords.Count,
                    totalRecords = existing.Count,
                    data = newRecords.Take(50)
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

        // ================= API 4: Background Live Tracking =================
        [HttpPost("StartLiveTracking")]
        public async Task<IActionResult> StartLiveTracking()
        {
            try
            {
                _ = Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine($"🔄 Solana live tracking: {DateTime.Now}");
                            var transactions = await FetchTransactions();
                            var existing = LoadStore();
                            var newRecords = ProcessTransactions(transactions, existing);

                            if (newRecords.Count > 0)
                            {
                                existing.InsertRange(0, newRecords);
                                SaveStore(existing);
                                Console.WriteLine($"✅ Live tracking: {newRecords.Count} new records added");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Live tracking error: {ex.Message}");
                        }

                        await Task.Delay(TimeSpan.FromMinutes(5));
                    }
                });

                return Ok(new
                {
                    status = "success",
                    message = "Solana live tracking started! Har 5 minute mein naye transactions check honge.",
                    chain = "Solana"
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
    }
}

