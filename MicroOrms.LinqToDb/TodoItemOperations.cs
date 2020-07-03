using AutoMapper;
using LinqToDB;
using MicroOrms.Entities;
using MicroOrms.LinqToDb.Entities;
using MicroOrms.LinqToDb.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace MicroOrms.LinqToDb
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private static readonly IMapper mapper = TodoItemMapper.Mapper;
        private readonly string dbConfigurationName;

        public TodoItemOperations(string dbConfigurationName)
        {
            this.dbConfigurationName = dbConfigurationName;
        }

        public long Create(TodoItem todoItem)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                var mappedItem = mapper.Map<LinqToDbTodoItem>(todoItem);
                return linqToDbTodoDatabase.InsertWithInt64Identity(mappedItem);
            }
        }

        public bool Delete(long id)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return linqToDbTodoDatabase.TodoItems.Where(t => t.Id == id).Delete() > 0;
            }
        }

        public TodoItem Read(long id)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return mapper.Map<TodoItem>(linqToDbTodoDatabase.TodoItems.Where(t => t.Id == id).FirstOrDefault());
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return mapper.Map<IEnumerable<TodoItem>>(linqToDbTodoDatabase.TodoItems);
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return linqToDbTodoDatabase.Update(mapper.Map<LinqToDbTodoItem>(todoItem)) > 0;
            }
        }
    }
}
