using FluentValidation;
using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using LoginAppDemo.Services.Interface;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using LoginAppDemo.Security;
using AutoMapper;
using LoginAppDemo.Helper;

namespace LoginAppDemo.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IValidator<UserModel> _userValidator;
        private readonly IValidator<UserLoginModel> _userLoginValidator;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IEmailService emailService, IValidator<UserModel> userValidator, IValidator<UserLoginModel> userLoginValidator, IMapper mapper)
        {
            _userService = userService;
            _emailService = emailService;
            _userValidator = userValidator;
            _userLoginValidator = userLoginValidator;
            _mapper = mapper;
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel userModel)
        {
            var validationResult = _userValidator.Validate(userModel);
            var emailExists = _userService.CheckEmailExists(userModel.Email);
            var userNameExists = _userService.CheckUserNameExists(userModel.UserName);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(userModel);
            }
            if (userNameExists)
            {
                ModelState.AddModelError("UserName", "Username already exists");
                return View(userModel);
            }
            if (!validationResult.IsValid)
            {
                return View(userModel);
            }

            var result = _userService.Create(userModel);
        
            if (!result.IsSuccessfull) return View(userModel);
            _emailService.SendEmail(userModel.Email);
            return RedirectToAction("WaitingPage", "Home",new { email = userModel.Email});
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel userModel)
        {
            var validationResult = _userLoginValidator.Validate(userModel);

            if (validationResult.IsValid)
            {
                var user = _userService.GetUserByUserName(userModel.UserName);
                
                var crypto = new SimpleCrypto.PBKDF2(); 
                if (user != null && user.Password == crypto.Compute(userModel.Password, user.PasswordSalt))
                {
                    var roles = user.UserRoles.Select(m => m.Role.RoleName).ToArray();
                    var person = user.Persons.FirstOrDefault();
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.id;
                    serializeModel.FirstName = person?.FirstName ?? "";
                    serializeModel.LastName = person?.LastName ?? "";
                    serializeModel.roles = roles;

                    string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false, //pass here true, if you want to implement remember me functionality
                    userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    var personModel = _mapper.Map<Person, PersonModel>(person);
                    return person != null ? RedirectToAction("ViewProfile", "Profile") :
                                            RedirectToAction("EditProfile", "Profile");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Username/Password is incorrect");
                    return View();
                }

            }
            return RedirectToAction("Index", "Profile");
        }

    }
}