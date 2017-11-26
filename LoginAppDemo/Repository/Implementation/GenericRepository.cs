using LoginAppDemo.Models;
using LoginAppDemo.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoginAppDemo.Repository.Implementation
{
    public class GenericRepository : ReadRepository, IGenericRepository
    {
        private readonly LoginAppEntities context;

        public GenericRepository(LoginAppEntities _context) : base(_context)
         {
            context = _context;
        }

        public virtual void Create<TEntity>(TEntity entity) where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity) where TEntity : class
        {
            
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<TEntity>(object id) where TEntity : class
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save() 
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return Task.FromResult(0);
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }
    }
}