using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Models.Services;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionService _productOptionService;
        private readonly IProductService _productService;

        public ProductOptionsController(IProductOptionService productOptionService, IProductService productService)
        {
            _productOptionService = productOptionService;
            _productService = productService;
        }


        [Route("{productId}/options")]
        [HttpGet]
        public IEnumerable<ProductOption> GetOptions(Guid productId)
        {
            var productOptions = _productOptionService.LoadProductOptions(productId);
            return productOptions;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            return CheckProductOptionExistByIds(productId, id);
        }

        private ProductOption CheckProductOptionExistByIds(Guid productId, Guid id)
        {
            var option = _productOptionService.GetProductOptionByIds(productId, id);
            if (!option.IsNew) return option;

            var message = $"Product option not found with ProductId = {productId} and ProductOptionId = {id}";
            throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        }

        [Route("{productId}/options")]
        [HttpPost]
        public HttpResponseMessage CreateOption(Guid productId, [FromBody]NewProductOptionRequest optionRequest)
        {
            //Check if the productId exist 
            var product = _productService.GetProductById(productId);
            if (product.IsNew)
            {
                var message = $"Product with id = {productId} not found";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            //Save product option 
            var newProductOption = new ProductOption
            {
                Description = optionRequest.Description,
                Id = Guid.NewGuid(),
                Name = optionRequest.Name,
                ProductId = productId,
                IsNew = true
            };
            _productOptionService.SaveProductOption(newProductOption);
            return Request.CreateResponse(HttpStatusCode.OK, newProductOption);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateOption(Guid productId, Guid id, [FromBody]NewProductOptionRequest optionRequest)
        {
            //Check if the product and options exist
            CheckProductOptionExistByIds(productId, id);

            //save updated option 
            var updatedOption = new ProductOption
            {
                Description = optionRequest.Description,
                Id = id,
                Name = optionRequest.Name,
                ProductId = productId,
                IsNew = false
            };
            _productOptionService.SaveProductOption(updatedOption);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteOption(Guid productId, Guid id)
        {
            //Check if the product and options exist
            CheckProductOptionExistByIds(productId, id);
            //Delete option
            _productOptionService.DeleteProductOption(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}