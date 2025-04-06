using logger;
using Microsoft.Extensions.Configuration;
using GDMS_Contracts.IRepository;
using GDMS_DAL.Context;
using GDMS_DAL.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
namespace GDMS_Contracts.Repository
{
    public class RegistrationRepo : IRegistrationRepo
    {
        private readonly IConfiguration _configuration;

        public RegistrationRepo(GDMS_DbContext rM_DbContext, IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public LAND_PARCELS? GetLandParcelByREN(string REN)
        {
            try
            {
                using (GDMS_DbContext _context = new GDMS_DbContext(_configuration))
                {
                    LAND_PARCELS? Land_Parcels = _context.LAND_PARCELs.FirstOrDefault(x => x.LP_ID == REN);
                    _context.Dispose();
                    return Land_Parcels;
                }
            }
            catch (Exception ex)
            {
                Serilog_Logger.LogError("function :( GetLandParcelByREN ) Error : " + ex.Message + ex.InnerException);
                return null;
            }
        }
        public LAND_PARCELS UpdateLandParcelByREN(string REN, int Status)
        {
            try
            {
                LAND_PARCELS? lAND_PARCELS = GetLandParcelByREN(REN);
                if (lAND_PARCELS == null)
                {
                    Serilog_Logger.LogError("function :( UpdateLandParcelByREN ) Error :REN is not found ");

                    return new LAND_PARCELS() { LP_ID = "REN is not found" };
                }
                using (GDMS_DbContext _context = new GDMS_DbContext(_configuration))
                {

                    lAND_PARCELS.REGISTRATION_STATUS = Status;
                    _context.LAND_PARCELs.Update(lAND_PARCELS);
                    _context.SaveChanges();
                    _context.Dispose();
                    if (Status == 2)
                        InsertintoLAND_PARCELS_Reg(REN);
                    return lAND_PARCELS;
                }


            }
            catch (Exception ex)
            {
                Serilog_Logger.LogError("function :( UpdateLandParcelByREN )  in REN \"" + REN + "\" Error Message  : " + ex.Message + ex.InnerException);
                return null;
            }
        }
        public bool InsertintoLAND_PARCELS_Reg(string REN)
        {
            try
            {
                using (GDMS_DbContext _context = new GDMS_DbContext(_configuration))
                {
                    _context.Database.ExecuteSqlRaw("SDE.InsertToLAND_PARCELS_Reg @InputParam", new SqlParameter("@InputParam", REN));
                }
                return true;
            }
            catch (Exception ex)
            {
                Serilog_Logger.LogError("function :( InsertintoLAND_PARCELS_Reg )  in REN \"" + REN + "\" Error Message  : " + ex.Message + " Error InnerException : " + ex.InnerException);
                return false;
            }
        }

    }
}
