using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Tops.Test.UnitTest;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace Tops.Test.Helper
{
    internal class SetUpMockHelper
    {
        public static IProductRepository SetUpProductRepository()
        {
            var products = DataInitializer.GetProductFromTextFile();
            var attrType = DataInitializer.GetAllTypeAttributeTypeDomains();
            var attrValue = DataInitializer.GetAttributeValueDomains();
            var productHeader = DataInitializer.GetaProductAttributeHeaders();

            var repository = new Mock<IProductRepository>();

            repository.Setup(x => x.GetAll(It.IsAny<IProductResourceParameters>()))
                .Returns(new Func<IProductResourceParameters, IQueryable<IProductDomain>>(
                    productResourceParameters =>
                    {
                        return products.Where(p =>
                                string.IsNullOrWhiteSpace(productResourceParameters.SearchText)
                                || p.ProductName.ToUpperInvariant()
                                    .Contains(productResourceParameters.SearchText.ToUpperInvariant())
                                || p.ApoClassCode.ToString()
                                    .Contains(productResourceParameters.SearchText.ToUpperInvariant()))
                            .Where(x => string.IsNullOrEmpty(productResourceParameters.ApoClass)||
                                        x.ApoClassCode == productResourceParameters.ApoClass)
                            .AsQueryable();
                    }
                ));

            repository.Setup(x => x.Add(It.IsAny<IProductDomain>()))
                .Returns(new Func<IProductDomain, IProductDomain>(newProduct =>
                {
                    dynamic maxProductId = products.Last().Id;
                    var nextProductId = Convert.ToInt32(maxProductId) + 1;
                    newProduct.Id = (int) nextProductId;
                    products.Add(newProduct as ProductServiceTest.ProductDomain);

                    return newProduct;
                }));

            repository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<IProductDomain>()))
                .Returns(new Func<int, IProductDomain, IProductDomain>((id, product) =>
                {
                    var productDomain = products.Find(x => x.Id == id);
                    if (productDomain == null)
                        return null;
                    productDomain.BrandId = product.BrandId;
                    productDomain.ApoClassCode = product.ApoClassCode;
                    productDomain.ProductName = product.ProductName;
                    productDomain.ProductCode = product.ProductCode;
                    productDomain.ProductDescription = product.ProductDescription;

                    return productDomain;
                }));

            repository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback(new Action<int>(id =>
                {
                    var findIndex = products.FindIndex(x => x.Id == id);
                    products.RemoveAt(findIndex);
                }));

            repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, IProductDomain>(id => { return products.SingleOrDefault(x => x.Id == id); }));

            repository.Setup(x => x.GetProductAttribute(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new Func<int, int, IEnumerable<IAttributeTypeAndValueDomain>>(
                    (productId, apoClass) =>
                    {
                        var matchList = productHeader.Where(x => x.ApoClass.Equals(apoClass)
                                                                 && x.ProductId == productId)
                            .ToList();

                        var attrTypeAndValueList = new List<ProductServiceTest.AttributeTypeAndValueDomain>();

                        foreach (var productAttributeHeader in matchList)
                            attrTypeAndValueList.Add(new ProductServiceTest.AttributeTypeAndValueDomain
                            {
                                AttributeTypeDomain = new ProductServiceTest.AttributeTypeDomain
                                {
                                    Id = productAttributeHeader.TypeId
                                },
                                AttributeValueDomain = new ProductServiceTest.AttributeValueDomain
                                {
                                    Id = productAttributeHeader.ValueId
                                }
                            });

                        return attrTypeAndValueList;
                    }));


            return repository.Object;
        }

        public static IAttributeTypeService GetAttributeTypeService()
        {
            var attrType = DataInitializer.GetAllTypeAttributeTypeDomains();
            var repository = new Mock<IAttributeTypeService>();

            repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, IAttributeTypeDomain>(id =>
                {
                    return attrType.SingleOrDefault(x => x.Id == id);
                }));

            return repository.Object;
        }

        public static IAttributeValueService GetAttributeValueService()
        {
            var attrValue = DataInitializer.GetAttributeValueDomains();
            var repository = new Mock<IAttributeValueService>();

            repository.Setup(x => x.GetValueByType(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Func< int, int, IAttributeValueDomain>(
                    (typeId, valueId) =>
                    {
                        return attrValue.SingleOrDefault(x => x.TypeId == typeId && x.Id == valueId);
                    }));

            return repository.Object;
        }
    }
}