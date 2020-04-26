using MicroOrms.Entities;
using System;
using System.Collections.Generic;

namespace MicroOrms.LinqToDb
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private readonly string dbConnectionString;

        public TodoItemOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(TodoItem entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public TodoItem Read(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(TodoItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
