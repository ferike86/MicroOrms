using MicroOrms.Entities;
using System;
using System.Data;

namespace MicroOrms.AdoNet
{
    public class TodoDatabase : ITodoDatabase
    {
        private Func<IDbConnection> ConnectionFactory { get; }

        public TodoDatabase(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public ICrudOperations<User> Users => new UserOperations(ConnectionFactory, TodoItems);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(ConnectionFactory);
    }
}
