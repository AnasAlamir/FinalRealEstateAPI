using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.Inquiry
{
    public class InquiryInsertDto
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public string InquiryMessage { get; set; }
    }
}
