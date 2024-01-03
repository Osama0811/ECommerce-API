using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Domain.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<SecurityUser> SecurityUser { get; }
        IRepository<Document> Document { get; }
        IRepository<ProductCategory> ProductCategory { get; }
      


        int Save();
        Task<int> SaveAsync();
    }
}