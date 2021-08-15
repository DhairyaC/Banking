using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UserController : ControllerBase
    {
        private readonly BankingContext db;

        public UserController(BankingContext context)
        {
            db = context;
        }

        // USER LOGIN
        #region user login
        [HttpGet]
        [Route("login")]
        public IActionResult UserLogin([FromQuery(Name = "userid")] long uId, [FromQuery(Name = "password")] string password)
        {
            try
            {
                AccountDetail ad = db.AccountDetails.Where(u => u.UserId == uId).FirstOrDefault();
                if (ad == null)
                {
                    return Ok("invalid user id");
                }
                if (ad.AccountStatus == "locked")
                {
                    return Ok("account locked");
                }

                //Decryption of login password
                string pwd = db.AccountDetails.Where(a => a.UserId == uId).Select(a => a.LoginPassword).ToList()[0];

                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(pwd);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string decrypt_login_password = new String(decoded_char);

                if (decrypt_login_password == password)
                {
                    return Ok("login success");
                }

                //if (ad.LoginPassword == password)
                //{
                //    return Ok("login success");
                //}
                else
                {
                    return Ok("wrong password");
                }
            }
            catch(Exception e)
            {
                return Ok("invalid user id");
            }   
        }
        #endregion


        // USER FORGOT ID
        #region forgot-id
        [HttpGet]
        [Route("forgot-id")]
        public IActionResult ForgotId([FromQuery(Name ="accountnumber")] long ac)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.AccountNumber == ac).FirstOrDefault();
            if(ad == null)
            {
                return Ok("wrong account number");
            }

            long userId = ad.UserId;

            long? custId = ad.CustId;
            CustomerDetail cd = db.CustomerDetails.Where(cu => cu.CustId == custId).FirstOrDefault();
            string to = cd.Email;

            int otp = SendEmailOtp(to, userId);
            return Ok(new { otp = otp, custid = custId });
        }
        #endregion

        #region send OTP via mail for forgot user-id
        private int SendEmailOtp(string to, long userId)
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

            string subject = "Forgot User ID";
            Random rand = new Random();
            int otp = rand.Next(100000, 999999);
            string body = "OTP for forgot ID is " + otp;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return otp;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        #endregion

        #region send new user-id via mail
        [HttpGet]
        [Route("receive-userid")]
        public IActionResult SendMailForUserID([FromQuery(Name ="accountnumber")] long ac)
        {
            AccountDetail a = db.AccountDetails.Where(ad => ad.AccountNumber == ac).FirstOrDefault();
            long userId = a.UserId;

            CustomerDetail c = db.CustomerDetails.Where(c => c.CustId == a.CustId).FirstOrDefault();
            string to = c.Email;

            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("dashbank7@gmail.com", "Dash@1399"),
            };

            string subject = "User ID Recover";
            
            string body = "User ID is " + userId;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return Ok("userid recoverd");
            }
            catch (Exception e)
            {
                return Ok("exception");
            }
        }
        #endregion


        // USER FORGOT
        #region send OTP via mail for forot password
        [HttpGet]
        [Route("forgot-password")]
        public IActionResult ForgotPassword([FromQuery(Name ="userid")] long userId)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null)
            {
                return Ok("invalid userid");
            }
            CustomerDetail cd = db.CustomerDetails.Where(c => c.CustId == ad.CustId).FirstOrDefault();
            string to = cd.Email;

            int otp = SendOtpForgot(to);

            return Ok(otp);
        }
        #endregion


        private int SendOtpForgot(string to)
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

            string subject = "Forgot User Credential";
            Random rand = new Random();
            int otp = rand.Next(100000, 999999);
            string body = "OTP  " + otp;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return otp;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        #region set new login password
        [HttpPut]
        [Route("set-password")]
        public IActionResult SetPassword([FromQuery(Name ="userid")] long userId, [FromQuery(Name ="p1")] string p1, [FromQuery(Name ="p2")] string p2)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if(p1 == p2)
            {
                string new_login_pwd = p1;

                byte[] encDataLogin_byte = new byte[new_login_pwd.Length];
                encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(new_login_pwd);
                string encodedNewLoginpassword = Convert.ToBase64String(encDataLogin_byte);

                ad.LoginPassword = encodedNewLoginpassword;
            }
            else
            {
                return BadRequest("not matched");
            }

            ad.AccountStatus = "unlocked";
            db.SaveChanges();
            return Ok("set");
        }
        #endregion


        //ACCOUNT SUMMARY
        #region account summary
        [HttpGet]
        [Route("account-summary")]
        public IActionResult AccountSummary([FromQuery(Name = "userid")] long userId)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null)
            {
                return BadRequest("invalid userid");
            }
            List<Transaction> tr = db.Transactions.Where(t => t.FromAccNum == ad.AccountNumber).ToList();

            long acNo = ad.AccountNumber;
            decimal bal = ad.CurrentBalance;

            var res = new { accountnumber = acNo, balance=bal, transactions=tr};

            return Ok(res);
        }
        #endregion


        // LOCK ACCOUNT
        #region lock-account
        [HttpPut]
        [Route("lock-account")]
        public IActionResult LockAccount([FromQuery(Name ="userid")] long userId)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            ad.AccountStatus = "locked";
            db.SaveChanges();
            return Ok("account locked");
        }
        #endregion


        // ADD BENEFICIARIES
        #region add beneficiaries
        [HttpPut]
        [Route("add-beneficiaries")]
        public IActionResult AddBeneficiaries([FromQuery(Name ="userid")] long userId, [FromQuery(Name ="name")] string name, [FromQuery(Name ="b1")] long b1, [FromQuery(Name ="b2")] long b2)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            long from = ad.AccountNumber;

            
            if(b1 != b2)
            {
                return Ok("not matched");
            }
            long to = b1;

            // check if beneficiary is already present
            Beneficiary bb = db.Beneficiaries.Where(b => b.BAccountNumber == b1 && b.AccountNumber == from).FirstOrDefault();
            if (bb != null) return Ok("already present");

            Beneficiary b = new Beneficiary();
            b.AccountNumber = from;
            b.BAccountNumber = to;
            b.Nickname = name;
            

            db.Beneficiaries.Add(b);
            db.SaveChanges();

            return Ok("added");
        }
        #endregion


        // GET LIST OF BENEFICIARIES
        #region list-of-beneficiaries
        [HttpGet]
        [Route("get-beneficiaries")]
        public IActionResult GetBeneficiaries([FromQuery(Name ="userid")] long userId)
        {
            AccountDetail ad = db.AccountDetails.Where(u => u.UserId == userId).FirstOrDefault();
            long acNo = ad.AccountNumber;

            List<Beneficiary> bList = db.Beneficiaries.Where(b => b.AccountNumber == acNo).ToList();
            return Ok(bList);
        }
        #endregion


        // TRANSACTION
        #region fund-transfer
        [HttpPost]
        [Route("transaction")]
        public IActionResult Transaction([FromQuery(Name ="userid")] long userId, [FromQuery(Name ="amount")] int amount, 
            [FromQuery(Name ="to")] long toAc, [FromQuery(Name ="mode")] string mode, [FromQuery(Name ="remark")] string? remark)
        {
            try
            {
                AccountDetail from = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
                if (from.CurrentBalance < amount)
                {
                    return Ok("insufficient funds");
                }

                AccountDetail to = db.AccountDetails.Where(a => a.AccountNumber == toAc).FirstOrDefault();

                // update balance after transaction
                from.CurrentBalance -= amount;
                to.CurrentBalance += amount;

 
                // insert record corresponding to paid person
                Transaction too = new Transaction();
                too.Mode = mode;
                //swap
                too.FromAccNum = toAc;
                too.PaidToAccNum = from.AccountNumber;
                too.Amount = amount;
                too.TranStatus = "Credited";
                too.Remark = remark;
                DateTime now1 = DateTime.Now;
                too.TransTime = now1;
                db.Transactions.Add(too);

                db.SaveChanges();

                // insert record corresponding to one who paid
                Transaction fr = new Transaction();
                fr.Mode = mode;
                fr.PaidToAccNum = toAc;
                fr.FromAccNum = from.AccountNumber;
                fr.Amount = -amount;
                fr.TranStatus = "Debited";
                fr.Remark = remark;
                DateTime now = DateTime.Now;
                fr.TransTime = now;
                db.Transactions.Add(fr);
                db.SaveChanges();

                // return Ok("done");
                Transaction transaction = (from t in db.Transactions orderby t.ReferenceId descending select t).FirstOrDefault();
                return Ok(transaction);

            }
            catch(Exception e)
            {
                return BadRequest("Exception");
            }
        }
        #endregion


        // CHANGE PASSWORD - INSIDE DASHBOARD
        #region change-password
        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangeAddress([FromQuery(Name ="userid")] long userId, [FromQuery(Name = "lp")] string lp, [FromQuery(Name ="clp")] string clp, [FromQuery(Name ="tp")] string tp,
            [FromQuery(Name ="ctp")] string ctp)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null)
            {
                return BadRequest("wrong userid");
            }
            if (lp != clp)
            {
                return BadRequest("login password not matched");
            }
            else if(tp != ctp)
            {
                return BadRequest("transaction password not matched");
            }


            // update details if same
            string l_pwd = clp;
            // Encryption of password
            byte[] encDataLogin_byte = new byte[l_pwd.Length];
            encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(l_pwd);
            string encodedLoginpassword = Convert.ToBase64String(encDataLogin_byte);

            ad.LoginPassword = encodedLoginpassword;

            string t_pwd = ctp;
            // Encryption of password
            byte[] encDataTrans_byte = new byte[t_pwd.Length];
            encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(t_pwd);
            string encodedTranspassword = Convert.ToBase64String(encDataLogin_byte);

            ad.TransactionPassword = encodedTranspassword;

            db.SaveChanges();
            return Ok("changed");
        }
        #endregion


        // REGISTER - INTERNET BANKING
        #region internet banking registration
        [HttpGet]
        [Route("register-ib")]
        public IActionResult RegisterBanking([FromQuery(Name = "accnum")] long acNo, [FromQuery(Name = "lp")] string lp,
            [FromQuery(Name = "clp")] string clp, [FromQuery(Name = "tp")] string tp, [FromQuery(Name = "ctp")] string ctp)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.AccountNumber == acNo).FirstOrDefault();
            if (ad == null)
            {
                return Ok("invalid accnum");
            }

            //Decryption of login password

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(lp);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decrypt_login_password = new String(decoded_char);

            //Decryption of transaction password

            System.Text.UTF8Encoding encoders = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decodes = encoder.GetDecoder();
            byte[] todecode_bytes = Convert.FromBase64String(tp);
            int charCounts = utf8Decodes.GetCharCount(todecode_bytes, 0, todecode_bytes.Length);
            char[] decoded_chars = new char[charCounts];
            utf8Decode.GetChars(todecode_bytes, 0, todecode_bytes.Length, decoded_chars, 0);
            string decrypt_transaction_password = new String(decoded_chars);

            if (decrypt_login_password != clp)
            {
                return Ok("login password didn't matched");
            }
            else if (decrypt_transaction_password != ctp)
            {
                return Ok("transaction password didn't matched");
            }
            else
            {
                ad.LoginPassword = clp;
                ad.TransactionPassword = ctp;
                db.SaveChanges();
                return Ok("set");
            }
        }
        #endregion


        // GET OTP - REGISTER INTERNET BANKING
        #region send OTP via email for internet banking
        [HttpGet]
        [Route("register-ib-otp")]
        public IActionResult RegisterIbOtp([FromQuery(Name ="accnum")] long acNo)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.AccountNumber == acNo).FirstOrDefault();
            if (ad == null) return BadRequest("invalid accnum");
            CustomerDetail cd = db.CustomerDetails.Where(c => c.CustId == ad.CustId).FirstOrDefault();
            string to = cd.Email;

            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("dashbank7@gmail.com", "Dash@1399"),
            };

            string subject = "Register Internet Banking";
            Random rand = new Random();
            int otp = rand.Next(100000, 999999);
            string body = "OTP  " + otp;
            try
            {
                email.Send("dashbank7@gmail.com", to, subject, body);
                return Ok(otp);
            }
            catch (Exception e)
            {
                return BadRequest("otp not sent");
            }
        }
        #endregion


        // GET USER DETAILS
        #region user details
        [HttpGet]
        [Route("fetch-details")]
        public IActionResult FetchUserDetails([FromQuery(Name ="userid")] long id)
        {
            /*AccountDetail ad = db.AccountDetails.Where(u => u.UserId == id).FirstOrDefault();
            if (ad == null) return BadRequest("invalid userid");
            long? custId = ad.CustId;

            CustomerDetail cd = db.CustomerDetails.Where(cu => cu.CustId == custId).FirstOrDefault();
            if (cd == null) return BadRequest("invalid custid");*/

            var res = from a in db.AccountDetails
                      join c in db.CustomerDetails
                      on a.CustId equals c.CustId
                      join ad in db.CustAddresses
                      on c.CustId equals ad.CustId
                      where a.UserId == id
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
                          dob=c.Dob,
                          Occupation_type = c.OccupationType,
                          source_of_income = c.SourceOfIncome,
                          gross_annual_income = c.GrossAnnualIncome,
                          debit_card = c.DebitCard,
                          net_banking = c.NetBanking,
                          approval_status = c.ApprovalStatus,
                          line1 = ad.Line1,
                          line2 = ad.Line2,
                          landmark = ad.Landmark,
                          city = ad.City,
                          cust_state = ad.CustState,
                          pin_code = ad.PinCode,
                          pline1 = ad.Line1,
                          pline2 = ad.Line2,
                          plandmark = ad.Landmark,
                          pcity = ad.City,
                          pcust_state = ad.CustState,
                          ppin_code = ad.PinCode
                      };

            return Ok(res);
        }
        #endregion


        // GET NAME
        #region get-name on dashboard
        [HttpGet]
        [Route("get-name")]
        public IActionResult GetName([FromQuery(Name ="userid")] long userId)
        {
            try
            {
                AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
                if (ad == null)
                {
                    return NotFound("account not found");
                }
                int? custId = ad.CustId;
                CustomerDetail cd = db.CustomerDetails.Where(c => c.CustId == custId).FirstOrDefault();
                string firstName = cd.FirstName;
                string lastName = cd.LastName;

                return Ok(firstName+" "+lastName);
            }
            catch(Exception e)
            {
                return BadRequest("exception");
            }
        }
        #endregion


        // GET ACCOUNT STATEMENT INFO
        #region account statement
        [HttpGet]
        [Route("get-info")]
        public IActionResult GetInfo([FromQuery(Name ="userid")] long userId)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null)
            {
                return Ok("Account not found");
            }
            long acNo = ad.AccountNumber;
            CustomerDetail cd = db.CustomerDetails.Where(c => c.CustId == ad.CustId).FirstOrDefault();
            if (cd == null)
            {
                return Ok("Customer not found");
            }
            string name = cd.FirstName+" "+cd.LastName;
            string type = ad.AccountType;
            decimal balance = ad.CurrentBalance;

            return Ok(new { acno = acNo, name = name, type = type, balance = balance });
        }
        #endregion


        // GET TRANSACTION LIST
        #region transaction-list
        [HttpGet]
        [Route("get-tr-range")]
        public IActionResult GetTrRange([FromQuery(Name ="acno")] long acNo, [FromQuery(Name ="from")] DateTime from, [FromQuery(Name ="to")] DateTime to)
        {
            /*var res = (from t in db.Transactions
                       where t.FromAccNum == acNo && t.TransTime >= Convert.ToDateTime(from) && Convert.ToDateTime(to)
                       select t).ToList();*/

            return Ok(db.Transactions.Where(t => t.FromAccNum == acNo && (t.TransTime >= from && t.TransTime <= to)).ToList());
        }
        #endregion


        // FETCH ADDRESS IN DASHBOARD
        #region fetch customer address 
        [HttpGet]
        [Route("fetch-address")]
        public IActionResult FetchAddress([FromQuery(Name ="userid")] long userId)
        {
            try
            {
                AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
                long? custId = ad.CustId;

                // fetch address
                List<CustAddress> ca = db.CustAddresses.Where(c => c.CustId == custId).ToList();
                return Ok(ca);

            }
            catch(Exception e)
            {
                return BadRequest("Exception");
            }
        }
        #endregion


        // UPDATE BASIC DETAILS
        #region fetch customer details
        [HttpPut]
        [Route("update-basic")]
        public IActionResult UpdateBasicDetails([FromQuery(Name ="id")]int id, [FromQuery(Name ="title")] string title,
            [FromQuery(Name ="fname")] string fname, [FromQuery(Name ="lname")] string lname, [FromQuery(Name ="mname")] string mname,
            [FromQuery(Name ="faname")] string faname, [FromQuery(Name ="mnum")] string mnum, [FromQuery(Name ="email")] string email,
            [FromQuery(Name ="aadhar")] long aadhar, [FromQuery(Name ="pan")] string pan, [FromQuery(Name ="occtype")] string occtype,
            [FromQuery(Name ="incsource")] string incsource, [FromQuery(Name ="gai")] string gai)
        {
            try
            {
                CustomerDetail cd = db.CustomerDetails.Where(c => c.CustId == id).FirstOrDefault();
                if (cd == null) return Ok("Customer not found");

                // make changes
                cd.Title = title;
                cd.FirstName = fname;
                cd.LastName = lname;
                cd.MiddleName = mname;
                cd.FathersName = faname;
                cd.MobileNumber = mnum;
                cd.Email = email;
                cd.Aadhar = aadhar;
                cd.PanCard = pan;
                cd.OccupationType = occtype;
                cd.SourceOfIncome = incsource;
                cd.GrossAnnualIncome = gai;

                db.SaveChanges();

                return Ok("updated");

            }
            catch(Exception e)
            {
                return BadRequest("Exception");
            }
        }
        #endregion


        // UPDATE ONLY LOGIN PASSWORD
        #region update login-password
        [HttpPost]
        [Route("only-login-password")]
        public IActionResult ChangeOnlyLogin([FromQuery(Name = "userid")] long userId, [FromQuery(Name = "pass1")] string pass1, [FromQuery(Name = "pass2")] string pass2)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null)
            {
                return Ok("not found");
            }

            if (pass1 != pass2)
            {
                return Ok("password didn't matched");
            }
            else
            {
                string login_pwd = pass2;
                // Encryption of login password
                byte[] encDataLogin_byte = new byte[login_pwd.Length];
                encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(login_pwd);
                string encodedLoginpassword = Convert.ToBase64String(encDataLogin_byte);

                ad.LoginPassword = encodedLoginpassword;

                db.SaveChanges();

                return Ok("set");
            }
        }
        #endregion


        // UPDATE ONLY TRANSACTION PASSWORD
        #region update tranaction-password
        [HttpPost]
        [Route("only-transaction-password")]
        public IActionResult ChangeOnlyTransaction([FromQuery(Name = "userid")] long userId, [FromQuery(Name = "pass1")] string pass1, [FromQuery(Name = "pass2")] string pass2)
        {
            AccountDetail ad = db.AccountDetails.Where(a => a.UserId == userId).FirstOrDefault();
            if (ad == null) return Ok("not found");
            if (pass1 != pass2) return Ok("password didn't matched");
            else
            {
                string trans_pwd = pass2;
                // Encryption of transaction password
                byte[] encDataLogin_byte = new byte[trans_pwd.Length];
                encDataLogin_byte = System.Text.Encoding.UTF8.GetBytes(trans_pwd);
                string encodedTranspassword = Convert.ToBase64String(encDataLogin_byte);

                ad.TransactionPassword = encodedTranspassword;

                db.SaveChanges();
                return Ok("set");
            }
        }
        #endregion



    }


}
