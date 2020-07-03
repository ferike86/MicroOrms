using MicroOrms.Entities;

namespace MicroOrms.LinqToDb
{
    public class TodoDatabase : ITodoDatabase
    {
        private readonly string dbConfigurationName;

        public TodoDatabase(string dbConfigurationName)
        {
            this.dbConfigurationName = dbConfigurationName;
        }

        public ICrudOperations<User> Users => new UserOperations(dbConfigurationName);

        public ICrudOperations<TodoItem> TodoItems => new TodoItemOperations(dbConfigurationName);
    }
}
