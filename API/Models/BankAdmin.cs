using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class BankAdmin
    {
        public BankAdmin()
        {
            Approvals = new HashSet<Approval>();
        }

        public int AdminId { get; set; }
        public string AdminPassword { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
    }
}
