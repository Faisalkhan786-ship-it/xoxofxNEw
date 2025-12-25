using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceContract;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ViewModel;
using static ViewModel.GeographyViewModel;

namespace Rentelligence.AI.MarketPlace.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
  
    public class GeographyController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILoggerManager _logger;
        public GeographyController(IServiceManager serviceManager, ILoggerManager logger, IConfiguration configuration)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }
        [HttpGet("getAllCountry")]
        public async Task<IActionResult> getAllCountryMethod()
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllCountryMethod");
            var getAllShippingMethod = await _serviceManager.geographyContract.getAllCountryMethod();
            if (getAllShippingMethod.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No County List");
            }
            return Ok(getAllShippingMethod);
        }
        [HttpGet("getAllState")]
        [Authorize]
        public async Task<IActionResult> getAllStateMethod(int Fk_CountryId)
        {
            _logger.logInfo($" {LoggingEvents.getAllItem} getAllStateMethod");
            var getAllShippingMethod = await _serviceManager.geographyContract.getAllStateMethod(Fk_CountryId);
            if (getAllShippingMethod.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No County List");
            }
            return Ok(getAllShippingMethod);
        }
        [HttpGet("getAllCity")]
        [Authorize]
        public async Task<IActionResult> getAllCityMethod(int Fk_StateId)
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllCityMethod Fk_StateId ${Fk_StateId}");
            var getByIdShipping = await _serviceManager.geographyContract.getAllCityMethod(Fk_StateId);
            if (getByIdShipping.statusCode == (int)HttpStatusCode.NotFound)
            {
                _logger.logWarn($"{LoggingEvents.getItemNotFound},No City Found");
            }
            return Ok(getByIdShipping);
        }
        ///
        [HttpPost("getAllContacUs")]
        [Authorize]
        public async Task<IActionResult> getAllContacUs()
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllContacUs");
            var get = await _serviceManager.geographyContract.getAllContacUs();        
            return Ok(get);
        }

 

        [HttpPost("addContactUs")]
        public async Task<IActionResult> addContactUs(ContactUsViewModel contactUsViewModel)
        {
            _logger.logInfo($" {LoggingEvents.updateItem} addContactUs");
            var add = await _serviceManager.geographyContract.addContactUs(contactUsViewModel);
            return Ok(add);
        }

        [HttpPost("getAllCareerType")]
        public async Task<IActionResult> getAllCareerType()
        {
            _logger.logInfo($" {LoggingEvents.getByIdItem} getAllCareerType");
            var get = await _serviceManager.geographyContract.getAllCareerType();
            return Ok(get);
        }
    }
}
