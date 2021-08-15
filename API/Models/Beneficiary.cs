using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class Beneficiary
    {
        public int BeneficiaryId { get; set; }
        public long AccountNumber { get; set; }
        public long BAccountNumber { get; set; }
        public string Nickname { get; set; }

        //public virtual AccountDetail AccountNumberNavigation { get; set; }
        //public virtual AccountDetail BAccountNumberNavigation { get; set; }
    }
}
