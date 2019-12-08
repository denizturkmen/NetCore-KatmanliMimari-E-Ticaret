using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.DataAccessLayer.Concrete.EntityFrameWork
{
    public class EfCoreCategoryDAL : EfCoreGenericRepository<Category, DataBaseContext>, ICategoryDAL
    {
        public Category GetByIdWithProducts(int id)
        {
            using (var context = new DataBaseContext())
            {
                //category içindeki productlar almak için
                return context.Categories
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault();

            }
        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context = new DataBaseContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1";
                context.Database.ExecuteSqlCommand(cmd, productId, categoryId);
            }
        }
    }
}
