using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using MicroOrms.LinqToDb.Entities;

namespace MicroOrms.LinqToDb
{
    internal class LinqToDbTodoDatabase : DataConnection
    {
        static LinqToDbTodoDatabase()
        {
            Configuration.Linq.AllowMultipleQuery = true;
        }

        public LinqToDbTodoDatabase(string dbConfigurationName) : base(dbConfigurationName)
        {
        }

        public ITable<LinqToDbUser> Users => GetTable<LinqToDbUser>();

        public ITable<LinqToDbTodoItem> TodoItems => GetTable<LinqToDbTodoItem>();
    }
}
