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
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public User Read(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> ReadAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
