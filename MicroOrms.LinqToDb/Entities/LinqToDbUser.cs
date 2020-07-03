using LinqToDB.Mapping;
using System.Collections.Generic;

namespace MicroOrms.LinqToDb.Entities
{
    [Table(Name = "[user]")]
    internal class LinqToDbUser
    {
        public LinqToDbUser()
        {
            TodoItems = new List<LinqToDbTodoItem>(0);
        }

        [PrimaryKey, Identity]
        public long Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [NotColumn]
        public ICollection<LinqToDbTodoItem> TodoItems { get; set; }
    }
}
