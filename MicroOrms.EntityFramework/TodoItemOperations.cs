using AutoMapper;
using MicroOrms.Entities;
using MicroOrms.EntityFramework.Mappers;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace MicroOrms.EntityFramework
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private static readonly IMapper mapper = TodoItemMapper.Mapper;
        private readonly string dbConnectionString;

        public TodoItemOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(TodoItem todoItem)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                var createdTodoItem = todoContext.todo_item.Add(mapper.Map<todo_item>(todoItem));
                todoContext.SaveChanges();
                return createdTodoItem.id;
            }
        }

        public bool Delete(long id)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                var todoItemToDelete = todoContext.todo_item.Find(id);
                todoContext.todo_item.Remove(todoItemToDelete);
                return todoContext.SaveChanges() > 0;
            }
        }

        public TodoItem Read(long id)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                return mapper.Map<TodoItem>(todoContext.todo_item.Find(id));
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                return mapper.Map<IEnumerable<TodoItem>>(todoContext.todo_item);
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                todoContext.todo_item.AddOrUpdate(mapper.Map<todo_item>(todoItem));
                return todoContext.SaveChanges() > 0;
            }
        }
    }
}
