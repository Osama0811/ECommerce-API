using CircuitsUc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.InfraStructure.Presistance
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public virtual DbSet<SecurityUser> SecurityUsers { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
       



        /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
              {
                  relationship.DeleteBehavior = DeleteBehavior.Restrict;
              }
          }*/
    }
}
