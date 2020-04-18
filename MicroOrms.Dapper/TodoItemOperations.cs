using Dapper;
using MicroOrms.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MicroOrms.Dapper
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private static readonly string readAllCommand = "SELECT id Id, name Name, is_complete IsComplete, user_id UserId FROM todo_item;";
        private static readonly string readCommand = "SELECT id Id, name Name, is_complete IsComplete, user_id UserId FROM todo_item WHERE id = @Id;";
        private static readonly string deleteCommand = "DELETE FROM todo_item WHERE id = @Id;";
        private static readonly string insertCommand = "INSERT INTO todo_item (name, is_complete, user_id) VALUES (@Name, @IsComplete, @UserId);";
        private static readonly string updateCommand = "UPDATE todo_item SET name = @NAME, is_complete = @IsComplete, user_id = @UserId WHERE id = @Id;";

        private readonly string dbConnectionString;

        public TodoItemOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Execute(insertCommand, todoItem);
                var createdTodoItem = connection.QueryFirst<TodoItem>("SELECT id Id, name Name, is_complete IsComplete, user_id UserId FROM todo_item WHERE name = @Name and is_complete = @IsComplete and user_id = @UserId", todoItem);
                return createdTodoItem.Id;
            }
        }

        public bool Delete(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Execute(deleteCommand, new { Id = id }) > 0;
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.QueryFirst<TodoItem>(readCommand, new { Id = id });
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Query<TodoItem>(readAllCommand);
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Execute(updateCommand, todoItem) > 0;
            }
        }
    }
}
