using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using CircuitsUc.Domain.IRepositories.Base;
using CircuitsUc.InfraStructure.Presistance;
using CircuitsUc.InfraStructure.Reposatories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.InfraStructure.Reposatories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DBContext context;
        public IRepository<SecurityUser> SecurityUser { get; private set; }
        public IRepository<Document> Document { get; private set; }
        public IRepository<ProductCategory> ProductCategory { get; private set; }
       



        public UnitOfWork(DBContext context)
        {
            this.context = context;
            SecurityUser = new Repository<SecurityUser>(this.context);
            Document = new Repository<Document>(this.context);
            ProductCategory = new Repository<ProductCategory>(this.context);
           


        }


        public void Dispose()
        {
            context.Dispose();
        }
        public int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex) { return -1; }
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }


    }
}
