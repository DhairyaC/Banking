using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class AccountDetail
    {
        public AccountDetail()
        {
            TransactionFromAccNumNavigations = new HashSet<Transaction>();
            TransactionPaidToAccNumNavigations = new HashSet<Transaction>();
        }

        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int UserId { get; set; }
        public int? CustId { get; set; }
        public string LoginPassword { get; set; }
        public string TransactionPassword { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastLogin { get; set; }
        public string AccountStatus { get; set; }
        public decimal CurrentBalance { get; set; }

        public virtual CustomerDetail Cust { get; set; }
        //public virtual Beneficiary BeneficiaryAccountNumberNavigation { get; set; }
        //public virtual Beneficiary BeneficiaryBAccountNumberNavigation { get; set; }
        public virtual ICollection<Transaction> TransactionFromAccNumNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionPaidToAccNumNavigations { get; set; }
    }
}
