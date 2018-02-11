using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCLMock;
using refactor_me.Models.Services.Mocks;
using refactor_me.Controllers;
using refactor_me.Models;

namespace Refactor_me.Test
{
    [TestClass]
    public class ProductControllerTest
    {
        private readonly ProductServiceMock _productServiceMock = new ProductServiceMock();
        private ProductsController _sut;

        [TestInitialize]
        public void TestInit()
        {
            _sut = new ProductsController(_productServiceMock);
        }

        [TestMethod]
        public void ValidateProduct_Product_IsValid()
        {
            _productServiceMock
                .When(svc=>svc.GetProductById(It.IsAny<Guid>()))
                .Return(() => new Product{IsNew = false});
            var result = _sut.GetProduct(new Guid("fc9c7b02-2db7-4e4a-a395-72f49c3a520f"));

            Assert.IsFalse(result.IsNew);
        }
    }
}
