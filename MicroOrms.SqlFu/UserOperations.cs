using MicroOrms.Entities;
using SqlFu;
using System;
using System.Collections.Generic;

namespace MicroOrms.SqlFu
{
    public class UserOperations : ICrudOperations<User>
    {
        private readonly IDbFactory dbFactory;

        public UserOperations(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public long Create(User user)
        {
            using (var connection = dbFactory.Create())
            {
                return Convert.ToInt64(connection.Insert(user).GetInsertedId<object>());
            }
        }

        public bool Delete(long id)
        {
            using (var connection = dbFactory.Create())
            {
                connection.DeleteFrom<TodoItem>(t => t.UserId == id);
                return connection.DeleteFrom<User>(u => u.Id == id) > 0;
            }
        }

        public User Read(long id)
        {
            using (var connection = dbFactory.Create())
            {
                var user = connection.QueryRow(q => q.From<User>().Where(u => u.Id == id).SelectAll());
                if (user != null)
                {
                    user.TodoItems = connection.QueryAs(q => q.From<TodoItem>().Where(t => t.UserId == user.Id).SelectAll());
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var connection = dbFactory.Create())
            {
                var userList = connection.QueryAs(q => q.From<User>().SelectAll());
                foreach (var user in userList)
                {
                    user.TodoItems = connection.QueryAs(q => q.From<TodoItem>().Where(t => t.UserId == user.Id).SelectAll());
                }
                return userList;
            }
        }

        public bool Update(User user)
        {
            using (var connection = dbFactory.Create())
            {
                return connection.UpdateFrom(
                    q => q.Data(user).Ignore(u => u.Id, u => u.TodoItems),
                    o => o.SetTableName(connection.GetTableName<User>()))
                    .Where(t => t.Id == user.Id).Execute() > 0;
            }
        }
    }
}
