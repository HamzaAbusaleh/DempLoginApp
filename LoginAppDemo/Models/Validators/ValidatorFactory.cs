using Autofac;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models.Validators
{
    public class  ValidatorFactory : IValidatorFactory
    {
        private readonly IComponentContext container;

        public ValidatorFactory(IComponentContext container)
        {
            this.container = container;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        public IValidator GetValidator(Type type)
        {
            var genericType = typeof(IValidator<>).MakeGenericType(type);
            object validator;
            if (container.TryResolve(genericType, out validator))
                return (IValidator)validator;

            return null;
        }
    }
}