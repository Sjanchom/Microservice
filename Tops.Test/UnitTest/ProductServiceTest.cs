using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using Xunit;

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

        public class ProductDto : IProductBaseDomain
        {
            public int Id { get; set; }
            public int ApoClass { get; set; }
            public string Code { get; set; }
            public string ProductName { get; set; }
            public int Brand { get; set; }
        }

        private List<ProductDomain> _productDomains;
        private IProductRepository _productRepository;

        #endregion

        public ProductServiceTest()
        {
            _productDomains = DataInitializer.GetAllProductDomain();
            _productRepository = SetUpMockHelper.SetUpProductRepository();
        }

        [Fact]
        public void ServiceShouldReturnCorrectCriteria()
        {
            var service = new ProductService(_productRepository);

            var sut = service.GetAll(1, 5, 200, "Co");

            Assert.True(sut.Count > 0);
            Assert.True(sut.All(x => x.ApoClass.ToString().Contains("Co".ToUpperInvariant()) || x.ProductName.ToUpperInvariant().Contains("Co".ToUpperInvariant())));
        }
    }

    public class ProductService:IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public PagedList<IProductBaseDomain> GetAll(int page, int pageSize, int apoClass, string searchText)
        {
            var productResourceParameter = new ProductResourceParamater(page,pageSize,apoClass,searchText);
            var products = productRepository.GetAll(productResourceParameter);

            List<IProductBaseDomain> productDomains = new List<IProductBaseDomain>();
            foreach (var productDomain in products)
            {
                var p = new ProductServiceTest.ProductDto();
                p.Id = productDomain.Id;
                p.ApoClass = productDomain.ApoClass;
                p.Brand = productDomain.Brand;
                p.Code = productDomain.Code;
                p.ProductName = productDomain.ProductName;

                productDomains.Add(p);
            }

            return PagedList<IProductBaseDomain>.Create(productDomains.AsQueryable(), page, pageSize);
        }

        public IProductForEditBaseDomain GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IProductForEditBaseDomain Create(IProductForCreate product)
        {
            throw new System.NotImplementedException();
        }

        public IProductForEditBaseDomain Edit(int proudctId, IProductForCreate product)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int productId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ProductResourceParamater : IProductResourceParameters
    {
        public ProductResourceParamater(int page, int pageSize, int apoClass, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
            ApoClass = apoClass;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int ApoClass { get; set; }
    }
}
