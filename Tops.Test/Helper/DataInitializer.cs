using System.Collections.Generic;
using Tops.Test.UnitTest;

namespace Tops.Test.Helper
{
    public class DataInitializer
    {
        public static List<ProductServiceTest.ProductDomain> GetAllProductDomain()
        {
            return new List<ProductServiceTest.ProductDomain>()
            {
                new ProductServiceTest.ProductDomain
                {
                    Id = 1,
                    ApoClass = 200,
                    BrandId = 1,
                    Code = "100021",
                    ProductName = "Cookies"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 2,
                    ApoClass = 200,
                    BrandId = 2,
                    Code = "104021",
                    ProductName = "Biscuit"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 3,
                    ApoClass = 200,
                    BrandId = 3,
                    Code = "400021",
                    ProductName = "Be"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 4,
                    ApoClass = 200,
                    BrandId = 1,
                    Code = "234021",
                    ProductName = "FunO"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 5,
                    ApoClass = 200,
                    BrandId = 2,
                    Code = "220021",
                    ProductName = "Oreo"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 6,
                    ApoClass = 201,
                    BrandId = 3,
                    Code = "4520021",
                    ProductName = "Sigha"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 7,
                    ApoClass = 201,
                    BrandId = 1,
                    Code = "1120021",
                    ProductName = "Leo"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 8,
                    ApoClass = 201,
                    BrandId = 2,
                    Code = "455021",
                    ProductName = "Tiger"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 9,
                    ApoClass = 201,
                    BrandId = 2,
                    Code = "235021",
                    ProductName = "Chang"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 10,
                    ApoClass = 202,
                    BrandId = 1,
                    Code = "872021",
                    ProductName = "U"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 11,
                    ApoClass = 202,
                    BrandId = 3,
                    Code = "234021",
                    ProductName = "Pepsi"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 12,
                    ApoClass = 202,
                    BrandId = 1,
                    Code = "445021",
                    ProductName = "Coke"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 13,
                    ApoClass = 202,
                    BrandId = 1,
                    Code = "864021",
                    ProductName = "Fanta"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 14,
                    ApoClass = 203,
                    BrandId = 1,
                    Code = "231021",
                    ProductName = "Sprite"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 15,
                    ApoClass = 203,
                    BrandId = 1,
                    Code = "2340021",
                    ProductName = "Miranda"
                },
                new ProductServiceTest.ProductDomain
                {
                    Id = 16,
                    ApoClass = 203,
                    BrandId = 1,
                    Code = "963021",
                    ProductName = "Something"
                },
            };
        }
    }
}