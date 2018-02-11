using System;
using System.Collections.Generic;
using refactor_me.Helpers;

namespace refactor_me.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly IDataProviderFactory _dataProviderFactory;

        public ProductService(IDataProviderFactory dataProviderFactory)
        {
            _dataProviderFactory = dataProviderFactory;
        }

        public IEnumerable<Product> LoadProducts(string name)
        {
            var where = string.IsNullOrEmpty(name) ? null : $"where lower(name) like '%{name.ToLower()}%'";
            var commandText = $"select * from product {where}";

            var cmd = _dataProviderFactory.RunCommand(commandText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                var items = new List<Product>();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var product = new Product
                        {
                            Id = Guid.Parse(rdr["id"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        };
                        items.Add(product);
                    }
                }
                return items;
            }
        }

        public Product GetProductById(Guid id)
        {
            var product = new Product {IsNew = true};
            var commandText = $"select * from product where id = '{id}'";
            var cmd = _dataProviderFactory.RunCommand(commandText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read())
                        return product;

                    product.IsNew = false;
                    product.Id = Guid.Parse(rdr["Id"].ToString());
                    product.Name = rdr["Name"].ToString();
                    product.Description = DBNull.Value == rdr["Description"] ? null : rdr["Description"].ToString();
                    product.Price = decimal.Parse(rdr["Price"].ToString());
                    product.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                }
            }  
            return product;
        }

        public void SaveProduct(Product product)
        {
            var commandText = product.IsNew
                ? $"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})"
                : $"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'";
            var cmd = _dataProviderFactory.RunCommand(commandText, Constants.SqlDbConnection);
            using (cmd.Connection)
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(Guid id)
        {
            var delProductOptionCommandText = $"delete from productoption where ProductId = '{id}'";
            var delProductCommandText = $"delete from product where id = '{id}'";

            var deleteProductOptionsCmd =
                _dataProviderFactory.RunCommand(delProductOptionCommandText, Constants.SqlDbConnection);
            using (deleteProductOptionsCmd.Connection)
            {
                deleteProductOptionsCmd.ExecuteNonQuery();
            }

            var deleteProductCmd = _dataProviderFactory.RunCommand(delProductCommandText, Constants.SqlDbConnection);
            using (deleteProductCmd.Connection)
            {
                deleteProductCmd.ExecuteNonQuery();
            }
        }
    }
}