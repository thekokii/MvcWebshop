using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcWebshop.WebUI.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }
        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Delete(int id)
        {
            Context.Set<TEntity>().Remove(Context.Set<TEntity>().Find(id));
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }

        public TEntity Get(int? id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
            //return Context.Set<TEntity>().ToList();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}