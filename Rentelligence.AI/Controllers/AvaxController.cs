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
    public class AvaxController : ControllerBase
    {
        private readonly ITransactionsLogRepository _repo;
        private readonly HttpClient _http;
        //https://api.snowtrace.io/api?module=account&action=txlist&address=0x8db97C7ceCe249c2b98bDC0226Cc4C2A57BF52FC&sort=desc  --lastest Data URl
        public AvaxController(ITransactionsLogRepository repo)
        {
            _repo = repo;

            _http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        // ================= CONFIG =================
        private static class CONFIG
        {
            public static string WalletToTrack =
                "0x8db97C7ceCe249c2b98bDC0226Cc4C2A57BF52FC";

            public static string ApiUrl =
                "https://api.snowtrace.io/api";

            public static string ApiKey = "";

            public static string File =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "avax_transactions.json"
                );

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
            public string explorerUrl { get; set; }
        }

        // ================= API RESPONSE =================
        public class ApiResponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public List<Transaction> result { get; set; }
        }

        public class Transaction
        {
            public string hash { get; set; }
            public string value { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string timeStamp { get; set; }
        }

        // ================= FILE LOAD =================
        private List<Tx> Load()
        {
            if (!System.IO.File.Exists(CONFIG.File))
                return new List<Tx>();

            var json = System.IO.File.ReadAllText(CONFIG.File);

            return string.IsNullOrWhiteSpace(json)
                ? new List<Tx>()
                : JsonConvert.DeserializeObject<List<Tx>>(json)
                    ?? new List<Tx>();
        }

        // ================= FILE SAVE =================
        private void Save(List<Tx> data)
        {
            var clean = data
                .GroupBy(x => x.txHash)
                .Select(x => x.First())
                .Take(CONFIG.MaxRecords)
                .ToList();

            System.IO.File.WriteAllText(
                CONFIG.File,
                JsonConvert.SerializeObject(
                    clean,
                    Formatting.Indented
                )
            );
        }

        // ================= FETCH AVAX =================
        private async Task<List<Tx>> FetchAvax()
        {
            var list = new List<Tx>();

            try
            {
                string url =
                    $"{CONFIG.ApiUrl}" +
                    $"?module=account" +
                    $"&action=txlist" +
                    $"&address={CONFIG.WalletToTrack}" +
                    $"&sort=desc";

                if (!string.IsNullOrWhiteSpace(CONFIG.ApiKey))
                {
                    url += $"&apikey={CONFIG.ApiKey}";
                }

                Console.WriteLine("📡 Fetching AVAX Transactions...");
                Console.WriteLine(url);

                var response = await _http.GetStringAsync(url);

                var data = JsonConvert.DeserializeObject<ApiResponse>(response);

                if (data?.result == null)
                {
                    Console.WriteLine("❌ No transactions found");
                    return list;
                }

                foreach (var tx in data.result)
                {
                    if (string.IsNullOrWhiteSpace(tx.hash))
                        continue;

                    decimal amount = 0;

                    decimal.TryParse(tx.value, out amount);

                    amount /= 1000000000000000000m;

                    if (amount <= 0)
                        continue;

                    list.Add(new Tx
                    {
                        txHash = tx.hash,
                        from = tx.from,
                        to = tx.to,
                        amount = amount,
                        datetime = DateTimeOffset
                            .FromUnixTimeSeconds(
                                Convert.ToInt64(tx.timeStamp)
                            )
                            .LocalDateTime
                            .ToString("yyyy-MM-dd HH:mm:ss"),

                        explorerUrl =
                            $"https://explorer.avax.network/c-chain/tx/{tx.hash}"
                    });
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

                var fetched = await FetchAvax();

                Console.WriteLine($"Fetched Total: {fetched.Count}");

                // ================= LAST HASH LOGIC =================
                var lastHash = existing.FirstOrDefault()?.txHash;

                List<Tx> newData;

                if (!string.IsNullOrEmpty(lastHash))
                {
                    int index =
                        fetched.FindIndex(x => x.txHash == lastHash);

                    newData = index != -1
                        ? fetched.Take(index).ToList()
                        : fetched;
                }
                else
                {
                    newData = fetched;
                }

                int inserted = 0;
                int duplicate = 0;
                int failed = 0;

                if (newData.Any())
                {
                    existing.InsertRange(0, newData);

                    Save(existing);

                    foreach (var tx in newData)
                    {
                        try
                        {
                            var res =
                                await _repo.addTransactionsLog(
                                    new TransactionsLogViewModel
                                    {
                                        NetworkChain = "AVAX",
                                        TransactionHash = tx.txHash,
                                        DateTime = DateTime.Now,
                                        Amount = tx.amount,
                                        FromAddress = tx.from,
                                        ToAddress = tx.to,
                                        TokenSymbol = "AVAX"
                                    });

                            if (res.statusCode == 200)
                                inserted++;

                            else if (res.statusCode == 409)
                                duplicate++;

                            else
                                failed++;
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
                    db = new
                    {
                        inserted,
                        duplicate,
                        failed
                    }
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