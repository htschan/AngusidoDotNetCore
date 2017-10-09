using System.Collections.Generic;

namespace Server.Repositories
{
   public interface IRepository<T> where T : class
   {
      IList<T> GetAll();
      void Insert(T entity);
      void Update(T entity);
      void Delete(T entity);
   }
}
