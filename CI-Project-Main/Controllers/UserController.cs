using CI_Project_Main.Models;
using CI_Project_Main.Models.Data;
using CI_Project_Main.Models.RegistrationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CI_Project_Main.Controllers
{
    public class UserController : Controller
    {

        private readonly CIPContext _db;

        public UserController(CIPContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            User user = new User();
            HttpContext.Session.Clear();
            return View(user);
        }

        //post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var isLoggedIn = await _db.Users.FirstOrDefaultAsync(m => m.Email == Email && m.Password == Password);

                var userFirstName = Email.Split("@")[0];
                if (isLoggedIn == null)
                {
                    //means no user found so register first
                    ViewBag.LoginStatus = 0;
                    return RedirectToAction("Index", "User");
                }

                else
                {
                    //means user is already there so now login successfull redirect to landing page

                    //now creating a session, we will destroy it when user presses sign out
                    HttpContext.Session.SetString("userID", userFirstName);
                    //HttpContext.Session.SetString("userName", FirstName);
                    return RedirectToAction("PlatformLanding", "User");


                }
            }




            return View();


        }



        public IActionResult Registration()
        {

            return View();
        }


        //post

        [HttpPost]
        public async Task<IActionResult> Registration(Registration user)
        {



            if (user.Password == user.ConfirmPassword)
            {
                //checking if user exist? if yes then rediret to login
                var isEmailAlreadyExists = _db.Users.Any(x => x.Email == user.Email);

                if (isEmailAlreadyExists)
                {
                    //ViewBag.userAlreadyThere = 1;
                    ModelState.AddModelError("Email", "User with this email already exists");

                }

                else
                {
                    //ViewBag.userAlreadyThere = 0;
                    var userData = new User()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Password = user.Password

                    };


                    await _db.Users.AddAsync(userData);
                    await _db.SaveChangesAsync();
                    ViewBag.status = 1;
                    return RedirectToAction("Index", "User");

                }





            }

            else
            {
                ModelState.AddModelError("ConfirmPassword", "Password does not match");
            }

            return View();
        }




        //get for forgot

        public IActionResult ForgotPassword()
        {
            User user = new User();
            return View(user);
        }

        //post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ForgotPassword(string Email)
        {

            if (ModelState.IsValid)
            {
                var isEmailExist = _db.Users.FirstOrDefault(x => x.Email == Email);
                if (isEmailExist == null)
                {
                    TempData["Error"] = "Email does not exist";
                    return RedirectToAction("ForgotPassword", "User");
                }

                else

                {
                    //generating token
                    var token = Guid.NewGuid().ToString();

                    PasswordReset passwordReset = new PasswordReset();

                    var passwordResetData = new PasswordReset
                    {
                        Email = Email,
                        Token = token
                    };

                    _db.PasswordReset.Add(passwordResetData);
                    _db.SaveChanges();

                    var resetLink = Url.Action("ResetPassword", "User", new { email = Email, token }, Request.Scheme);




                    //mail generating

                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    msg.From = new MailAddress("");
                    msg.To.Add(new MailAddress(Email));
                    msg.Subject = "Test";
                    msg.IsBodyHtml = true; //to make message body as html
                    msg.Body = $"Hey user your link is ready : click the <a href={resetLink}>{resetLink}</a> to reset password";
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("", "");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(msg);

                    TempData["mailSent"] = "Mail sent";



                }

            }
            return View();
        }


        //get for reset

        public IActionResult ResetPassword()
        {
            User user = new User();
            return View(user);
        }


        [HttpGet]

        public ActionResult ResetPassword(string email, string token)
        {
            var passwordReset = _db.PasswordReset.FirstOrDefault(pr => pr.Email == email && pr.Token == token);
            if (passwordReset == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Pass the email and token to the view for resetting the password
            var model = new ResetPassword
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword resetPasswordView)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = _db.Users.FirstOrDefault(u => u.Email == resetPasswordView.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgetPassword", "Home");
                }

                // Find the password reset record by email and token
                var passwordReset = _db.PasswordReset.FirstOrDefault(pr => pr.Email == resetPasswordView.Email && pr.Token == resetPasswordView.Token);
                if (passwordReset == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Update the user's password
                user.Password = resetPasswordView.Password;
                _db.SaveChanges();




            }

            return View(resetPasswordView);
        }

        //method to get list of missions

        //public List<Mission> GetMissions(int? pageNumber)
        //{
        //int pageSize = 5;
        //List<Mission> missions = _db.Missions.ToList();
        //List<Mission> missions = _db.Missions.Where(x => x.Title.ToLower().Contains(query.ToLower())).ToList();
        //return PaginationList<Mission>.Create(_db.Missions.ToList(), pageNumber ?? 1, pageSize);
        //}

        //public User GetUser()
        //{
        //    User user = new User();
        //    return user;
        //}

        public IActionResult NoMissionFound()
        {
            return View();
        }

        //get method

        public IActionResult PlatformLanding(int? pageNumber, string? searchTerm, int? sort)
        {


            //dynamic myModel = new ExpandoObject();
            //myModel.Mission = GetMissions(pageNumber);
            //myModel.User = GetUser();


            //getting items in filter dropdowns

            var cities = _db.Cities.ToList();
            ViewBag.cityItem = cities;

            var countries = _db.Countries.ToList();
            ViewBag.countryItem = countries;

            var themes = _db.MissionThemes.ToList();
            ViewBag.themeItem = themes;

            var skills = _db.Skills.ToList();
            ViewBag.skillItem = skills;


            var loginSession = HttpContext.Session.GetString("userID");

            if (loginSession == null)
            {
                return RedirectToAction("Index", "User");
            }

            else
            {
                int pageSize = 9;
                //List<Mission> missions = _db.Missions.ToList();

                if (searchTerm == null)
                {

                    if (sort == 1)
                    {
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderByDescending(m => m.Title).ToList(), pageNumber ?? 1, pageSize));
                    }

                    if (sort == 2)
                    {
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderBy(m => m.Title).ToList(), pageNumber ?? 1, pageSize));

                    }


                    return View(PaginationList<Mission>.Create(_db.Missions.ToList(), pageNumber ?? 1, pageSize));


                }

                else
                {


                    var isFound = _db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()));
                    //var isShortDescFound = _db.Missions.Where(m => m.ShortDescription.ToLower().Contains(searchTerm.ToLower()));
                    if (isFound.Any())
                    {

                        if (sort == 1)
                        {
                            return View(PaginationList<Mission>.Create(_db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower())).OrderByDescending(m => m.Title).ToList(), pageNumber ?? 1, pageSize));
                        }

                        if (sort == 2)
                        {
                            return View(PaginationList<Mission>.Create(_db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower())).OrderBy(m => m.Title).ToList(), pageNumber ?? 1, pageSize));

                        }

                        return View(PaginationList<Mission>.Create(_db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower())).ToList(), pageNumber ?? 1, pageSize));
                    }

                    else
                    {
                        return RedirectToAction("NoMissionFound", "User");
                    }


                }




            }





        }



    }

}



