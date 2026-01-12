using Common;
using EmailSystem;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Repository;
using RepositoryContract;
using ServiceContract;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModel;
using static Repository.SelfRepository;


namespace Rentelligence.AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SelfController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly ExtractToken _extractToken;
        private readonly ISelfRepository _selfRepository;

        public SelfController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration, ISelfRepository selfRepository)
        {
            _serviceManager = serviceManager;
            _logger = logger;
            _configuration = configuration;
            _selfRepository = selfRepository;
            _emailService = new EmailService(configuration);
            _extractToken = new ExtractToken(configuration);
        }


       //generate Wallet Address and private key
        [HttpPost("GenerateWalletAddress")]
        public IActionResult GenerateWalletAddress([FromBody] RequestUserWalletDetailsViewModel model)
        {
            var result = _selfRepository.GenerateWalletAddress(model);

            if (result.status == "Succeed")
                return Ok(result);
            else
                return BadRequest(result); 
        }

        [HttpPost("SITOBalance")]
        public async Task<IActionResult> SITOBalance([FromBody] RequestWalletAddressModel model)
        {
            try
            {
                var result = await _selfRepository.SItoBalanceAsync(model);
                if (result.status == "succeed")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result2<AddFundModel>
                {
                    status = "failed",
                    message = "Internal server error: " + ex.Message
                });
            }
        }
        //Check USDT balance on Waller Address
        [HttpPost("USDTBalance")]
        public async Task<IActionResult> USDTBalance([FromBody] RequestWalletAddressModel model)
        {
            try
            {
                var result = await _selfRepository.USDTBalanceAsync(model);
                if (result.status == "succeed") 
                    return Ok(result); 
                else
                    return BadRequest(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result2<AddFundModel>
                {
                    status = "failed",
                    message = "Internal server error: " + ex.Message
                });
            }
        }

        [HttpPost("SendSITODepositRequest")]
        public async Task<IActionResult> SendSITODepositRequest([FromBody] RequestDepositusdtModel model)
        {
            try
            {
                var result = await _selfRepository.SendSITODepositRequest(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new Result2<resposeAddFundModel>
                {
                    status = "failed",
                    message = "Internal server error: " + ex.Message,
                    data = null
                });
            }
        }

        //BNB Withdrawal Reqeuest 
        [HttpPost("SendUSDTDepositRequest")]
        public async Task<IActionResult> SendUSDTDepositRequest([FromBody] RequestDepositusdtModel model)
        {
            try
            {
                var result = await _selfRepository.SendUSDTDepositRequest(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new Result2<resposeAddFundModel>
                {
                    status = "failed",
                    message = "Internal server error: " + ex.Message,
                    data = null
                });
            }
        }


        //get All Wallet Address  
        [HttpPost("getAllWalletAddress")]
        public async Task<IActionResult> getAllWalletAddress()
        {
            var getAllWalletAddress = await _selfRepository.getAllWalletAddress();
            return Ok(getAllWalletAddress);
        }

        //get All Wallet Address By URID
        [HttpPost("getWalletAddressByURID")]
        public async Task<IActionResult> getWalletAddressByURID(Guid URID)
        {
            var getAllWalletAddress = await _selfRepository.getAllWalletAddressByURID(URID);
            return Ok(getAllWalletAddress);
        }


        //get USDT details list By URID
        [HttpPost("getSelfDepsiteDetailsByURID")]
        public async Task<IActionResult> getSelfDepsiteDetailsByURID(Guid URID)
        {
            var getAllWalletAddress = await _selfRepository.GetSelfDepsiteByURID(URID);
            return Ok(getAllWalletAddress);
        }
    }
}
