using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace MicroOrms.Dapper.Contrib.Entities
{
    [Table("[user]")]
    internal class DapperContribUser
    {
        public DapperContribUser()
        {
            TodoItems = new List<DapperContribTodoItem>(0);
        }

        public long Id { get; set; }

        public string Name { get; set; }

        [Computed]
        public ICollection<DapperContribTodoItem> TodoItems { get; set; }
    }
}
