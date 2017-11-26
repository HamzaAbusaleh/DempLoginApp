using FluentValidation;
using LoginAppDemo.Models.ViewModel;
using LoginAppDemo.Security;
using LoginAppDemo.Services.Interface;
using System.Web.Mvc;

namespace LoginAppDemo.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class ProfileController : BaseController
    {
        public readonly IPersonService _personService;

        private readonly IValidator<PersonModel> _personValidator;
        public ProfileController(IPersonService personService, IValidator<PersonModel> personValidator)
        {
            _personService = personService;
            _personValidator = personValidator;
        }

     
        public ActionResult EditProfile()
        {
            var person = _personService.GetProfileByUserId(User.UserId);
            return View(person);
        }

        public ActionResult ViewProfile(PersonModel person = null)
        { 
            
                person = _personService.GetProfileByUserId(User.UserId);
            
            return View(person);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(PersonModel person)
        {
            if (_personValidator.Validate(person).IsValid)
            {
                var result = _personService.Create(person, User.UserId);
                return RedirectToAction("ViewProfile", "Profile");
            }
            return View();
        }
    }
}