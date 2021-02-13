using Microsoft.EntityFrameworkCore;
using MssqlTestApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MssqlTestApp
{
    class OrdersContext : DbContext
    {
        public OrdersContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OrdersDb;Trusted_Connection=True;MultipleActiveResultSets=True;");
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaySum> PaySum { get; set; }
        public DbSet<Periodicity> Periodicities { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

    }
}
