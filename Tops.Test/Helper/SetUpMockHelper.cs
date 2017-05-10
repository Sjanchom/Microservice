using System;
using System.Linq;
using Moq;
using Tops.Test.UnitTest;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace Tops.Test.Helper
{
    class SetUpMockHelper
    {
        public static IProductRepository SetUpProductRepository()
        {
            var products = DataInitializer.GetAllProductDomain();

            var repository = new Mock<IProductRepository>();

            repository.Setup(x => x.GetAll(It.IsAny<IProductResourceParameters>()))
                .Returns(new Func<IProductResourceParameters, IQueryable<IProductDomain>>(
                    productResourceParameters =>
                    {
                        return products.Where(p =>
                         string.IsNullOrWhiteSpace(productResourceParameters.SearchText) 
                         || p.ProductName.ToUpperInvariant().Contains(productResourceParameters.SearchText.ToUpperInvariant())
                         || p.ApoClass.ToString().Contains(productResourceParameters.SearchText.ToUpperInvariant()))
                         .Where(x => productResourceParameters.ApoClass == 0 || x.ApoClass == productResourceParameters.ApoClass)
                        .AsQueryable();
                    }
                ));

            repository.Setup(x => x.Add(It.IsAny<IProductDomain>()))
                  .Returns(new Func<IProductDomain,IProductDomain>(newProduct =>
                  {
                      dynamic maxProductId = products.Last().Id;
                      dynamic nextProductId = Convert.ToInt32(maxProductId) + 1;
                      newProduct.Id = (int)nextProductId;
                      products.Add(newProduct as ProductServiceTest.ProductDomain);

                      return newProduct;
                  }));

            repository.Setup(x => x.Update(It.IsAny<int>(),It.IsAny<IProductDomain>()))
                .Returns(new Func<int,IProductDomain, IProductDomain>((id,product) =>
                {
                    var productDomain = products.Find(x => x.Id == id);
                    if (productDomain == null)
                        return null;
                    productDomain.BrandId = product.BrandId;
                    productDomain.ApoClass = product.ApoClass;
                    productDomain.ProductName = product.ProductName;
                    productDomain.Code = product.Code;
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
                .Returns(new Func<int, IProductDomain>(id =>
                {
                    return products.SingleOrDefault(x => x.Id == id);
                }));


            return repository.Object;
        }
    }
}