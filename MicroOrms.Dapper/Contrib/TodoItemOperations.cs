using AutoMapper;
using Dapper.Contrib.Extensions;
using MicroOrms.Dapper.Contrib.Entities;
using MicroOrms.Dapper.Contrib.Mappers;
using MicroOrms.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MicroOrms.Dapper.Contrib
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
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Insert(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }

        public bool Delete(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                var todoItemToDelete = connection.Get<DapperContribTodoItem>(id);
                return connection.Delete(todoItemToDelete);
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return mapper.Map<TodoItem>(connection.Get<DapperContribTodoItem>(id));
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return mapper.Map<IEnumerable<TodoItem>>(connection.GetAll<DapperContribTodoItem>());
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Update(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }
    }
}
