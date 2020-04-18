using MicroOrms.Entities;
using System.Collections.Generic;

namespace MicroOrms.AdoNet
{
    public class UserOperations : ICrudOperations<User>
    {
        private readonly string dbConnectionString;

        public UserOperations(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public long Create(User entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public User Read(long id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public bool Update(User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
