using MicroOrms.Entities;
using System;
using System.Collections.Generic;

namespace MicroOrms.EntityFramework
{
    public class UserOperations : ICrudOperations<User>
    {
        private readonly string dbConnectionString;

        public UserOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
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
