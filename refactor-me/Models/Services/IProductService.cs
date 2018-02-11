using System;
using System.Collections.Generic;

namespace refactor_me.Models.Services
{
    public interface IProductService
    {
        IEnumerable<Product> LoadProducts(string name);
        Product GetProductById(Guid id);
        void SaveProduct(Product product);
        void DeleteProduct(Guid id);
    }
}