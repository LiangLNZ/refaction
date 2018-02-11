using refactor_me.Helpers;
using System;
using System.Collections.Generic;

namespace refactor_me.Models.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IDataProviderFactory _dataProviderFactory;

        public ProductOptionService(IDataProviderFactory dataProviderFactory)
        {
            _dataProviderFactory = dataProviderFactory;
        }

        public IEnumerable<ProductOption> LoadProductOptions(Guid productId)
        {
            var where = $"where productid = '{productId}'";
            var cmdText = $"select * from productoption {where}";
            var cmd = _dataProviderFactory.RunCommand(cmdText, Constants.SqlDbConnection);

            using (cmd.Connection)
            {
                var items = new List<ProductOption>();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var productOption = new ProductOption
                        {
                            Id = Guid.Parse(rdr["id"].ToString()),
                            ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString()
                        };
                        items.Add(productOption);
                    }
                }
                return items;
            }
        }

        public ProductOption GetProductOptionByIds(Guid productId, Guid id)
        {
            var productOption = new ProductOption {IsNew = true};
            var cmdText = $"select * from productoption where id = '{id}' and productId = '{productId}'";
            var cmd = _dataProviderFactory.RunCommand(cmdText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read())
                        return productOption;
                    productOption.IsNew = false;
                    productOption.Id = Guid.Parse(rdr["Id"].ToString());
                    productOption.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                    productOption.Name = rdr["Name"].ToString();
                    productOption.Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString();
                }
                return productOption;
            }
        }

        public void SaveProductOption(ProductOption productOption)
        {
            var cmdText = productOption.IsNew ?
                $"insert into productoption (id, productid, name, description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')" :
                $"update productoption set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}'";
            var cmd = _dataProviderFactory.RunCommand(cmdText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                cmd.ExecuteNonQuery();
            } 
        }

        public void DeleteProductOption(Guid id)
        {
            var cmdText = $"delete from productoption where id = '{id}'";
            var cmd = _dataProviderFactory.RunCommand(cmdText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                cmd.ExecuteReader();
            }
        }
    }
}