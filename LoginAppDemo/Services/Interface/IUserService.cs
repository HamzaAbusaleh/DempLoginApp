using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Services.Interface
{
    public interface IUserService
    {
        UserModel GetUserById(long id);
        Result Create(UserModel user);
        bool CheckEmailExists(string email);
        bool CheckUserNameExists(string userName);
        User GetUserEntityById(long id);
        User GetUserByUserName(string userName);
    }
}