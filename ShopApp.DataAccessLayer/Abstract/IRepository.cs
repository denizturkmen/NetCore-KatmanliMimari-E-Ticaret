using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        T GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter);
        // T entity hangi class ise o ile ilgili colonlarda sorgu yapmak için (LinQ )
        // null eşitliyorum eğer colonlarla sorgulama yapmıyorsa bütün kayıtlar gelsin diye
        List<T> GetAll(Expression<Func<T, bool>> filter = null);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
