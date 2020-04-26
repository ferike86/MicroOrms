using MicroOrms.Entities;

namespace MicroOrms.LinqToDb
{
    public class TodoDatabase : ITodoDatabase
    {
        private readonly string dbConnectionString;

        public TodoDatabase(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public ICrudOperations<User> Users => new UserOperations(dbConnectionString);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(dbConnectionString);
    }
}
