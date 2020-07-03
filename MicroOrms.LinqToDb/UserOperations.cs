using AutoMapper;
using LinqToDB;
using MicroOrms.Entities;
using MicroOrms.LinqToDb.Entities;
using MicroOrms.LinqToDb.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace MicroOrms.LinqToDb
{
    public class UserOperations : ICrudOperations<User>
    {
        private static readonly IMapper userMapper = UserMapper.Mapper;
        private readonly string dbConfigurationName;

        public UserOperations(string dbConfigurationName)
        {
            this.dbConfigurationName = dbConfigurationName;
        }

        public long Create(User user)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return linqToDbTodoDatabase.InsertWithInt64Identity(userMapper.Map<LinqToDbUser>(user));
            }
        }

        public bool Delete(long id)
        {
            using (var transaction = new TransactionScope())
            {
                using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
                {
                    linqToDbTodoDatabase.TodoItems.Where(t => t.User_Id == id).Delete();
                    var affectedRows = linqToDbTodoDatabase.Users.Where(u => u.Id == id).Delete();

                    transaction.Complete();

                    return affectedRows > 0;
                }
            }
        }

        public User Read(long id)
        {
            using (var transaction = new TransactionScope())
            {
                using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
                {
                    var user = linqToDbTodoDatabase.Users.Where(u => u.Id == id).FirstOrDefault();

                    if (user != null)
                    {
                        user.TodoItems = linqToDbTodoDatabase.TodoItems.Where(t => t.User_Id == id).ToList();
                    }

                    transaction.Complete();

                    return userMapper.Map<User>(user);
                }
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var transaction = new TransactionScope())
            {
                using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
                {
                    var userList = linqToDbTodoDatabase.Users.ToList();
                    var todoItemList = linqToDbTodoDatabase.TodoItems.ToList();

                    transaction.Complete();

                    return userMapper.Map<IEnumerable<User>>(userList.Select(user => SelectTodoItems(user, todoItemList)));
                }
            }
        }

        public bool Update(User user)
        {
            using (var linqToDbTodoDatabase = new LinqToDbTodoDatabase(dbConfigurationName))
            {
                return linqToDbTodoDatabase.Update(userMapper.Map<LinqToDbUser>(user)) > 0;
            }
        }

        private LinqToDbUser SelectTodoItems(LinqToDbUser user, IEnumerable<LinqToDbTodoItem> todoItems)
        {
            var todoItemsToAdd = todoItems.Where(todoItem => todoItem.User_Id == user.Id);
            user.TodoItems = todoItemsToAdd.ToList();
            return user;
        }
    }
}
