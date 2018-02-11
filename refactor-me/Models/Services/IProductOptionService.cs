using System;
using System.Collections.Generic;

namespace refactor_me.Models.Services
{
    public interface IProductOptionService
    {
        IEnumerable<ProductOption> LoadProductOptions(Guid productId);
        ProductOption GetProductOptionByIds(Guid productId, Guid id);
        void SaveProductOption(ProductOption productOption);
        void DeleteProductOption(Guid id);
    }
}
