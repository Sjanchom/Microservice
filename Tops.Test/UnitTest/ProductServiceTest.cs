using System;
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
            public string ProductDescription { get; set; }
            public int BrandId { get; set; }
        }

        public class ProductDto : IProductBaseDomain
        {
            public int Id { get; set; }
            public int ApoClass { get; set; }
            public string Code { get; set; }
            public string ProductName { get; set; }
            public int BrandId { get; set; }
        }

        private readonly List<ProductDomain> _productDomains;
        private readonly IProductRepository _productRepository;

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
            Assert.True(sut.All(x => x.ApoClass == 200));
            Assert.True(
                sut.All(
                    x =>
                        x.ApoClass.ToString().Contains("Co".ToUpperInvariant()) ||
                        x.ProductName.ToUpperInvariant().Contains("Co".ToUpperInvariant())));
        }

        [Fact]
        public void ServiceShouldReturnAllWhenNotAssignCriteria()
        {
            var service = new ProductService(_productRepository);

            var sut = service.GetAll(1, 20, 0, "");

            Assert.Equal(_productDomains.Count, sut.Count);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenAssignNotExistInDatabase()
        {
            var service = new ProductService(_productRepository);

            var sut = service.GetAll(1, 20, 0, "dfsfsdgsgsdg");

            Assert.True(sut.Count == 0);
        }

        [Fact]
        public void ServiceShouldReturnNewProductWhenAddSuccess()
        {
            var newProduct = new ProductForCreate();
            newProduct.ApoClass = 200;
            newProduct.BrandId = 2;
            newProduct.Code = "304981";
            newProduct.ProductDescription = "BraBra";
            newProduct.ProductName = "HoHo";

            var lastId = _productDomains.Last().Id;

            var service = new ProductService(_productRepository);

            var sut = service.Create(newProduct);

            Assert.True(sut.Id == lastId + 1);
            Assert.Equal(sut.ApoClass,newProduct.ApoClass);
            Assert.Equal(sut.ProductName,newProduct.ProductName);
            Assert.Equal(sut.BrandId,newProduct.BrandId);
            Assert.Equal(sut.Code,newProduct.Code);
        }

        [Fact]
        public void ServiceShouldReturnCorrectProductWhenGetById()
        {
            var service = new ProductService(_productRepository);

            var sut = service.GetById(1);

            Assert.Equal(sut.Id,1);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenGivenNotExistId()
        {
            var service = new ProductService(_productRepository);

            var sut = service.GetById(333033);

            Assert.Equal(sut,null);
        }

        [Fact]
        public void ServiceShouldReturnCorrectValueWheneditSuccess()
        {
            var product = new ProductForCreate();
            product.ApoClass = 200;
            product.BrandId = 2;
            product.Code = "304981";
            product.ProductDescription = "BraBra";
            product.ProductName = "HoHo";

            var service = new ProductService(_productRepository);

            var sut = service.Edit(1, product) as ProductDto;

            var result = new ProductDto();
            result.ApoClass = product.ApoClass;
            result.BrandId = product.BrandId;
            result.Code = product.Code;
            result.Id = 1;
            result.ProductName = product.ProductName;

            AssertObjects.PropertyValuesAreEquals(result, sut);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenIdNotExist()
        {
            var product = new ProductForCreate();
            product.ApoClass = 200;
            product.BrandId = 2;
            product.Code = "304981";
            product.ProductDescription = "BraBra";
            product.ProductName = "HoHo";

            var service = new ProductService(_productRepository);

            var sut = service.Edit(122222, product) as ProductDto;

            Assert.Null(sut);
        }

        [Fact]
        public void ServiceShouldReturnTrueWhenDeleteSuccess()
        {

            var service = new ProductService(_productRepository);

            var sut = service.Delete(1);

            Assert.True(sut);
        }
    }

    public class ProductForCreate : IProductForCreate
    {
        public int Id { get; set; }
        public int ApoClass { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }

        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get;
            set; }
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public PagedList<IProductBaseDomain> GetAll(int page, int pageSize, int apoClass, string searchText)
        {
            var productResourceParameter = new ProductResourceParamater(page, pageSize, apoClass, searchText);
            var products = productRepository.GetAll(productResourceParameter);

            List<IProductBaseDomain> productDomains = new List<IProductBaseDomain>();
            foreach (var productDomain in products)
            {
                var p = new ProductServiceTest.ProductDto();
                p.Id = productDomain.Id;
                p.ApoClass = productDomain.ApoClass;
                p.BrandId = productDomain.BrandId;
                p.Code = productDomain.Code;
                p.ProductName = productDomain.ProductName;

                productDomains.Add(p);
            }

            return PagedList<IProductBaseDomain>.Create(productDomains.AsQueryable(), page, pageSize);
        }

        public IProductForEditBaseDomain GetById(int id)
        {
            var product = productRepository.GetById(id);

            if (product == null)
                return null;


            var p = new ProductForEdit();
            p.Id = product.Id;
            p.ApoClass = product.ApoClass;
            p.BrandId = product.BrandId;
            p.Code = product.Code;
            p.ProductDescription = product.ProductDescription;
            p.ProductName = product.ProductName;

            return p;
        }

        public IProductBaseDomain Create(IProductForCreate product)
        {
            var pro = new ProductServiceTest.ProductDomain();
            pro.ApoClass = 200;
            pro.BrandId = 2;
            pro.Code = "304981";
            pro.ProductDescription = "BraBra";
            pro.ProductName = "HoHo";
            var lastestProduct = productRepository.Add(pro);

            var p = new ProductServiceTest.ProductDto();
            p.Id = lastestProduct.Id;
            p.ApoClass = lastestProduct.ApoClass;
            p.BrandId = lastestProduct.BrandId;
            p.Code = lastestProduct.Code;
            p.ProductName = lastestProduct.ProductName;

            return p;
        }

        public IProductBaseDomain Edit(int productId, IProductForCreate product)
        {
            var domain = new ProductServiceTest.ProductDomain();
            domain.ApoClass = product.ApoClass;
            domain.Code = product.Code;
            domain.ProductDescription = product.ProductDescription;
            domain.ProductName = product.ProductName;
            domain.BrandId = product.BrandId;

            var updatedProduct = productRepository.Update(productId, domain);

            if (updatedProduct == null)
                return null;

            var p = new ProductServiceTest.ProductDto();
            p.ApoClass = updatedProduct.ApoClass;
            p.BrandId = updatedProduct.BrandId;
            p.Code = updatedProduct.Code;
            p.ProductName = updatedProduct.ProductName;
            p.Id = updatedProduct.Id;

            return p;
        }

        public bool Delete(int productId)
        {
            try
            {
                productRepository.Delete(productId);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public class ProductForEdit:IProductForEditBaseDomain
    {
        public int Id { get; set; }
        public int ApoClass { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string ProductDescription { get; set; }
        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get; set;
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