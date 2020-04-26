using MicroOrms.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MicroOrms.AdoNet
{
    public class UserOperations : ICrudOperations<User>
    {
        private static readonly string readAllCommand = "SELECT * FROM [user];";
        private static readonly string readCommand = "SELECT * FROM [user] WHERE id = @Id;";
        private static readonly string deleteCommand = "DELETE FROM [user] WHERE id = @Id;";
        private static readonly string deleteTodoItemsCommand = "DELETE FROM todo_item WHERE user_id = @Id;";
        private static readonly string insertCommand = "INSERT INTO [user] (name) VALUES (@Name);";
        private static readonly string updateCommand = "UPDATE [user] SET name = @Name WHERE id = @Id;";

        private readonly string dbConnectionString;
        private ICrudOperations<TodoItem> todoItemOperations;

        public UserOperations(string dbConnectionString, ICrudOperations<TodoItem> todoItemOperations)
        {
            this.dbConnectionString = dbConnectionString;
            this.todoItemOperations = todoItemOperations;
        }

        public long Create(User user)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(insertCommand, connection))
                {
                    command.Parameters.AddWithValue("Name", user.Name);
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand("SELECT * FROM [user] WHERE name = @Name", connection))
                {
                    command.Parameters.AddWithValue("Name", user.Name);

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
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(deleteTodoItemsCommand, connection, transaction))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(deleteCommand, connection, transaction))
                    {
                        command.Parameters.AddWithValue("Id", id);
                        var affectedRows = command.ExecuteNonQuery();

                        transaction.Commit();

                        return affectedRows > 0;
                    }
                }
            }
        }

        public User Read(long id)
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
                            var user = ReadUser(dataReader);
                            user.TodoItems = todoItemOperations.ReadAll().Where(todoItem => todoItem.UserId == user.Id).ToList();
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
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(readAllCommand, connection))
                {
                    var userList = new List<User>();
                    var todoItemList = todoItemOperations.ReadAll();

                    using (var dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var user = ReadUser(dataReader);
                                user.TodoItems = todoItemList.Where(todoItem => todoItem.UserId == user.Id).ToList();
                                userList.Add(user);
                            }
                        }
                    }

                    return userList;
                }
            }
        }

        public bool Update(User user)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("Id", user.Id);
                    command.Parameters.AddWithValue("Name", user.Name);
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
