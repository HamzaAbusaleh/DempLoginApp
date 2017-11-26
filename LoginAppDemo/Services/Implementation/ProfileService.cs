using LoginAppDemo.Services.Interface;
using System;
using System.Linq;
using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using LoginAppDemo.Repository.Interface;
using AutoMapper;
using System.Data.Entity.Validation;
using System.Collections.Generic;

namespace LoginAppDemo.Services.Interface
{
    public class PersonService : IPersonService
    {
        private readonly IGenericRepository _repo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public PersonService(IGenericRepository repo, IMapper mapper, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
            _mapper = mapper;
        }
 
        
        public PersonModel GetProfileByUserId(long userId)
        {
            var user = _repo.GetFirst<User>(e => e.id == userId);
            var person = user.Persons.FirstOrDefault();
            person = person != null ? person : new Person();
            var personModel = person != null ?  _mapper.Map<Person, PersonModel>(person) : new PersonModel();
            
            personModel.CompanyName = user.CompanyName;
            return personModel;
        }
   
        public Result Create(PersonModel personModel,long userId)
        {
            var result = new Result();
            try
            {
                var oldPerson  = _userService.GetUserEntityById(userId).Persons.FirstOrDefault();
              
                var person = _mapper.Map<PersonModel, Person>(personModel);
                person.UserId = userId;
                var address = person.Addresses.FirstOrDefault();
                person.EmployeedFrom = DateTime.Now;

                if(oldPerson != null)
                {
                    oldPerson.Birthdate = personModel.Birthdate;
                    oldPerson.Department = personModel.Department;
                    oldPerson.EmployeedFrom = personModel.EmployeedFrom;
                    oldPerson.EmployeedTo = personModel.EmployeedTo;
                    oldPerson.FirstName = personModel.FirstName;
                    oldPerson.LastName = personModel.LastName;
                    oldPerson.Gender = personModel.Gender;
                    oldPerson.LeaveReason = personModel.LeaveReason;
                    oldPerson.Position = personModel.Position;

                    oldPerson.Addresses.FirstOrDefault().Addressline1 = personModel.Address.Addressline1;
                    oldPerson.Addresses.FirstOrDefault().Addressline2 = personModel.Address.Addressline2;
                    oldPerson.Addresses.FirstOrDefault().CountryCode = personModel.Address.CountryCode;
                    oldPerson.Addresses.FirstOrDefault().Place = personModel.Address.Place;
                    oldPerson.Addresses.FirstOrDefault().ZIP= personModel.Address.ZIP;
                    
                    _repo.Update(oldPerson);
                     _repo.Save();
                }
                else
                {
                    _repo.Create(person);
                    _repo.Save();
                  
                }
             

            }
            catch (DbEntityValidationException e)
            {
                result.ErrorMessage.Add(e.Message);
            }
            return result;
        }
 
    }
}