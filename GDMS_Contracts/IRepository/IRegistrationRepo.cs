using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDMS_DAL.Models;

namespace GDMS_Contracts.IRepository
{
    public interface IRegistrationRepo
    {
        LAND_PARCELS? GetLandParcelByREN(string REN);
        LAND_PARCELS UpdateLandParcelByREN(string REN, int Status);
        bool InsertintoLAND_PARCELS_Reg(string REN);
    }
}
