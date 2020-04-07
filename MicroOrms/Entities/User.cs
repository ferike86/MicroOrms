using System.Collections.Generic;

namespace MicroOrms.Entities
{
    public class User
    {
        public User()
        {
            TodoItems = new List<TodoItem>(0);
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; }
    }
}
