using LoginAppDemo.Services.Interface;
using System;
using System.Linq;
using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using LoginAppDemo.Repository.Interface;
using AutoMapper;
using System.Data.Entity.Validation;

namespace LoginAppDemo.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public bool CheckEmailExists(string email)
        {
            var user = _repo.Get<User>(e => e.Email.ToLower() == email.ToLower()).FirstOrDefault();
            return user != null;
        }

        public bool CheckUserNameExists(string userName)
        {
            var user = _repo.Get<User>(e => e.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            return user != null;
        }

        public Result Create(UserModel userModel)
        {
            var result = new Result();
            try
            {
                var user = _mapper.Map<UserModel, User>(userModel);
                var crypto = new SimpleCrypto.PBKDF2();
                var encrypPass = crypto.Compute(user.Password);
                user.PasswordSalt = crypto.Salt;
                user.Password = crypto.HashedText;
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                _repo.Create(user);
                _repo.Save();

                var userRole = new UserRole { UserId = user.id, RoleId = 2 };
                _repo.Create(userRole);
                _repo.Save();

            }
            catch(DbEntityValidationException e)
            {
                result.ErrorMessage.Add(e.Message);
            }
            return result;
        }

        public User GetUserEntityById(long id)
        {
            var user = _repo.GetFirst<User>(e => e.id == id);
            return user;
        }

        public User GetUserByUserName(string userName)
        {
            var user = _repo.GetFirst<User>(e => e.UserName == userName);
            return user;
        }

        public UserModel GetUserByEmail(string email)
        {
            var user = _repo.GetFirst<User>(e => e.Email.ToLower() == email.ToLower());
            return _mapper.Map<User, UserModel>(user);
        }

        public UserModel GetUserById(long id)
        {
            var user = _repo.GetById<User>(id);
            return _mapper.Map<User, UserModel>(user);
        }

    }
}