using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.Entities.Entity;

namespace ShopApp.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDAL _categoryDal;


        public CategoryManager(ICategoryDAL categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public Category GetByIdWithProducts(int id)
        {
            return _categoryDal.GetByIdWithProducts(id);
        }

        public void Create(Category entity)
        {
           _categoryDal.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryDal.Delete(entity);
        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryDal.DeleteFromCategory(categoryId,productId);
        }


        public Category GetById(int id)
        {
            return _categoryDal.GetById(id);
        }

        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }

        public void Update(Category entity)
        {
           _categoryDal.Update(entity);
        }
    }
}

