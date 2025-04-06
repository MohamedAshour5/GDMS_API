using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMS_DAL.Models
{
    [Table("LAND_PARCELS", Schema = "SDE")]
    public class LAND_PARCELS
    {
        [Key]
        public int? OBJECTID { get; set; }
        public string?  LP_ID { get; set; }
        public int? REGISTRATION_STATUS { get; set; }

    }
}
