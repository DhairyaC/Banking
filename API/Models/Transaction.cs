using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class Transaction
    {
        public int ReferenceId { get; set; }
        public string Mode { get; set; }
        public long PaidToAccNum { get; set; }
        public long FromAccNum { get; set; }
        public decimal Amount { get; set; }
        public string TranStatus { get; set; }
        public string Remark { get; set; }
        public DateTime? TransTime { get; set; }

        public virtual AccountDetail FromAccNumNavigation { get; set; }
        public virtual AccountDetail PaidToAccNumNavigation { get; set; }
    }
}
