using MvcWebshop.WebUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebshop.WebUI.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        TEntity Get(int? id);
        IEnumerable<TEntity> GetAll();

        void Update(TEntity entity);

        void Delete(int id);

        void Save();

        void Dispose(bool disposing);
    }
}
