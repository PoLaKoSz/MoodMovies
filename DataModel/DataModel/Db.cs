using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DataModel.DataModel
{
    public class Db : IDb
    {
        private static string _dbFile { get; set; }
        public DatabaseContext context { get; private set; }
        
        public Db(string dbFolder)
        {
            _dbFile = Path.Combine(dbFolder, "MoodMovies.db");
        }
        
        /// <summary>
        /// Update the DB shema to the latest version
        /// </summary>
        public void AutoMigrate()
        {
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSQLite()
                    // Set the connection string
                    .WithGlobalConnectionString("Data Source=" + _dbFile)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Db).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </sumamry>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        /// <summary>
        /// Set the DB's datacontext
        /// </summary>
        public void Connect()
        {
            context = new DatabaseContext(_dbFile);
        }
    }
}
