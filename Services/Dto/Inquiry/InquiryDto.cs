using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto.Inquiry
{
    public class InquiryDto
    {
        public int InquiryId { get; set; }
        public string UserName { get; set; }
        public string PropertyName { get; set; }
        public string InquiryMessage { get; set; }
        public DateTime InquiryDateSent { get; set; }
    }
}
