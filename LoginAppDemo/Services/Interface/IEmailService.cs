using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(string email);
    }
}