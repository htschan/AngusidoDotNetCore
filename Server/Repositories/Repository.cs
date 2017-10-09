using System;
using System.Collections.Generic;

namespace Server.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        string errorMessage = string.Empty;

        public Repository()
        {
        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}