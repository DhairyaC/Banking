using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace OnlineBankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BankingContext db;

        public AdminController(BankingContext context)
        {
            db = context;
        }


        //GET:ADMIN LOGIN
        #region admin-login
        [HttpGet]
        [Route("login")]
        public IActionResult AdminLogin([FromQuery(Name = "id")] int adminId, [FromQuery(Name = "password")] string adminPassword)
        {
            BankAdmin ba = db.BankAdmins.Where(ad => ad.AdminId == adminId).FirstOrDefault();
            if (ba == null)
            {
                return BadRequest("invalid id");
            }
            if (ba.AdminPassword == adminPassword)
            {
                return Ok("valid");
            }
            else
            {
                return Ok("invalid password");
            }
        }
        #endregion


        //GET:PENDING LIST
        #region pending-list
        [HttpGet]
        [Route("pending-list")]
        public IActionResult PendingList([FromQuery(Name = "adminid")] int adminId)
        {
            Approval app = db.Approvals.Where(ad => ad.AllotedTo == adminId).FirstOrDefault();
            if (app == null)
            {
                return BadRequest("not alloted anything");
            }
            int? custId = app.CustId;
            List<CustAddress> addList = db.CustAddresses.Where(cu => cu.CustId == custId).ToList();
            CustAddress add1 = null; CustAddress add2 = null;
            if (addList.Count == 2)
            {
                add1 = addList[0];
                add2 = addList[1];
            }
            else
            {
                add1 = addList[0];
                add2 = addList[0];
            }

            var res = (from c in db.CustomerDetails
                      join a in db.Approvals
                      on c.CustId equals a.CustId
                      where c.ApprovalStatus == "Pending" && a.AllotedTo == adminId
                      select new { cust_id = c.CustId, title=c.Title, first_name=c.FirstName, middle_name=c.MiddleName,
                      last_name=c.LastName, father_name=c.FathersName, gender=c.Gender, mobile_number=c.MobileNumber,
                      email=c.Email, aadhar=c.Aadhar, pan_card=c.PanCard, pan_doc=c.PanDoc, Occupation_type=c.OccupationType,
                      source_of_income=c.SourceOfIncome, gross_annual_income=c.GrossAnnualIncome, debit_card=c.DebitCard, net_banking=c.NetBanking, approval_status=c.ApprovalStatus,
                      line1 = add1.Line1, line2=add1.Line2, landmark=add1.Landmark, city=add1.City, cust_state=add1.CustState, pin_code=add1.PinCode,
                      pline1=add2.Line1, pline2=add2.Line2, plandmark=add2.Landmark, pcity=add2.City, pcust_state=add2.CustState, ppin_code=add2.PinCode}).ToList();

            return Ok(res);
        }
        #endregion



        // UPDATE: After Approval - Change status, insert in account details, create mail of initial password
        #region after-approval
        [HttpPost]
        [Route("after-approval")]
        public IActionResult ChangeStatus([FromQuery(Name = "custid")] int? custId)
        {
            try
            {
                // change status
                CustomerDetail c = db.CustomerDetails.Where(cu => cu.CustId == custId).FirstOrDefault();
                c.ApprovalStatus = "Approved";
                db.SaveChanges();

                //insert in account details
                AccountDetail ac = new AccountDetail();
                ac.AccountType = "Savings";
                ac.CustId = custId;

                string log_password = RandomString(9, false);
                // Encryption of login password
                byte[] encDataLogin_byte = new byte[log_password.Length];
                encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(log_password);
                string encodedLoginPassword = Convert.ToBase64String(encDataLogin_byte);

                ac.LoginPassword = encodedLoginPassword;


                string trans_password = RandomString(9, false);
                // Encryption of transaction password
                byte[] encDataTrans_byte = new byte[trans_password.Length];
                encDataTrans_byte = System.Text.Encoding.UTF8.GetBytes(trans_password);
                string encodedTransactionPassword = Convert.ToBase64String(encDataTrans_byte);

                ac.TransactionPassword = encodedTransactionPassword;

                DateTime created_on = DateTime.Now;
                ac.CreatedOn = created_on;
                ac.LastLogin = null;
                ac.AccountStatus = "unlocked";
                ac.CurrentBalance = 25000;

                Random rand = new Random();
                int userId = rand.Next(1000000, 9999999);
                ac.UserId = userId;


                db.AccountDetails.Add(ac);

                db.SaveChanges();

                // send email with default credentials
                if (SendEmailPassword(c.Email, RandomString(8, false), RandomString(8, false), ac.AccountNumber, ac.UserId))
                {
                    return Ok("process completed");
                }
                else
                {
                    return Ok("process not completed");
                }
            }
            catch(Exception e)
            {
                return BadRequest("exception");
            }

            return BadRequest("error");
        }
        #endregion


        // REJECT 
        #region after-reject
        [HttpPut]
        [Route("after-rejection")]
        public IActionResult AfterRejection([FromQuery(Name = "custid")] int? custId)
        {
            try
            {
                // change status
                CustomerDetail c = db.CustomerDetails.Where(cu => cu.CustId == custId).FirstOrDefault();
                c.ApprovalStatus = "Rejected";
                db.SaveChanges();

                // send email with default credentials
                if (SendEmailRejection(c.Email))
                {
                    return Ok("rejected approval");
                }
                else
                {
                    return Ok("process not completed");
                }
            }
            catch (Exception e)
            {
                return BadRequest("exception");
            }
            return BadRequest("error");
        }
        #endregion


        // UPDATE: Track Status
        #region track-status
        [HttpGet]
        [Route("track-status")]
        public IActionResult TrackApprovalStatusAsync([FromQuery(Name = "srn")] int SRN)
        {
            try
            {
                Approval app = db.Approvals.Where(a => a.Srn == SRN).FirstOrDefault();
                if (app == null)
                {
                    return BadRequest("invalid srn");
                }
                
                int? custId = app.CustId;
                CustomerDetail c = db.CustomerDetails.Where(c => c.CustId == custId).FirstOrDefault();

                string status = c.ApprovalStatus;
                string to = c.Email;

                int otp = SendEmailOtp(to);

                return Ok(new { otp = otp, status = status});
            }
            catch (Exception e)
            {
                BadRequest("exception");
            }
            return BadRequest("error in sending mail");
        }
        #endregion



        //VIEW CUSTOMER DETAILS - Display particular customer details
        #region display user-details
        [HttpGet]
        [Route("user-details")]
        public IActionResult ViewCustomerDetails([FromQuery(Name = "custid")] long custId)
        {
            List<CustAddress> addList = db.CustAddresses.Where(cu => cu.CustId == custId).ToList();
            CustAddress add1 = null; CustAddress add2 = null;
            if (addList.Count == 2)
            {
                add1 = addList[0];
                add2 = addList[1];
            }
            else
            {
                add1 = addList[0];
                add2 = addList[0];
            }

            var res = from c in db.CustomerDetails
                      where c.CustId == custId
                      select new
                      {
                          cust_id = c.CustId,
                          title = c.Title,
                          first_name = c.FirstName,
                          middle_name = c.MiddleName,
                          last_name = c.LastName,
                          father_name = c.FathersName,
                          gender = c.Gender,
                          mobile_number = c.MobileNumber,
                          email = c.Email,
                          aadhar = c.Aadhar,
                          pan_card = c.PanCard,
                          pan_doc = c.PanDoc,
                          dob = c.Dob,
                          Occupation_type = c.OccupationType,
                          source_of_income = c.SourceOfIncome,
                          gross_annual_income = c.GrossAnnualIncome,
                          debit_card = c.DebitCard,
                          net_banking = c.NetBanking,
                          approval_status = c.ApprovalStatus,
                          line1 = add1.Line1,
                          line2 = add1.Line2,
                          landmark = add1.Landmark,
                          city = add1.City,
                          cust_state = add1.CustState,
                          pin_code = add1.PinCode,
                          pline1 = add2.Line1,
                          pline2 = add2.Line2,
                          plandmark = add2.Landmark,
                          pcity = add2.City,
                          pcust_state = add2.CustState,
                          ppin_code = add2.PinCode
                      };
            return Ok(res);
        }
        #endregion



        // EMAIL - OTP
        #region send OTP via email
        private int SendEmailOtp(string to)
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

            string subject = "Status Tracking";
            Random rand = new Random();
            int otp = rand.Next(100000, 999999);
            string body = "OTP for approval status tracking is " + otp;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return otp;
            }
            catch(Exception e)
            {
                return -1;
            }
        }
        #endregion



        //EMAIL - INTITAL PASSWORD
        #region send approval via email
        private bool SendEmailPassword(string to, string lp, string tp, long ac, long uid)
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

            string subject = "Default Credentials for Net Banking";
            string body = "Account Number : "+ac+" | UserID : "+uid+". You can now register for Internet Banking";
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion


        //EMAIL - REJECTION
        #region send rejection via email
        private bool SendEmailRejection(string to)
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

            string subject = "Account Approval Rejected";
            string body = "Admin has rejected account approval status";
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion



        //RANDOM PASSWORD GENERATOR
        #region method for random password generator
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        #endregion

    }
}
