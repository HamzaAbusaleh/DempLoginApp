using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoginAppDemo.Repository.Interface
{
    public interface IGenericRepository : IReadRepository
    {
        void Create<TEntity>(TEntity entity)
            where TEntity : class;

        void Update<TEntity>(TEntity entity)
            where TEntity : class;

        void Delete<TEntity>(object id)
            where TEntity : class;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class;

        void Save();

        Task SaveAsync();
    }
}