using MicroOrms.Entities;

namespace MicroOrms
{
    public interface ITodoDatabase
    {
        ICrudOperations<User> Users { get; }

        ICrudOperations<TodoItem> TodoItems { get; }
    }
}
