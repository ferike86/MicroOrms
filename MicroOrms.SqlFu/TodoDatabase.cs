using MicroOrms.Entities;
using SqlFu;
using SqlFu.Configuration;
using SqlFu.Providers.SqlServer;
using System.Data.SqlClient;

namespace MicroOrms.SqlFu
{
    public class TodoDatabase : ITodoDatabase
    {
        private readonly IDbFactory dbFactory;

        public TodoDatabase(string dbConnectionString)
        {
            SqlFuManager.Configure(cfg =>
            {
                cfg.AddProfile(new SqlServer2012Provider(SqlClientFactory.Instance.CreateConnection), dbConnectionString);
                cfg.ConfigureTableForPoco<TodoItem>(info =>
                {
                    info.TableName = new TableName("todo_item");
                    info.Property(t => t.Id).IsAutoincremented();
                    info.Property(t => t.IsComplete).MapToColumn("is_complete");
                    info.Property(t => t.UserId).MapToColumn("user_id");
                });
            });
            dbFactory = SqlFuManager.GetDbFactory();
        }

        public ICrudOperations<User> Users => new UserOperations(dbFactory);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(dbFactory);
    }
}
