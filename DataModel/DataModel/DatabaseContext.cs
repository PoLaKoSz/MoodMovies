using DataModel.DataModel.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;

namespace DataModel.DataModel
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Pass the connection string dynamically at runtime.
        /// </summary>
        /// <param name="dbPath"></param>
        public DatabaseContext(string dbPath)
            : base(new SQLiteConnection()
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
            modelBuilder.Entity<User_Movies>()
            .HasKey(x => new { x.User_Id, x.UId });

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<User_Movies> UserMovies { get; set; }
    }
    
}
