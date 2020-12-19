using AutoMapper;
using Dapper.Contrib.Extensions;
using MicroOrms.Dapper.Contrib.Entities;
using MicroOrms.Dapper.Contrib.Mappers;
using MicroOrms.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MicroOrms.Dapper.Contrib
{
    public class UserOperations : ICrudOperations<User>
    {
        private static readonly IMapper userMapper = UserMapper.Mapper;
        private Func<IDbConnection> ConnectionFactory { get; }

        public UserOperations(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public long Create(User user)
        {
            using (var connection = ConnectionFactory())
            {
                return connection.Insert(userMapper.Map<DapperContribUser>(user));
            }
        }

        public bool Delete(long id)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var todoItemsToDelete = connection.GetAll<DapperContribTodoItem>(transaction).Where(todoItem => todoItem.User_Id == id).ToList();
                    connection.Delete(todoItemsToDelete, transaction);

                    var userToDelete = connection.Get<DapperContribUser>(id, transaction);
                    var isDeleted = connection.Delete(userToDelete, transaction);

                    transaction.Commit();

                    return isDeleted;
                }
            }
        }

        public User Read(long id)
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var user = connection.Get<DapperContribUser>(id, transaction);

                    user.TodoItems = connection.GetAll<DapperContribTodoItem>(transaction).Where(todoItem => todoItem.User_Id == user.Id).ToList();

                    transaction.Commit();

                    return userMapper.Map<User>(user);
                }
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var connection = ConnectionFactory())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var userList = connection.GetAll<DapperContribUser>(transaction);
                    var todoItemList = connection.GetAll<DapperContribTodoItem>(transaction);

                    transaction.Commit();

                    return userMapper.Map<IEnumerable<User>>(userList.Select(user => SelectTodoItems(user, todoItemList)));
                }
            }
        }

        public bool Update(User user)
        {
            using (var connection = ConnectionFactory())
            {
                return connection.Update(userMapper.Map<DapperContribUser>(user));
            }
        }

        private DapperContribUser SelectTodoItems(DapperContribUser user, IEnumerable<DapperContribTodoItem> todoItems)
        {
            var todoItemsToAdd = todoItems.Where(todoItem => todoItem.User_Id == user.Id);
            user.TodoItems = todoItemsToAdd.ToList();
            return user;
        }
    }
}
