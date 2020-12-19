using AutoMapper;
using Dapper.Contrib.Extensions;
using MicroOrms.Dapper.Contrib.Entities;
using MicroOrms.Dapper.Contrib.Mappers;
using MicroOrms.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace MicroOrms.Dapper.Contrib
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private static readonly IMapper mapper = TodoItemMapper.Mapper;
        private Func<IDbConnection> ConnectionFactory { get; }

        public TodoItemOperations(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public long Create(TodoItem todoItem)
        {
            using (var connection = ConnectionFactory())
            {
                return connection.Insert(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }

        public bool Delete(long id)
        {
            using (var connection = ConnectionFactory())
            {
                var todoItemToDelete = connection.Get<DapperContribTodoItem>(id);
                return connection.Delete(todoItemToDelete);
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = ConnectionFactory())
            {
                return mapper.Map<TodoItem>(connection.Get<DapperContribTodoItem>(id));
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = ConnectionFactory())
            {
                return mapper.Map<IEnumerable<TodoItem>>(connection.GetAll<DapperContribTodoItem>());
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = ConnectionFactory())
            {
                return connection.Update(mapper.Map<DapperContribTodoItem>(todoItem));
            }
        }
    }
}
