using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBankingAPI.ViewModel
{
    public class Registration
    {
        //Customer Details
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


        //Address Details
        public int CustId { get; set; }
        public string TypeOfAddress { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string CustState { get; set; }
        public int PinCode { get; set; }

        public string Line1_Residential { get; set; }
        public string Line2_Residential { get; set; }
        public string Landmark_Residential { get; set; }
        public string City_Residential { get; set; }
        public string CustState_Residential { get; set; }
        public int PinCode_Residential { get; set; }
    }
}
