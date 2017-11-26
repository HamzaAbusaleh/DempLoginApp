using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Services.Interface
{
    public interface IPersonService
    {
        PersonModel GetProfileByUserId(long userId);
        Result Create(PersonModel personModel, long userId);
    }
}