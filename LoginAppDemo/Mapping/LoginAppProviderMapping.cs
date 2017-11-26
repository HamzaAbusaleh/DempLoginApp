using AutoMapper;
using LoginAppDemo.Models;
using LoginAppDemo.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace LoginAppDemo.Mapping
{
    public class LoginAppProviderMapping : Profile
    {
        public LoginAppProviderMapping()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
            CreateMap<Person, PersonModel>()
                   .ForMember(x => x.Address, opt => opt.MapFrom(src =>
                    
                        new AddressModel()
                        {
                            Addressline1 = src.Addresses.FirstOrDefault() != null ? src.Addresses.FirstOrDefault().Addressline1 : null,
                            Addressline2 = src.Addresses.FirstOrDefault() != null ? src.Addresses.FirstOrDefault().Addressline2 : null,
                            CountryCode = src.Addresses.FirstOrDefault() != null ? src.Addresses.FirstOrDefault().CountryCode : null,
                            ZIP = src.Addresses.FirstOrDefault() != null ? src.Addresses.FirstOrDefault().ZIP : null,
                            Place = src.Addresses.FirstOrDefault() != null ? src.Addresses.FirstOrDefault().Place : null
                        }
                   ));
          
            CreateMap<PersonModel, Person>()
                 .ForMember(x => x.Id, opt => opt.Ignore())
                 .ForMember(x => x.UserId, opt => opt.Ignore()) 
                 .ForMember(x => x.Addresses, opt => opt.MapFrom(src =>
                    new List<Address>
                    {
                        new Address()
                        {
                            Addressline1 = src.Address.Addressline1,
                            Addressline2 = src.Address.Addressline2,
                            CountryCode = src.Address.CountryCode,
                            Place = src.Address.Place,
                            ZIP = src.Address.ZIP
                        }
                    }));
        }
       
    }
    }