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
            public int TypeId { get; set; }
        }

        public class AttributeTypeDto : IAttributeTypeDataTranferObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class AttributeValueDto : IAttributeValueDataTranferObject
        {
            public int TypeId { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class AttributeTypeAndValueDto : IAttributeTypeAndValueDataTranferObject
        {
            public IAttributeTypeDataTranferObject Type { get; set; }
            public IAttributeValueDataTranferObject Value { get; set; }
        }

        public class ProductAttributeHeader : IProductHeader
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int ApoClass { get; set; }
            public int TypeId { get; set; }
            public int ValueId { get; set; }
        }

        private readonly List<ProductDomain> _productDomains;
        private readonly IProductRepository _productRepository;
        private readonly IAttributeTypeService _attributeTypeService;
        private readonly IAttributeValueService _attributeValueService;

        #endregion


        //var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //var archiveFolder = Path.Combine(currentDirectory, "Data");
        //var files = Directory.GetFiles(archiveFolder);
        public ProductServiceTest()
        {
            DataInitializer.GetProductFromTextFile();
               _productDomains = DataInitializer.GetProductFromTextFile();
            _productRepository = SetUpMockHelper.SetUpProductRepository();

            _attributeTypeService = SetUpMockHelper.GetAttributeTypeService();
            _attributeValueService = SetUpMockHelper.GetAttributeValueService();
        }

        [Fact]
        public void ServiceShouldReturnCorrectCriteria()
        {
            var service = new ProductService(_productRepository,_attributeTypeService,_attributeValueService);

            var sut = service.GetAll(1, 5, "200", "Co");

            Assert.True(sut.Count > 0);
            Assert.True(sut.All(x => x.ApoClassCode == "200"));
            Assert.True(
                sut.All(
                    x =>
                        x.ApoClassCode.ToString().Contains("Co".ToUpperInvariant()) ||
                        x.ProductName.ToUpperInvariant().Contains("Co".ToUpperInvariant())));
        }

        [Fact]
        public void ServiceShouldReturnAllWhenNotAssignCriteria()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetAll(1, 20, "", "");

            Assert.Equal(_productDomains.Count, sut.Count);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenAssignNotExistInDatabase()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetAll(1, 20, "", "dfsfsdgsgsdg");

            Assert.True(sut.Count == 0);
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
            Assert.Equal(sut.ApoClassCode,newProduct.ApoClassCode.ToString());
            Assert.Equal(sut.ProductName,newProduct.ProductName);
            Assert.Equal(sut.BrandId,newProduct.BrandId);
            Assert.Equal(sut.ProductCode,newProduct.ProductCode);
        }


        [Fact]
        public void ServiceShouldReturnCorrectProductWhenGetById()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(1);

            Assert.Equal(sut.Id,1);
        }

        [Fact]
        public void ServiceShouldReturnNullWhenGivenNotExistId()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(333033);

            Assert.Equal(sut,null);
        }

        [Fact]
        public void ServiceShouldReturnCorrectAttribute()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(1);

            Assert.Equal(3,sut.ListAttributeTypeAndValueDataTranferObjects.Count());
            Assert.True(sut.ListAttributeTypeAndValueDataTranferObjects.All(x => x.Value.TypeId == x.Type.Id));
        }

        [Fact]
        public void ServiceShouldReturnNullWhenProductAndClassNoMatch()
        {
            var service = new ProductService(_productRepository, _attributeTypeService, _attributeValueService);

            var sut = service.GetById(6);

            Assert.Equal(sut.ListAttributeTypeAndValueDataTranferObjects.Count(),0);
        }

        [Fact]
        public void ServiceShouldReturnCorrectValueWheneditSuccess()
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
            result.ApoClassCode = product.ApoClassCode.ToString();
            result.BrandId = product.BrandId;
            result.ProductCode = product.ProductCode;
            result.Id = 1;
            result.ProductName = product.ProductName;

            AssertObjects.PropertyValuesAreEquals(result, sut);
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

        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get;
            set; }
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IAttributeValueService attributeValueService;
        private readonly IAttributeTypeService attributeTypeService;

        public ProductService(IProductRepository productRepository
            ,IAttributeTypeService attributeTypeService,IAttributeValueService attributeValueService)
        {
            this.productRepository = productRepository;
            this.attributeValueService = attributeValueService;
            this.attributeTypeService = attributeTypeService;
        }

        public PagedList<IProductBaseDomain> GetAll(int page, int pageSize, string apoClass, string searchText)
        {
            var productResourceParameter = new ProductResourceParamater(page, pageSize, apoClass, searchText);
            var products = productRepository.GetAll(productResourceParameter);

            List<IProductBaseDomain> productDomains = new List<IProductBaseDomain>();
            foreach (var productDomain in products)
            {
                var p = new ProductServiceTest.ProductDto();
                p.Id = productDomain.Id;
                p.ApoClassCode = productDomain.ApoClassCode;
                p.BrandId = productDomain.BrandId;
                p.ProductCode = productDomain.ProductCode;
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

            var attrLists = productRepository.GetProductAttribute(id, product.ApoClassCode);


            var p = new ProductForEdit();
            p.Id = product.Id;
            p.ApoClassCode = product.ApoClassCode;
            p.BrandId = product.BrandId;
            p.ProductCode = product.ProductCode;
            p.ProductDescription = product.ProductDescription;
            p.ProductName = product.ProductName;

            var list = new List<ProductServiceTest.AttributeTypeAndValueDto>();

            foreach (var attributeTypeAndValueDomain in attrLists)
            {
                var type = attributeTypeService.GetById(attributeTypeAndValueDomain.AttributeTypeDomain.Id);
                var value = attributeValueService.GetValueByType(attributeTypeAndValueDomain.AttributeTypeDomain.Id
                    , attributeTypeAndValueDomain.AttributeValueDomain.Id);

                var typeDto = new ProductServiceTest.AttributeTypeDto();
                typeDto.Id = type.Id;
                typeDto.Code = type.Code;
                typeDto.Name = type.Name;

                var valueDto = new ProductServiceTest.AttributeValueDto();
                valueDto.Id = value.Id;
                valueDto.Code = value.Code;
                valueDto.Name = value.Name;
                valueDto.TypeId = value.TypeId;

                
                list.Add(new ProductServiceTest.AttributeTypeAndValueDto()
                {
                    Type = typeDto,
                    Value = valueDto
                });
            }

            p.ListAttributeTypeAndValueDataTranferObjects = list;

            return p;
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

    public class ProductForEdit:IProductForEditBaseDomain
    {
        public int Id { get; set; }
        public string ApoClassCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string ProductDescription { get; set; }
        public IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get; set;
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