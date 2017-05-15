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
            var productDetail = DataInitializer.GetaProductAttributeHeaders();

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
                            .Where(x => string.IsNullOrEmpty(productResourceParameters.ApoClass) ||
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
                .Returns(new Func<int, string, IEnumerable<IAttributeTypeAndValueDomain>>(
                    (productId, apoClass) =>
                    {
                        var matchList = productDetail.Where(x => x.ApoClass.Equals(apoClass)
                                                                 && x.ProductId.Equals(productId.ToString()))
                            .ToList();

                        var attrTypeAndValueList = new List<ProductServiceTest.AttributeTypeAndValueDomain>();

                        foreach (var productAttributeHeader in matchList)
                            attrTypeAndValueList.Add(new ProductServiceTest.AttributeTypeAndValueDomain
                            {
                                AttributeTypeDomain = new ProductServiceTest.AttributeTypeDomain
                                {
                                    Id = Convert.ToInt32(productAttributeHeader.TypeId)
                                },
                                AttributeValueDomain = new ProductServiceTest.AttributeValueDomain
                                {
                                    Id = Convert.ToInt32(productAttributeHeader.ValueId)
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
                .Returns(new Func<int, IAttributeTypeDomain>(
                    id => { return attrType.SingleOrDefault(x => x.Id == id); }));

            return repository.Object;
        }

        public static IAttributeValueService GetAttributeValueService()
        {
            var attrValue = DataInitializer.GetAttributeValueDomains();
            var repository = new Mock<IAttributeValueService>();

            repository.Setup(x => x.GetValueByType(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Func<int, int, IAttributeValueDomain>(
                    (typeId, valueId) =>
                    {
                        return attrValue.SingleOrDefault(x => x.TypeId == typeId.ToString() && x.Id == valueId);
                    }));

            return repository.Object;
        }

        public static IApoDivisionRepository GetApoDivisionRepository()
        {
            var apoDivision = DataInitializer.GetApoDivisions();
            var repository = new Mock<IApoDivisionRepository>();

            repository.Setup(x => x.GetAll(It.IsAny<IBaseResourceParameter>()))
                .Returns(new Func<IBaseResourceParameter, IEnumerable<ApoDivisionDomain>>(
                    resourceParameter =>
                    {
                        return apoDivision.Where(x => string.IsNullOrWhiteSpace(resourceParameter.SearchText)
                                                      || x.Name.ToLowerInvariant()
                                                          .ToLowerInvariant()
                                                          .Contains(resourceParameter.SearchText.ToLowerInvariant())
                                                      ||
                                                      x.Code.ToLowerInvariant()
                                                          .Contains(resourceParameter.SearchText.ToLowerInvariant()));
                    }));

            repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, IApoDivisionDomain>(id =>
                {
                    return apoDivision.SingleOrDefault(x => x.Id == id);
                }));


            repository.Setup(x => x.Add(It.IsAny<IApoDivisionDomain>()))
                .Returns(new Func<IApoDivisionDomain, IApoDivisionDomain>(apoAddOrEdit =>
                {
                    dynamic maxId = apoDivision.Last().Id;
                    var nextId = Convert.ToInt32(maxId) + 1;
                    var nextCode = (Convert.ToInt32(apoDivision.Last().Code) + 1).ToString("D3");
                    apoAddOrEdit.Id = (int) nextId;
                    apoAddOrEdit.Code = nextCode;
                    apoDivision.Add(apoAddOrEdit as ApoDivisionDomain);

                    return apoAddOrEdit;
                }));

            repository.Setup(x => x.GetByName(It.IsAny<IApoDivisionForCreateOrEdit>()))
                .Returns(new Func<IApoDivisionForCreateOrEdit, IApoDivisionDomain>(apoAddOrEdit =>
                {
                    return apoDivision.FirstOrDefault(x => x.Name.ToLowerInvariant()
                        .Equals(apoAddOrEdit.Name.Trim().ToLowerInvariant()));
                }));

            repository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<IApoDivisionDomain>()))
                .Returns(new Func<int, IApoDivisionDomain, IApoDivisionDomain>((id, apoDivisionDomain) =>
                {
                    var apoDiv = apoDivision.SingleOrDefault(x => x.Id == id);

                    if (apoDiv != null)
                    {
                        apoDiv.Name = apoDivisionDomain.Name;

                        return apoDiv;
                    }

                    return null;
                }));

            repository.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(new Func<int, bool>(id =>
                {
                    try
                    {
                        return apoDivision.Single(x => x.Id == id) != null;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }));
            return repository.Object;
        }
    }
}