using System.Collections.Generic;
using System.Threading;
using Tops.Test.Helper;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace Tops.Test.UnitTest
{
    public class ProductServiceTest
    {

        #region EntitiesUsage

        public class ProductDomain : IProductDomain
        {
            public int Id { get; set; }
            public int ApoClass { get; set; }
            public string Code { get; set; }
            public string ProductName { get; set; }
            public int Brand { get; set; }
            public string ProductDescription { get; set; }
        }

        private List<ProductDomain> _productDomains;
        private IProductRepository _productRepository;

        #endregion

        public ProductServiceTest()
        {
            _productDomains = DataInitializer.GetAllProductDomain();

        }
    }
}
