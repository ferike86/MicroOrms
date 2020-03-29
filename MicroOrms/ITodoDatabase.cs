using System.Collections.Generic;

namespace MicroOrms
{
    public interface ITodoDatabase
    {
        long Create(TodoItem todoItem);

        TodoItem Read(long id);

        IEnumerable<TodoItem> ReadAll();

        bool Update(TodoItem todoItem);

        bool Delete(long id);
    }
}
