using AutoMapper;
using MicroOrms.Entities;
using MicroOrms.EntityFramework.Mappers;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace MicroOrms.EntityFramework
{
    public class UserOperations : ICrudOperations<User>
    {
        private static readonly IMapper mapper = UserMapper.Mapper;
        private readonly string dbConnectionString;

        public UserOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(User user)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                var createdUser = todoContext.user.Add(mapper.Map<user>(user));
                todoContext.SaveChanges();
                return createdUser.id;
            }
        }

        public bool Delete(long id)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                var userToDelete = todoContext.user.Find(id);
                todoContext.todo_item.RemoveRange(todoContext.todo_item.Where(todoItem => todoItem.user_id == id));
                todoContext.user.Remove(userToDelete);
                return todoContext.SaveChanges() > 0;
            }
        }

        public User Read(long id)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                return mapper.Map<User>(todoContext.user.Find(id));
            }
        }

        public IEnumerable<User> ReadAll()
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                return mapper.Map<IEnumerable<User>>(todoContext.user);
            }
        }

        public bool Update(User user)
        {
            using (var todoContext = new TodoContext(dbConnectionString))
            {
                todoContext.user.AddOrUpdate(mapper.Map<user>(user));
                return todoContext.SaveChanges() > 0;
            }
        }
    }
}
