using System;

namespace DataModel.DataModel
{
    public interface IDb
    {
        DatabaseContext context { get; }

        /// <summary>
        /// Update the DB shema to the latest version
        /// </summary>
        void AutoMigrate();

        /// <summary>
        /// Set the DB's datacontext
        /// </summary>
        void Connect();
    }
}
