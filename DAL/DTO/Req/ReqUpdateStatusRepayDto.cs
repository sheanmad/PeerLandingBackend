using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqUpdateStatusRepayDto
    {
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
