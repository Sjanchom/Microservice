﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Tops.Test.UnitTest;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsShareClass.Models.Domain;
using TopsShareClass.Models.DataTranferObjects;

namespace Tops.Test.Helper
{
    public class SetUpMockHelper
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
                .Returns(new Func<IBaseResourceParameter, IQueryable<ApoDivisionDomain>>(
                    resourceParameter =>
                    {
                        return apoDivision.Where(x => string.IsNullOrWhiteSpace(resourceParameter.SearchText)
                                                      || x.Name.ToLowerInvariant()
                                                          .ToLowerInvariant()
                                                          .Contains(resourceParameter.SearchText.ToLowerInvariant())
                                                      ||
                                                      x.Code.ToLowerInvariant()
                                                          .Contains(resourceParameter.SearchText.ToLowerInvariant()))
                                                          .Where(x => x.IsActive == 1)
                                                          .AsQueryable();
                    }));

            repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, IApoDivisionDomain>(id =>
                {
                    return apoDivision.SingleOrDefault(x => x.Id == id && x.IsActive == 1);
                }));


            repository.Setup(x => x.Add(It.IsAny<IApoDivisionDomain>()))
                .Returns(new Func<IApoDivisionDomain, IApoDivisionDomain>(apoAddOrEdit =>
                {
                    dynamic maxId = apoDivision.Last().Id;
                    var nextId = Convert.ToInt32(maxId) + 1;
                    var nextCode = (Convert.ToInt32(apoDivision.Last().Code) + 1).ToString("D2");
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

            repository.Setup(x => x.GetAll())
                .Returns(new Func<IEnumerable<IApoDivisionDomain>>(() => apoDivision.ToList()));
            return repository.Object;
        }

        public static IApoGroupRepository GetApoGroupRepository()
        {
            var apoGroup = DataInitializer.GetApoGroup();

            var repository = new Mock<IApoGroupRepository>();

            repository.Setup(x => x.GetAll(It.IsAny<IApoGroupResourceParameter>()))
                .Returns(new Func<IApoGroupResourceParameter, IQueryable<IApoGroupDomain>>(apoGroupResourceParameter =>
                {
                    return apoGroup.Where(x => (!apoGroupResourceParameter.ApoDivsionId.HasValue
                                                || apoGroupResourceParameter.ApoDivsionId.Value == x.DivisionId)
                                               && (string.IsNullOrWhiteSpace(apoGroupResourceParameter.SearchText)
                                                   || x.Name.ToLowerInvariant()
                                                       .Contains(
                                                           apoGroupResourceParameter.SearchText.ToLowerInvariant())
                                                   || x.Code.ToLowerInvariant()
                                                       .Contains(
                                                           apoGroupResourceParameter.SearchText.ToLowerInvariant())))
                        .AsQueryable();
                }));

            repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Func<int, IApoGroupDomain>(id =>
                {
                    return apoGroup.SingleOrDefault(x => x.Id == id);
                } ));


            repository.Setup(x => x.Add(It.IsAny<IApoGroupDomain>()))
                .Returns(new Func<IApoGroupDomain, IApoGroupDomain>(apoAddOrEdit =>
                {
                    dynamic maxId = apoGroup.Last().Id;
                    var nextId = Convert.ToInt32(maxId) + 1;
                    var nextCode = (Convert.ToInt32(apoGroup.Last().Code) + 1).ToString("D2");
                    apoAddOrEdit.Id = (int)nextId;
                    apoAddOrEdit.Code = nextCode;
                    apoGroup.Add(apoAddOrEdit as ApoGroupDomain);

                    return apoAddOrEdit;
                }));

            repository.Setup(x => x.GetByName(It.IsAny<IApoGroupForCreateOrEdit>()))
                .Returns(new Func<IApoGroupForCreateOrEdit, ApoGroupDomain>(apoAddOrEdit =>
                {
                    return apoGroup.FirstOrDefault(x => x.Name.ToLowerInvariant()
                        .Equals(apoAddOrEdit.Name.Trim().ToLowerInvariant()));
                }));


            repository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<IApoGroupDomain>()))
                .Returns(new Func<int, IApoGroupDomain, IApoGroupDomain>((id, apoDivisionDomain) =>
                {
                    var apoDiv = apoGroup.SingleOrDefault(x => x.Id == id);

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
                        return apoGroup.Single(x => x.Id == id) != null;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }));

            repository.Setup(x => x.GetByApoDivision(It.IsAny<int>()))
                .Returns(new Func<int, IQueryable<IApoGroupDomain>>(id =>
                {
                    return apoGroup.Where(x => x.DivisionId == id).AsQueryable();
                }));

            repository.Setup(x => x.GetAll())
                .Returns(new Func<IEnumerable<IApoGroupDomain>>(() => apoGroup.ToList()));

            return repository.Object;
        }

        public static IApoGroupService GetApoGroupService()
        {
            var apoGroup = DataInitializer.GetApoGroup();

            var repository = new Mock<IApoGroupService>();


            repository.Setup(x => x.GetApoGroupByApoDivision(It.IsAny<int>()))
                .Returns(new Func<int, IEnumerable<IApoGroupDataTranferObject>>(id =>
                {
                    var list = apoGroup.Where(x => x.DivisionId == id);
                    return Mapper.Map<List<ApoGroupDto>>(list);

                }));

            return repository.Object;
        }

        public static IApoDivisionService GetApoDivisionService()
        {
            var apoDivisions = DataInitializer.GetApoDivisions();

            var repository = new Mock<IApoGroupService>();


           
            return repository.Object;
        }
    }
}