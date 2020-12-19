using MicroOrms.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MicroOrms.AdoNet
{
    public class UserOperations : CrudOperationsBase, ICrudOperations<User>
    {
        private static readonly string readAllCommand = "SELECT * FROM [user];";
        private static readonly string readCommand = "SELECT * FROM [user] WHERE id = @Id;";
        private static readonly string deleteCommand = "DELETE FROM [user] WHERE id = @Id;";
        private static readonly string deleteTodoItemsCommand = "DELETE FROM todo_item WHERE user_id = @Id;";
        private static readonly string insertCommand = "INSERT INTO [user] (name) VALUES (@Name);";
        private static readonly string updateCommand = "UPDATE [user] SET name = @Name WHERE id = @Id;";

        private Func<IDbConnection> ConnectionFactory { get; }
        private ICrudOperations<TodoItem> TodoItemOperations { get; }

        public UserOperations(Func<IDbConnection> connectionFactory, ICrudOperations<TodoItem> todoItemOperations)
        {
            ConnectionFactory = connectionFactory;
            TodoItemOperations = todoItemOperations;
        }

        public long Create(User user)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertCommand;
                    command.Parameters.Add(CreateParameter(command, "Name", user.Name));
                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [user] WHERE name = @Name";
                    command.Parameters.Add(CreateParameter(command, "Name", user.Name));

                    using (var dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        return ReadUser(dataReader).Id;
                    }
                }
            }
        }

        public bool Delete(long id)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = deleteTodoItemsCommand;
                        command.Parameters.Add(CreateParameter(command, "Id", id));
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = deleteCommand;
                        command.Parameters.Add(CreateParameter(command, "Id", id));
                        command.Transaction = transaction;
                        var affectedRows = command.ExecuteNonQuery();

                        transaction.Commit();

                        return affectedRows > 0;
                    }
                }
            }
        }

        public User Read(long id)
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
                            var user = ReadUser(dataReader);
                            user.TodoItems = TodoItemOperations.ReadAll().Where(todoItem => todoItem.UserId == user.Id).ToList();
                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = readAllCommand;
                    var userList = new List<User>();
                    var todoItemList = TodoItemOperations.ReadAll();

                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var user = ReadUser(dataReader);
                            user.TodoItems = todoItemList.Where(todoItem => todoItem.UserId == user.Id).ToList();
                            userList.Add(user);
                        }
                    }

                    return userList;
                }
            }
        }

        public bool Update(User user)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = updateCommand;
                    command.Parameters.Add(CreateParameter(command, "Id", user.Id));
                    command.Parameters.Add(CreateParameter(command, "Name", user.Name));
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        internal static User ReadUser(IDataReader dataReader)
        {
            var user = new User()
            {
                Id = dataReader.GetInt64(0),
                Name = dataReader.GetString(1),
            };

            return user;
        }
    }
}
