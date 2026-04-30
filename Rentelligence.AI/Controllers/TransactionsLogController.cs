using Common;
using Dapper;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using RepositoryContract;
using ServiceContract;
using System.Data;
using System.Globalization;
using System.Numerics;
using ViewModel;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILoggerManager _logger;
    private readonly ExtractToken extractToken;
    private readonly ITransactionsLogRepository _transactionsLogRepository;

    // SINGLE CONSTRUCTOR (IMPORTANT)
    public TransactionController(
        IServiceManager serviceManager,
        ILoggerManager logger,
        IConfiguration configuration,
        ITransactionsLogRepository transactionsLogRepository)
    {
        _serviceManager = serviceManager;
        _logger = logger;
        _transactionsLogRepository = transactionsLogRepository;
        extractToken = new ExtractToken(configuration);
    }

    // ================= CONFIG =================
    private static class CONFIG
    {
        public static string OutputFile =
            Path.Combine(Directory.GetCurrentDirectory(), "all_transactions.json");

        public static int MaxRecords = 10000;
        public static int BlocksToScan = 20000;

        public static class ETH
        {
            public static string[] RpcUrls = {
                "https://eth-mainnet.public.blastapi.io",
                "https://ethereum.publicnode.com"
            };
            public static string Contract = "0xdAC17F958D2ee523a2206206994597C13D831ec7";
            public static int Decimals = 6;
        }

        public static class BSC
        {
            public static string[] RpcUrls = {
                "https://bsc-dataseed.binance.org/"
            };
            public static string Contract = "0x55d398326f99059ff775485246999027b3197955";
            public static int Decimals = 18;
        }
    }

    // ================= MODEL =================
    public class TxRecord
    {
        public string chain { get; set; }
        public string datetime { get; set; }
        public string txHash { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string amount { get; set; }
        public decimal rawAmount { get; set; }
        public string tokenSymbol { get; set; }
        public string explorerUrl { get; set; }
    }

    // ================= STORAGE =================
    private List<TxRecord> LoadStore()
    {
        if (!System.IO.File.Exists(CONFIG.OutputFile))
            return new List<TxRecord>();

        var json = System.IO.File.ReadAllText(CONFIG.OutputFile);
        return JsonConvert.DeserializeObject<List<TxRecord>>(json) ?? new List<TxRecord>();
    }

    private void SaveStore(List<TxRecord> records)
    {
        var trimmed = records
            .GroupBy(x => x.txHash)
            .Select(g => g.First())
            .Take(CONFIG.MaxRecords)
            .ToList();

        var json = JsonConvert.SerializeObject(trimmed, Formatting.Indented);
        System.IO.File.WriteAllText(CONFIG.OutputFile, json);

        Console.WriteLine($"✅ Saved {trimmed.Count} records");
    }

    // ================= BLOCK TRACK =================
    private BigInteger GetLastBlock(string chain)
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), $"last_block_{chain}.txt");

        if (!System.IO.File.Exists(file))
            return 0;

        return BigInteger.Parse(System.IO.File.ReadAllText(file));
    }

    private void SaveLastBlock(string chain, BigInteger block)
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), $"last_block_{chain}.txt");
        System.IO.File.WriteAllText(file, block.ToString());
    }

    private string Now()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    // ================= WEB3 =================
    private async Task<Web3> CreateWeb3(string[] rpcUrls)
    {
        foreach (var url in rpcUrls)
        {
            try
            {
                var web3 = new Web3(url);
                await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                Console.WriteLine($"✅ Connected: {url}");
                return web3;
            }
            catch
            {
                Console.WriteLine($"❌ Failed: {url}");
            }
        }

        throw new Exception("All RPC failed");
    }

    // ================= LOG SCAN =================
    private async Task<List<FilterLog>> ScanLogs(Web3 web3, string contract, BigInteger fromBlock, BigInteger toBlock)
    {
        var allLogs = new List<FilterLog>();
        int chunkSize = 300;

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
                    Address = new[] { contract },
                    Topics = new object[]
                    {
                        "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef"
                    }
                };

                var logs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filter);
                allLogs.AddRange(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Chunk failed {i}-{end}: {ex.Message}");
            }
        }

        return allLogs;
    }

    // ================= PARSE =================
    private TxRecord? ParseLog(FilterLog log, int decimals, string chain)
    {
        if (log.Topics == null || log.Topics.Length < 3)
            return null;

        var from = "0x" + log.Topics[1].ToString().Substring(26);
        var to = "0x" + log.Topics[2].ToString().Substring(26);

        var value = BigInteger.Parse(log.Data.Substring(2), NumberStyles.HexNumber);
        var amount = UnitConversion.Convert.FromWei(value, decimals);

        return new TxRecord
        {
            chain = chain,
            datetime = Now(),
            txHash = log.TransactionHash,
            fromAddress = from,
            toAddress = to,
            amount = amount.ToString("N2"),
            rawAmount = (decimal)amount,
            tokenSymbol = "USDT",
            explorerUrl = chain == "Ethereum"
                ? $"https://etherscan.io/tx/{log.TransactionHash}"
                : $"https://bscscan.com/tx/{log.TransactionHash}"
        };
    }

    // ================= SCAN =================
    private async Task<List<TxRecord>> ScanChain(string chain, string[] rpc, string contract, int decimals)
    {
        var web3 = await CreateWeb3(rpc);
        var currentBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        var lastBlock = GetLastBlock(chain);

        var fromBlock = lastBlock == 0
            ? currentBlock.Value - CONFIG.BlocksToScan
            : lastBlock + 1;

        var logs = await ScanLogs(web3, contract, fromBlock, currentBlock.Value);

        var list = new List<TxRecord>();

        foreach (var log in logs)
        {
            var tx = ParseLog(log, decimals, chain);
            if (tx == null) continue;
            if (tx.rawAmount <= 0) continue;

            list.Add(tx);

            if (list.Count >= CONFIG.MaxRecords)
                break;
        }

        SaveLastBlock(chain, currentBlock.Value);

        return list;
    }

    // ================= API =================
    [HttpPost("Convert")]
    public async Task<IActionResult> Convert()
    {
        try
        {
            var existing = LoadStore();

            var ethTask = ScanChain("Ethereum", CONFIG.ETH.RpcUrls, CONFIG.ETH.Contract, CONFIG.ETH.Decimals);
            var bscTask = ScanChain("BSC", CONFIG.BSC.RpcUrls, CONFIG.BSC.Contract, CONFIG.BSC.Decimals);

            await Task.WhenAll(ethTask, bscTask);

            var newRecords = ethTask.Result.Concat(bscTask.Result).ToList();

            Console.WriteLine($"🔥 New Records: {newRecords.Count}");

            // ✅ SAVE TO DB
            foreach (var tx in newRecords)
            {
                try
                {
                    var model = new TransactionsLogViewModel
                    {
                        NetworkChain = tx.chain,
                        TransactionHash = tx.txHash,
                        DateTime = DateTime.TryParse(tx.datetime, out var dt) ? dt : DateTime.Now,
                        Amount = tx.rawAmount,
                        FromAddress = tx.fromAddress,
                        ToAddress = tx.toAddress,
                        TokenSymbol = tx.tokenSymbol
                    };

                    await _transactionsLogRepository.addTransactionsLog(model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ DB Error {tx.txHash}: {ex.Message}");
                }
            }

            // ✅ SAVE JSON
            existing.InsertRange(0, newRecords);
            SaveStore(existing);

            return Ok(new
            {
                totalRecords = existing.Count,
                insertedInDb = newRecords.Count,
                ethereum = ethTask.Result.Count,
                bsc = bscTask.Result.Count,
                preview = existing.Take(10)
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR: {ex.Message}");
            return StatusCode(500, ex.Message);
        }
    }
}