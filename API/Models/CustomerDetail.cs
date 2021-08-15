using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class CustomerDetail
    {
        public CustomerDetail()
        {
            AccountDetails = new HashSet<AccountDetail>();
            CustAddresses = new HashSet<CustAddress>();
        }

        public int CustId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public long Aadhar { get; set; }
        public string PanCard { get; set; }
        public string PanDoc { get; set; }
        public DateTime Dob { get; set; }
        public string OccupationType { get; set; }
        public string SourceOfIncome { get; set; }
        public string GrossAnnualIncome { get; set; }
        public bool DebitCard { get; set; }
        public bool NetBanking { get; set; }
        public string ApprovalStatus { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
        public virtual ICollection<AccountDetail> AccountDetails { get; set; }
        public virtual ICollection<CustAddress> CustAddresses { get; set; }
    }
}
