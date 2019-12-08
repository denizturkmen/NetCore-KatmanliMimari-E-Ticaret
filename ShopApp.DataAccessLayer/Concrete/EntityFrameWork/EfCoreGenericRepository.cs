using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccessLayer.Abstract;

namespace ShopApp.DataAccessLayer.Concrete.EntityFrameWork
{
    // T class(Product ya da Category) TContext=DatabaseContext alcak
    //bunu yapmamızdaki amaç EfCoreProductDal ve EfCoreCategoryDal metotlar aynı 
    // sadece class isimleri değiştiği için generic olarak tanımladık
    public class EfCoreGenericRepository<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext, new()

    {
        public virtual void Create(T entity)
        {
            using (var context = new DataBaseContext())
            {
                //generic tanımlıyoruz hangi class gelirse onu alcak
                // product.add oluyor
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public virtual void Delete(T entity)
        {
            using (var context = new DataBaseContext())
            {
                //generic tanımlıyoruz hangi class gelirse onu alcak
                // product.remove oluyor
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        //burada virtual diyerek bu metotu istediğimiz yerde override edebiliriz NEDEN
        //Çünkü biz cart update ettiğimizde cart güncelliyor ama cartItems güncellenmiyor 
        //carttında ilişkili olduğu cartItem güncellemek lazım
        public virtual void Update(T entity)
        {
            using (var context = new DataBaseContext())
            {
                //generic tanımlıyoruz hangi class gelirse onu alcak
                // product.update oluyor
                //Entity Framemework özgü update
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public virtual List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<T>().ToList()
                    : context.Set<T>().Where(filter).ToList();
            }
        }

        public virtual T GetById(int id)
        {
            using (var context = new DataBaseContext())
            {
                return context.Set<T>().Find(id);
            }
        }

        public virtual T GetOne(Expression<Func<T, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<T>().Where(filter).SingleOrDefault();
            }
        }

    }
}
