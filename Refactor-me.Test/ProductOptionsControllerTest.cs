using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCLMock;
using refactor_me.Controllers;
using refactor_me.Models;
using refactor_me.Models.Services.Mocks;

namespace Refactor_me.Test
{
    /// <summary>
    /// Summary description for ProductOptionsController
    /// </summary>
    [TestClass]
    public class ProductOptionsControllerTest
    {
        private readonly ProductOptionServiceMock _productOptionServiceMock = new ProductOptionServiceMock();
        private readonly ProductServiceMock _productServiceMock = new ProductServiceMock();
        private ProductOptionsController _sut;

        [TestInitialize]
        public void TestInit()
        {
            _sut = new ProductOptionsController(_productOptionServiceMock, _productServiceMock);
        }

        [TestMethod]
        public void ValidateProductOption_ProductOption_IsValid()
        {
            var productId = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c");
            var id = new Guid("fc9c7b02-2db7-4e4a-a395-72f49c3a520f");
             _productOptionServiceMock
                .When(svc=>svc.GetProductOptionByIds(It.Is(productId), It.Is(id)))
                .Return(() => new ProductOption{IsNew = false});

            var result = _sut.GetOption(productId, id);

            Assert.IsFalse(result.IsNew);
        }
    }
}
