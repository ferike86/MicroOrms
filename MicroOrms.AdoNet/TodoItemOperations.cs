using MicroOrms.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace MicroOrms.AdoNet
{
    public class TodoItemOperations : CrudOperationsBase, ICrudOperations<TodoItem>
    {
        private static readonly string readAllCommand = "SELECT * FROM todo_item;";
        private static readonly string readCommand = "SELECT * FROM todo_item WHERE id = @Id;";
        private static readonly string deleteCommand = "DELETE FROM todo_item WHERE id = @Id;";
        private static readonly string insertCommand = "INSERT INTO todo_item (name, is_complete, user_id) VALUES (@Name, @IsComplete, @UserId);";
        private static readonly string updateCommand = "UPDATE todo_item SET name = @Name, is_complete = @IsComplete, user_id = @UserId WHERE id = @Id;";

        private Func<IDbConnection> ConnectionFactory { get; }

        public TodoItemOperations(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public long Create(TodoItem todoItem)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertCommand;
                    command.Parameters.Add(CreateParameter(command, "Name", todoItem.Name));
                    command.Parameters.Add(CreateParameter(command, "IsComplete", todoItem.IsComplete));
                    command.Parameters.Add(CreateParameter(command, "UserId", todoItem.UserId));
                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM todo_item WHERE name = @Name and is_complete = @IsComplete and user_id = @UserId";
                    command.Parameters.Add(CreateParameter(command, "Name", todoItem.Name));
                    command.Parameters.Add(CreateParameter(command, "IsComplete", todoItem.IsComplete));
                    command.Parameters.Add(CreateParameter(command, "UserId", todoItem.UserId));

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
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = deleteCommand;
                    command.Parameters.Add(CreateParameter(command, "Id", id));
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public TodoItem Read(long id)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = readCommand;
                    command.Parameters.Add(CreateParameter(command, "Id", id));

                    using (var dataReader = command.ExecuteReader())
                    {
                        var hasMoreRows = dataReader.Read();
                        if (hasMoreRows)
                        {
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
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = readAllCommand;
                    var todoItemList = new List<TodoItem>();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            todoItemList.Add(ReadTodoItem(dataReader));
                        }
                    }

                    return todoItemList;
                }
            }
        }

        public bool Update(TodoItem todoItem)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = updateCommand;
                    command.Parameters.Add(CreateParameter(command, "Id", todoItem.Id));
                    command.Parameters.Add(CreateParameter(command, "Name", todoItem.Name));
                    command.Parameters.Add(CreateParameter(command, "IsComplete", todoItem.IsComplete));
                    command.Parameters.Add(CreateParameter(command, "UserId", todoItem.UserId));
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        internal static TodoItem ReadTodoItem(IDataReader dataReader)
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
