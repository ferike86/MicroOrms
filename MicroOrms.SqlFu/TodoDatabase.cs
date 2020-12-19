using MicroOrms.Entities;
using SqlFu;
using SqlFu.Configuration;
using SqlFu.Providers.Sqlite;
using SqlFu.Providers.SqlServer;
using System;
using System.Data.SqlClient;

namespace MicroOrms.SqlFu
{
    public class TodoDatabase : ITodoDatabase
    {
        private readonly IDbFactory dbFactory;

        public TodoDatabase(ProviderType providerType, string dbConnectionString)
        {
            SqlFuManager.Configure(cfg =>
            {
                switch(providerType)
                {
                    case ProviderType.SqlServer2012:
                        cfg.AddProfile(new SqlServer2012Provider(SqlClientFactory.Instance.CreateConnection), dbConnectionString);
                        break;
                    case ProviderType.Sqlite:
                        cfg.AddProfile(new SqliteProvider(SqlClientFactory.Instance.CreateConnection), dbConnectionString);
                        break;
                    default:
                        throw new ArgumentException("Not supported provider type", nameof(providerType));
                }
                cfg.ConfigureTableForPoco<TodoItem>(info =>
                {
                    info.TableName = new TableName("todo_item");
                    info.Property(t => t.Id).IsAutoincremented();
                    info.Property(t => t.IsComplete).MapToColumn("is_complete");
                    info.Property(t => t.UserId).MapToColumn("user_id");
                });
                cfg.ConfigureTableForPoco<User>(info =>
                {
                    info.TableName = new TableName("user");
                    info.Property(u => u.Id).IsAutoincremented();
                    info.IgnoreProperties(u => u.TodoItems);
                });
            });
            dbFactory = SqlFuManager.GetDbFactory();
        }

        public ICrudOperations<User> Users => new UserOperations(dbFactory);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(dbFactory);
    }
}
