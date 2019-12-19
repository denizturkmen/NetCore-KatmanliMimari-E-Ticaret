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
    public class EfCoreProductDAL : EfCoreGenericRepository<Product, DataBaseContext>, IProductDAL
    {

        //biz aslıdna burda DatabaseContext böyle oluşturmak yanlış
        //çünkü EfCoreProductDAL bağımlı olmuş oluyor bunu DI ile çözecez
        //Ve bütün işlemler tekrarladığı için bunuda generic yapıya çekecez
        //DataBaseContext db = new DataBaseContext();

        //Note burda değişen tek şey categorydal ya da productdal metotlar aynı 
        //ondan dolayı bunuda generic yapıyoruz EfCoreGenericRepository yapacaz

        // public class EfCoreProductDAL:IProductDAL


        public IEnumerable<Product> GetPopularProducts()
        {
            return GetAll().ToList();
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using (var context = new DataBaseContext())
            {
                //list olarak tanımladığımız asqoeyable yaptık sorgu filterlremesi yapalım
                var products = context.Products.AsQueryable();

                //category bilgisi var mı yokmu
                //products/telefon
                //isnullempty : dışarıdan parametre okarak verilen değer boşsa true döndürür
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                    //any ile products içinde category bilgisi var mı onu kontrol ediyoruz
                }
                //bütün product listesi
                //return products.ToList();

                //bütün pruductlar listelenmeden koşul yazoıyoruz
                //parametre oalrak 1 verdiğimizden
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public Product GetProductDetails(int id)
        {
            //Dispose etmek için yaptık
            using (var context = new DataBaseContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories) // burada producttan productcategory join
                    .ThenInclude(i => i.Category) // productcategoryden category join yapıyoruz
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new DataBaseContext())
            {
                //burada AsQueryable ile product içindeki verileri alıyoruz
                var products = context.Products.AsQueryable();


                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));

                }

                //totalli alıyoz
                return products.Count();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using (var context = new DataBaseContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }

        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new DataBaseContext())
            {
                var product = context.Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(i => i.Id == entity.Id);

                if (product != null)
                {
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.Price = entity.Price;

                    //Important asıl önemli olan yer burası
                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        CategoryId = catid,
                        ProductId = entity.Id
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}