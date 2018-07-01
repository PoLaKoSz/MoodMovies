using System;

namespace DataModel.DataModel
{
    public interface IDb
    {
        DatabaseContext context { get; }

        void DumpDatabase();
    }
}
