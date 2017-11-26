using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LoginAppDemo.Models.ViewModel
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string ContactNo { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}