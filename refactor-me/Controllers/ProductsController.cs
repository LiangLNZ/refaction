using refactor_me.Models;
using refactor_me.Models.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Route]
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            var products = _productService.LoadProducts(string.Empty);
            return products;
        }

        [Route("{name}/search")]
        [HttpGet]
        public IEnumerable<Product> SearchByName(string name)
        {
            var products = _productService.LoadProducts(name);
            return products;
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            return CheckProductExist(id);
        }

        private Product CheckProductExist(Guid id)
        {
            var product = _productService.GetProductById(id);
            if (!product.IsNew) return product;

            var message = $"Product with Id = {id} not found";
            throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        }

        [Route]
        [HttpPost]
        public HttpResponseMessage Create([FromBody]NewProductRequest newProductRequest)
        {
            var newProduct = new Product
            {
                DeliveryPrice = newProductRequest.DeliveryPrice,
                Description = newProductRequest.Description,
                Id = Guid.NewGuid(),
                Name = newProductRequest.Name,
                Price = newProductRequest.Price,
                IsNew = true
            };
            _productService.SaveProduct(newProduct);
            return Request.CreateResponse(HttpStatusCode.OK, newProduct);
        }

        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage Update(Guid id, [FromBody]NewProductRequest newProductRequest)
        {
            //Check if product exists
            CheckProductExist(id);

            //Save updated product
            var updatedProduct = new Product
            {
                DeliveryPrice = newProductRequest.DeliveryPrice,
                Description = newProductRequest.Description,
                Id = id,
                Name = newProductRequest.Name,
                Price = newProductRequest.Price,
                IsNew = false
            };
            _productService.SaveProduct(updatedProduct);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            //Check if product exists
            CheckProductExist(id);

            //Delete product 
            _productService.DeleteProduct(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
