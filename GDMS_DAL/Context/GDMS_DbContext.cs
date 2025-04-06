using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GDMS_DAL.Models;

namespace GDMS_DAL.Context
{
    public class GDMS_DbContext: DbContext
    {
        private IConfiguration _config;
        public GDMS_DbContext(IConfiguration config)
        {
            _config = config;
        }
        public GDMS_DbContext(DbContextOptions<GDMS_DbContext> options, IConfiguration config):base(options)
        {
            _config = config;
        }

        public virtual DbSet<LAND_PARCELS> LAND_PARCELs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_config["GDMS_DBEntities"]))
            {
              //  var con = EncryptionManager.DecryptString(_config["GDMS_DBEntities"]);
                optionsBuilder.UseSqlServer(_config["GDMS_DBEntities"]);
            }
        }
    }
}
