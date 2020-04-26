using Dapper;
using MicroOrms.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MicroOrms.Dapper
{
    public class UserOperations : ICrudOperations<User>
    {
        private static readonly string readAllCommand = "SELECT [user].id Id, [user].name Name, todo_item.id Id, todo_item.name Name, todo_item.is_complete IsComplete, todo_item.user_id UserId FROM [user] LEFT JOIN todo_item ON [user].id = todo_item.user_id;";
        private static readonly string readCommand = "SELECT id Id, name Name FROM [user] WHERE id = @Id; SELECT id Id, name Name, is_complete IsComplete, user_id UserId FROM todo_item WHERE user_id = @Id;";
        private static readonly string deleteUserCommand = "DELETE FROM [user] WHERE id = @Id;";
        private static readonly string deleteTodoItemsCommand = "DELETE FROM todo_item WHERE user_id = @Id;";
        private static readonly string insertUserCommand = "INSERT INTO [user] (name) VALUES (@Name);";
        private static readonly string updateCommand = "UPDATE [user] SET name = @Name WHERE id = @Id;";

        private readonly string dbConnectionString;

        public UserOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(User user)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Execute(insertUserCommand, user);
                var createdUser = connection.QueryFirst<User>("SELECT id Id, name Name FROM [user] WHERE name = @Name", user);
                return createdUser.Id;
            }
        }

        public bool Delete(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(deleteTodoItemsCommand, new { Id = id }, transaction);
                    var affectedRows = connection.Execute(deleteUserCommand, new { Id = id }, transaction);

                    transaction.Commit();

                    return affectedRows > 0;
                }
            }
        }

        public User Read(long id)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                var result = connection.QueryMultiple(readCommand, new { Id = id });
                var user = result.ReadSingle<User>();
                user.TodoItems = result.Read<TodoItem>().AsList();
                return user;
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                var userDictionary = new Dictionary<long, User>();

                var userList = connection.Query<User, TodoItem, User>(readAllCommand, (user, todoItem) =>
                {
                    if (!userDictionary.TryGetValue(user.Id, out User userEntity))
                    {
                        userEntity = user;
                        userDictionary.Add(userEntity.Id, userEntity);
                    }

                    if (todoItem != null)
                    {
                        userEntity.TodoItems.Add(todoItem);
                    }

                    return userEntity;
                }, splitOn: "Id").Distinct().ToList();
                return userList;
            }
        }

        public bool Update(User user)
        {
            using (var connection = new SqlConnection(dbConnectionString))
            {
                return connection.Execute(updateCommand, user) > 0;
            }
        }
    }
}
