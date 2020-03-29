using Dapper.Contrib.Extensions;

namespace MicroOrms.Dapper.Contrib
{
    [Table("todo_item")]
    internal class DapperContribTodoItem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Is_Complete { get; set; }

        public long User_Id { get; set; }
    }
}
