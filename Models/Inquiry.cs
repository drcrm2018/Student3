using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Student3.Models
{
    public class Inquiry
    {
        [Key]
        public Guid InquiryId { get; set; }
        public String Question { get; set; }
        public String Response { get; set;}
        public Guid ContactId { get; set; }
        public String UserId { get; set; }
    }
}
