using System.Collections.Generic;

namespace MicroOrms
{
    public interface ICrudOperations<TEntity> where TEntity : class
    {
        long Create(TEntity entity);

        TEntity Read(long id);

        IEnumerable<TEntity> ReadAll();

        bool Update(TEntity entity);

        bool Delete(long id);
    }
}
