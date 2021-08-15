using System;
using System.Collections.Generic;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class CustAddress
    {
        public int CustId { get; set; }
        public string TypeOfAddress { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string CustState { get; set; }
        public int PinCode { get; set; }

        public virtual CustomerDetail Cust { get; set; }
    }
}
