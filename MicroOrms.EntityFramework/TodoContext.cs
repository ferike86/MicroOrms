using System.Data.Entity;

namespace MicroOrms.EntityFramework
{
    public partial class TodoContext : DbContext
    {
        public TodoContext(string dbConnectionString) : base(dbConnectionString)
        {
        }
    }
}
