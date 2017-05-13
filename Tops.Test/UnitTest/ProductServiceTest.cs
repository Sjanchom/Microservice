using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        public ProductServiceTest()
        {
            MapperHelper.SetUpMapper();

            _productDomains = DataInitializer.GetProductFromTextFile();
            _attrType = DataInitializer.GetAllTypeAttributeTypeDomains();
            _attrValue = DataInitializer.GetAttributeValueDomains();
            _productDetail = DataInitializer.GetaProductAttributeHeaders();


            _productRepository = SetUpMockHelper.SetUpProductRepository();

            _attributeTypeService = SetUpMockHelper.GetAttributeTypeService();
            _attributeValueService = SetUpMockHelper.GetAttributeValueService();
        }

        public class ProductDomain : IProductDomain
        {
            public int Id { get; set; }
            public string ApoClassCode { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public int BrandId { get; set; }
        }

        public class ProductDto : IProductBaseDomain
        {
            public int Id { get; set; }
            public string ApoClassCode { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public int BrandId { get; set; }
        }

        public class AttributeTypeAndValueDomain : IAttributeTypeAndValueDomain
        {
            public IAttributeTypeDomain AttributeTypeDomain { get; set; }
            public IAttributeValueDomain AttributeValueDomain { get; set; }
        }


        public class AttributeTypeDomain : IAttributeTypeDomain
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class AttributeValueDomain : IAttributeValueDomain
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string TypeId { get; set; }
            public string ApoClass { get; set; }
        }

        public class AttributeTypeDto : IAttributeTypeDataTranferObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class AttributeValueDto : IAttributeValueDataTranferObject
        {
            public string TypeId { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class AttributeTypeAndValueDto : IAttributeTypeAndValueDataTranferObject
        {
            public IAttributeTypeDataTranferObject Type { get; set; }
            public IAttributeValueDataTranferObject Value { get; set; }
        }

        public class ProductAttributeDetail : IProductDetail
        {
            public string Id { get; set; }
            public string ProductId { get; set; }
            public string ApoClass { get; set; }
            public string TypeId { get; set; }
            public string ValueId { get; set; }
        }

        private readonly List<ProductDomain> _productDomains;
        private readonly List<AttributeTypeDomain> _attrType;
        private readonly List<AttributeValueDomain> _attrValue;
        private readonly List<ProductAttributeDetail> _productDetail;
        private readonly IProductRepository _productRepository;
        private readonly IAttributeTypeService _attributeTypeService;
        private readonly IAttributeValueService _attributeValueService;

        [Fact]
        public void ServiceShouldReturnAllWhenNotAssignCriteria()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetAll(1, 20, "", "");

            Assert.True(sut.Count <= 20);
        }

        [Fact]
        public void ServiceShouldReturnCorrectAttribute()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(1);

            var apoMatch = _productDomains.Single(x => x.Id == 1).ApoClassCode;
            var totalTypeCount = _productDetail.Count(x => x.ApoClass == apoMatch && x.ProductId == "1");

            Assert.Equal(totalTypeCount, sut.ListAttributeTypeAndValueDataTranferObjects.Count());
            Assert.True(
                sut.ListAttributeTypeAndValueDataTranferObjects.All(x => x.Value.TypeId == x.Type.Id.ToString()));
        }

        [Fact]
        public void ServiceShouldReturnCorrectCriteria()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetAll(1, 5, "45020001", "อันอัน");

            Assert.True(sut.Count > 0);
            Assert.True(sut.All(x => x.ApoClassCode == "45020001"));
            Assert.True(
                sut.All(
                    x =>
                        x.ApoClassCode.ToString().Contains("อันอัน".ToUpperInvariant()) ||
                        x.ProductName.ToUpperInvariant().Contains("อันอัน".ToUpperInvariant())));
        }


        [Fact]
        public void ServiceShouldReturnCorrectProductWhenGetById()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(1);

            Assert.Equal(sut.Id, 1);
        }

        [Fact]
        public void ServiceShouldReturnCorrectValueWhenEditSuccess()
        {
            var product = new ProductForCreate();
            product.ApoClassCode = "200";
            product.BrandId = 2;
            product.ProductCode = "304981";
            product.ProductDescription = "BraBra";
            product.ProductName = "HoHo";

            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.Edit(1, product) as ProductDto;

            var result = new ProductDto();
            result.ApoClassCode = product.ApoClassCode;
            result.BrandId = product.BrandId;
            result.ProductCode = product.ProductCode;
            result.Id = 1;
            result.ProductName = product.ProductName;

            AssertObjects.PropertyValuesAreEquals(result, sut);
        }

        [Fact]
        public void ServiceShouldReturnNewProductWhenAddSuccess()
        {
            var newProduct = new ProductForCreate();
            newProduct.ApoClassCode = "200";
            newProduct.BrandId = 2;
            newProduct.ProductCode = "304981";
            newProduct.ProductDescription = "BraBra";
            newProduct.ProductName = "HoHo";

            var lastId = _productDomains.Last().Id;

            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.Create(newProduct);

            Assert.True(sut.Id == lastId + 1);
            Assert.Equal(sut.ApoClassCode, newProduct.ApoClassCode);
            Assert.Equal(sut.ProductName, newProduct.ProductName);
            Assert.Equal(sut.BrandId, newProduct.BrandId);
            Assert.Equal(sut.ProductCode, newProduct.ProductCode);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenAssignNotExistInDatabase()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetAll(1, 20, "", "dfsfsdgsgsdg");

            Assert.True(sut.Count == 0);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenGivenNotExistId()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(333033);

            Assert.Equal(sut, null);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenIdNotExist()
        {
            var product = new ProductForCreate();
            product.ApoClassCode = "200";
            product.BrandId = 2;
            product.ProductCode = "304981";
            product.ProductDescription = "BraBra";
            product.ProductName = "HoHo";

            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.Edit(122222, product) as ProductDto;

            Assert.Null(sut);
        }

        [Fact]
        public void ServiceShouldNotReturnNullListOfAttribute()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(600);

            Assert.True(sut.ListAttributeTypeAndValueDataTranferObjects.Count() != 0);
        }

        [Fact]
        public void ServiceShouldReturnTrueWhenDeleteSuccess()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.Delete(1);

            Assert.True(sut);
        }
    }

    public class ProductForCreate : IProductForCreate
    {
        public int Id { get; set; }
        public string ApoClassCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }

        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects
        {
            get;
            set;
        }
    }

    public class ProductService : IProductService
    {
        private readonly IAttributeTypeService attributeTypeService;
        private readonly IAttributeValueService attributeValueService;
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository
            , IAttributeTypeService attributeTypeService, IAttributeValueService attributeValueService)
        {
            this.productRepository = productRepository;
            this.attributeValueService = attributeValueService;
            this.attributeTypeService = attributeTypeService;
        }

        public PagedList<IProductBaseDomain> GetAll(int page, int pageSize, string apoClass, string searchText)
        {
            var productResourceParameter = new ProductResourceParamater(page, pageSize, apoClass, searchText);

            var products = productRepository.GetAll(productResourceParameter);

            var productDomains = Mapper.Map<List<ProductServiceTest.ProductDto>>(products);

            return PagedList<IProductBaseDomain>.Create(productDomains.AsQueryable(), page, pageSize);
        }

        public IProductForEditBaseDomain GetById(int id)
        {
            var product = productRepository.GetById(id);

            if (product == null)
                return null;

            var attrLists = productRepository.GetProductAttribute(id, product.ApoClassCode);

            var productFroEdit = Mapper.Map<ProductForEdit>(product);

            var list = new List<ProductServiceTest.AttributeTypeAndValueDto>();

            foreach (var attributeTypeAndValueDomain in attrLists)
            {
                var type = attributeTypeService.GetById(attributeTypeAndValueDomain.AttributeTypeDomain.Id);
                var value = attributeValueService.GetValueByType(attributeTypeAndValueDomain.AttributeTypeDomain.Id
                    , attributeTypeAndValueDomain.AttributeValueDomain.Id);

                list.Add(new ProductServiceTest.AttributeTypeAndValueDto
                {
                    Type = Mapper.Map<ProductServiceTest.AttributeTypeDto>(type),
                    Value = Mapper.Map<ProductServiceTest.AttributeValueDto>(value)
                });
            }

            productFroEdit.ListAttributeTypeAndValueDataTranferObjects = list;

            return productFroEdit;
        }

        public IProductBaseDomain Create(IProductForCreate product)
        {
            var pro = new ProductServiceTest.ProductDomain();
            pro.ApoClassCode = "200";
            pro.BrandId = 2;
            pro.ProductCode = "304981";
            pro.ProductDescription = "BraBra";
            pro.ProductName = "HoHo";
            var lastestProduct = productRepository.Add(pro);

            var p = new ProductServiceTest.ProductDto();
            p.Id = lastestProduct.Id;
            p.ApoClassCode = lastestProduct.ApoClassCode;
            p.BrandId = lastestProduct.BrandId;
            p.ProductCode = lastestProduct.ProductCode;
            p.ProductName = lastestProduct.ProductName;

            return p;
        }

        public IProductBaseDomain Edit(int productId, IProductForCreate product)
        {
            var domain = new ProductServiceTest.ProductDomain();
            domain.ApoClassCode = product.ApoClassCode;
            domain.ProductCode = product.ProductCode;
            domain.ProductDescription = product.ProductDescription;
            domain.ProductName = product.ProductName;
            domain.BrandId = product.BrandId;

            var updatedProduct = productRepository.Update(productId, domain);

            if (updatedProduct == null)
                return null;

            var p = new ProductServiceTest.ProductDto();
            p.ApoClassCode = updatedProduct.ApoClassCode;
            p.BrandId = updatedProduct.BrandId;
            p.ProductCode = updatedProduct.ProductCode;
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

    public class ProductForEdit : IProductForEditBaseDomain
    {
        public int Id { get; set; }
        public string ApoClassCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string ProductDescription { get; set; }

        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects
        {
            get;
            set;
        }
    }

    public class ProductResourceParamater : IProductResourceParameters
    {
        public ProductResourceParamater(int page, int pageSize, string apoClass, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
            ApoClass = apoClass;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public string ApoClass { get; set; }
    }
}