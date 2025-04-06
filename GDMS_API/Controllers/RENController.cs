using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json.Linq;
using GDMS_API.Enums;
using GDMS_API.Interfaces;
using GDMS_API.Models;
using GDMS_API.Services;
using GDMS_Contracts.IRepository;
using GDMS_DAL.ComonManager;
using GDMS_DAL.Context;

namespace GDMS_API.Controllers
{
    [Route("api/REN")]
    [ApiController]

    public class RENController : ControllerBase
    {
        private readonly ICommonHelperFunction _commonHelperFunction;
        private readonly IService _apiService;
        private readonly IConfiguration _configuration;
        private readonly IRegistrationRepo _IRegistrationRepo;
        public RENController(IService apiService, ICommonHelperFunction commonHelperFunction, IConfiguration configuration, IRegistrationRepo registrationRepo)
        {
            _apiService = apiService;
            _commonHelperFunction = commonHelperFunction;
            _configuration = configuration;
            _IRegistrationRepo = registrationRepo;
        }
        [HttpPost]
        [Route("GetOwnerInfo")]
        public async Task<IActionResult> GetOwnerInfoAsync(RequsetParam REN)
        {
            string Token = await _apiService.GetToken();
            OwnerInfos result = await _apiService.GetTitleByRen(REN.REN, Token);
            return Ok(result);
        }
        [HttpPost]
        [Route("UpdateStatus")]
        [Authorize(Roles = UserRoles.Update_REGISTRATION_STATUS)]
        public ActionResult UpdateResgistrationStatus(UpdateParam updateParam)
        {
            int? result = _commonHelperFunction.checkStatusIsFound(updateParam.Status_ID);

            if (result == 0) return Ok("satus ID In Not Found ");

            var Land_parcel = _IRegistrationRepo.UpdateLandParcelByREN(updateParam.REN, updateParam.Status_ID);

            if (Land_parcel != null && Land_parcel.LP_ID == "REN is not found")
                return Ok("REN is not found");
            else if (Land_parcel != null)
                return Ok(Land_parcel);
            else
                return Ok("DB Not connect Sucssuccfully");
        }
    }
}
