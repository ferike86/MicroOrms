using LinqToDB.Mapping;

namespace MicroOrms.LinqToDb.Entities
{
    [Table(Name = "todo_item")]
    internal class LinqToDbTodoItem
    {
        [PrimaryKey, Identity]
        public long Id { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "is_complete")]
        public bool Is_Complete { get; set; }

        [NotNull]
        [Column(Name = "user_id")]
        public long User_Id { get; set; }
    }
}
