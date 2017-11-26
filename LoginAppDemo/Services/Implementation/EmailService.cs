using LoginAppDemo.Helper;
using LoginAppDemo.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Services.Implementation
{
    public class EmailService : IEmailService
    {

        public void SendEmail(string email)
        {
            MailHelper.SendEmail(email, "", "Thanks for registering, please click this link to login <a href='http://localhost:60844/Account/Login'>Login</a>");
        }
    }
}