using MicroOrms.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MicroOrms.AdoNet
{
    public class TodoItemOperations : ICrudOperations<TodoItem>
    {
        private static readonly string readAllCommand = "SELECT * FROM todo_item;";
        private static readonly string readCommand = "SELECT * FROM todo_item WHERE id = @Id;";
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
                connection.Open();
                using (var command = new SqlCommand(insertCommand, connection))
                {
                    command.Parameters.AddWithValue("Name", todoItem.Name);
                    command.Parameters.AddWithValue("IsComplete", todoItem.IsComplete);
                    command.Parameters.AddWithValue("UserId", todoItem.UserId);
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("SELECT * FROM todo_item WHERE name = @Name and is_complete = @IsComplete and user_id = @UserId", connection))
                {
                    command.Parameters.AddWithValue("Name", todoItem.Name);
                    command.Parameters.AddWithValue("IsComplete", todoItem.IsComplete);
                    command.Parameters.AddWithValue("UserId", todoItem.UserId);

                    using (var dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        return ReadTodoItem(dataReader).Id;
                    }
                }
            }
        }

        public bool Delete(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(deleteCommand, connection))
                {
                    command.Parameters.AddWithValue("Id", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(readCommand, connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            return ReadTodoItem(dataReader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public IEnumerable<TodoItem> ReadAll()
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(readAllCommand, connection))
                {
                    var todoItemList = new List<TodoItem>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                todoItemList.Add(ReadTodoItem(dataReader));
                            }
                        }
                    }

                    return todoItemList;
                }
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("Id", todoItem.Id);
                    command.Parameters.AddWithValue("Name", todoItem.Name);
                    command.Parameters.AddWithValue("IsComplete", todoItem.IsComplete);
                    command.Parameters.AddWithValue("UserId", todoItem.UserId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        private static TodoItem ReadTodoItem(IDataReader dataReader)
        {
            var todoItem = new TodoItem()
            {
                Id = dataReader.GetInt64(0),
                Name = dataReader.GetString(1),
                IsComplete = dataReader.GetBoolean(2),
                UserId = dataReader.GetInt64(3)
            };

            return todoItem;
        }
    }
}
