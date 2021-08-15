using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineBankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly BankingContext db;

        public RegistrationController(BankingContext context)
        {
            db = context;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Registration details)
        {
            CustomerDetail cd = new CustomerDetail();

            // saving email
            string to = details.Email;


            // updating details
            cd.Title = details.Title;
            cd.FirstName = details.FirstName;
            cd.LastName = details.LastName;
            cd.MiddleName = details.MiddleName;
            cd.FathersName = details.FathersName;
            cd.Email = details.Email;
            cd.ApprovalStatus = details.ApprovalStatus;
            cd.Aadhar = details.Aadhar;
            cd.PanCard = details.PanCard;
            cd.MobileNumber = details.MobileNumber;
            cd.Gender = "not set";
            cd.PanDoc = details.PanDoc;
            cd.Dob = details.Dob;
            cd.OccupationType = details.OccupationType;
            cd.SourceOfIncome = details.SourceOfIncome;
            cd.GrossAnnualIncome = details.GrossAnnualIncome;
            db.CustomerDetails.Add(cd);
            db.SaveChanges();

            // fetch custid
            CustomerDetail cc = db.CustomerDetails.Where(c => c.Email == details.Email).FirstOrDefault();
            int custId = cc.CustId;

            // updating address details
            if (details.TypeOfAddress == "Yes")
            {
                CustAddress ca = new CustAddress();
                ca.CustId = custId;
                ca.TypeOfAddress = "Same";
                ca.Line1 = details.Line1;
                ca.Line2 = details.Line2;
                ca.Landmark = details.Landmark;
                ca.City = details.City;
                ca.CustState = details.CustState;
                ca.PinCode = details.PinCode;
                db.CustAddresses.Add(ca);
            }
            else
            {
                // adding permanent address details

                CustAddress ca2 = new CustAddress();
                ca2.TypeOfAddress = "Permanent";
                ca2.Line1 = details.Line1;
                ca2.Line2 = details.Line2;
                ca2.Landmark = details.Landmark;
                ca2.City = details.City;
                ca2.CustState = details.CustState;
                ca2.PinCode = details.PinCode;
                ca2.CustId = custId;
                db.CustAddresses.Add(ca2);
                db.SaveChanges();


                // adding residential address details

                CustAddress ca3 = new CustAddress();
                ca3.TypeOfAddress = "Resident";
                ca3.Line1 = details.Line1_Residential;
                ca3.Line2 = details.Line2_Residential;
                ca3.Landmark = details.Landmark_Residential;
                ca3.City = details.City_Residential;
                ca3.CustState = details.CustState_Residential;
                ca3.PinCode = details.PinCode_Residential;
                ca3.CustId = custId;
                db.CustAddresses.Add(ca3);
                db.SaveChanges();

            }

            // add record to approvals to admin-1
            Approval ap = new Approval();
            Random rand = new Random();
            int srn = rand.Next(1000000, 9999999);
            int allotedTo = 10672240; // 
            ap.CustId = custId;
            ap.AllotedTo = allotedTo;
            ap.Srn = srn;
            db.Approvals.Add(ap);
            db.SaveChanges();

            //-----------------------------------------------------
            // add record to approvals to admin-2
            Approval ap1 = new Approval();
            int allotedTo2 = 10672513;
            ap1.CustId = custId;
            ap1.Srn = srn;
            ap1.AllotedTo = allotedTo2;
            db.Approvals.Add(ap1);
            db.SaveChanges();

            //-----------------------------------------------------
            if (SendMailRefNumber(srn, to) == 1)
            {
                return Ok("inserted");
            }
            else
            {
                return Ok("error in sending mail");
            }

        }

        // SEND EMAIL OF REFERENCE NUMBER
        private int SendMailRefNumber(int srn, string to)
        {
            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("dashbank7@gmail.com", "Dash@1399"),
            };

            string subject = "Reference Number";
            string body = "Reference Number  " + srn;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return 1;
            }

            catch (Exception e)
            {
                return -1;
            }
        }
    }
    
}
