using MicroOrms.Entities;
using System;
using System.Data;

namespace MicroOrms.Dapper.Contrib
{
    public class TodoDatabase : ITodoDatabase
    {
        private Func<IDbConnection> ConnectionFactory { get; }

        public TodoDatabase(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public ICrudOperations<User> Users => new UserOperations(ConnectionFactory);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(ConnectionFactory);
    }
}
