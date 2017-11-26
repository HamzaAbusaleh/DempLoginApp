using FluentValidation;
using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using LoginAppDemo.Security;
using LoginAppDemo.Services.Interface;
using System;
using System.Web.Mvc;
namespace LoginAppDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserModel> _userValidator;
        public HomeController(IUserService userService, IValidator<UserModel> userValidator)
        {
            _userService = userService;
            _userValidator = userValidator;
        }

        
        public ActionResult Index()
        {
            var a = new  UserModel();
          
            return View();
        }

        public ActionResult WaitingPage(string email)
        {
            ViewBag.Text = email;
            return View();
        }

    }
}