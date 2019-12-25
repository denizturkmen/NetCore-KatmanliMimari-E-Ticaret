using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.DataAccessLayer.Concrete.EntityFrameWork;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        //Bu yanlış çnkü ProductManager, EfCoreProductDAL bağımlı oluyor
        //KAtmanlı mimariye ters
        //Concrete class concrete yapıyı çağırması yanlış
        //Dopru olan Interfa Interface üzerinden işlem yapması
        //EfCoreProductDAL _efCoreProductDal = new EfCoreProductDAL();

        
        //DAL katmanındaki ilişkili oldupu INTERFACE üzerinden metotları ulaşıp işlem yapacaz bunun içini
        private IProductDAL _productDal;

        //Dışardan parametre olarak gelen productdal bu sınıf içine alıyoruz
        public ProductManager(IProductDAL productDal)
        {
            _productDal = productDal;
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            return _productDal.GetProductsByCategory(category, page, pageSize);
        }

        public bool Create(Product entity)
        {
            if (Validate(entity))
            {
                _productDal.Create(entity);
                return true;
            }
            return false;
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _productDal.Update(entity, categoryIds);
        }

        public void Delete(Product entity)
        {
            _productDal.Delete(entity);
        }

        public Product GetProductDetails(int id)
        {
            return _productDal.GetProductDetails(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productDal.GetByIdWithCategories(id);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }

        public int GetCountByCategory(string category)
        {
            return _productDal.GetCountByCategory(category);
        }

        public string ErrorMessage { get; set; }

        public bool Validate(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz";
                isValid = false;
            }

            return isValid;
        }
    }
}
