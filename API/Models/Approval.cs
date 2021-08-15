using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class Approval
    {
        public int ApprovalId { get; set; }
        public int? CustId { get; set; }
        public int Srn { get; set; }
        public int? AllotedTo { get; set; }

        public virtual BankAdmin AllotedToNavigation { get; set; }
        public virtual CustomerDetail Cust { get; set; }
    }
}
