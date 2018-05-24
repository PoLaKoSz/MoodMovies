using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataModel
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Pass the connection string dynamically at runtime.
        /// </summary>
        /// <param name="dbPath"></param>
        public DatabaseContext(string dbPath) : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = dbPath,
                ForeignKeys = true
            }.ConnectionString
        }, true)
        {

        }
        /// <summary>
        /// This override solves this exception 'System.Data.Entity.Infrastructure.DbUpdateException'
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        //List of the all the tables we will be using
        public DbSet<Users> Users { get; set; }
    }
    
}
