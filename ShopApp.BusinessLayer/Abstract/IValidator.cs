using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.BusinessLayer.Abstract
{
    public interface IValidator<T>
    {
        string ErrorMessage { get; set; }
        bool Validate(T entity);
    }
}
