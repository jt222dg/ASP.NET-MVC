using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    public class Repository : IRepository
    {
        private MapEntities m_entities;

        public Repository()
        {
            this.m_entities = new MapEntities();
        }

        public void Add<T>(T entity) where T : class
        {
            try
            {
               this.m_entities.Set<T>().Add(entity);
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                this.m_entities.Set<T>().Remove(entity);
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }

        public T Find<T>(Func<T, bool> where) where T : class
        {
            try
            {
                return this.m_entities.Set<T>().SingleOrDefault(where);
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }

        public IEnumerable<T> FindAll<T>() where T : class
        {
            try
            {
                return this.m_entities.Set<T>().AsEnumerable();
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }

        public IQueryable<T> Query<T>() where T : class
        {
            try
            {
                return this.m_entities.Set<T>().AsQueryable<T>();
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }

        public void Save()
        {
            try
            {
                this.m_entities.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Fel vid databasanrop");
            }
        }
    }
}