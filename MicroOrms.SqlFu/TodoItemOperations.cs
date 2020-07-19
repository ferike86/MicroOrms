using MicroOrms.Entities;
using SqlFu;
using System;
using System.Collections.Generic;

namespace MicroOrms.SqlFu
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private readonly IDbFactory dbFactory;

        public TodoItemOperations(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public long Create(TodoItem todoItem)
        {
            using (var connection = dbFactory.Create())
            {
                return Convert.ToInt64(connection.Insert(todoItem).GetInsertedId<object>());
            }
        }

        public bool Delete(long id)
        {
            using (var connection = dbFactory.Create())
            {
                return connection.DeleteFrom<TodoItem>(t => t.Id == id) > 0;
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = dbFactory.Create())
            {
                return connection.QueryRow(q => q.From<TodoItem>().Where(t => t.Id == id).SelectAll());
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = dbFactory.Create())
            {
                return connection.QueryAs(q => q.From<TodoItem>().SelectAll());
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = dbFactory.Create())
            {
                return connection.UpdateFrom(
                    q => q.Data(todoItem).Ignore(t => t.Id),
                    o => o.SetTableName(connection.GetTableName<TodoItem>()))
                    .Where(t => t.Id == todoItem.Id).Execute() > 0;
            }
        }
    }
}
