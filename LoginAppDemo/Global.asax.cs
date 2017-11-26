using Autofac;
using Autofac.Integration.Mvc;
using LoginAppDemo.Repository.Interface;
using LoginAppDemo.Repository.Implementation;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LoginAppDemo.Models;
using LoginAppDemo.Services.Implementation;
using LoginAppDemo.Services.Interface;
using AutoMapper;
using LoginAppDemo.Mapping;
using FluentValidation;
using FluentValidation.Mvc;
using LoginAppDemo.Models.Validators;
using System.Web.Security;
using LoginAppDemo.Security;
using Newtonsoft.Json;

namespace LoginAppDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            var loginAppProfile = typeof(LoginAppProviderMapping).Assembly.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t)).ToArray();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in loginAppProfile) { cfg.AddProfile(profile); }
            }));
            
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
             .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
             .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<LoginAppEntities>().InstancePerRequest();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();

            builder.RegisterType<UserValidator>().As<IValidator>().InstancePerLifetimeScope();
            builder.RegisterType<UserLoginValidator>().As<IValidator>().InstancePerLifetimeScope();

            builder.RegisterType<GenericRepository>().As<IGenericRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReadRepository>().As<IReadRepository>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<PersonService>().As<IPersonService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();


            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();
            
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
         
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new ValidatorFactory(container);
            });
        
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.FirstName = serializeModel.FirstName;
                newUser.LastName = serializeModel.LastName;
                newUser.roles = serializeModel.roles;

                HttpContext.Current.User = newUser;
            }

        }
    }
}
