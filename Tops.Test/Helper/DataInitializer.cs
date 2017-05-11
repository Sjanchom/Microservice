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

        public static IEnumerable<ProductServiceTest.AttributeTypeDomain> GetAllTypeAttributeTypeDomains()
        {
            return new List<ProductServiceTest.AttributeTypeDomain>()
            {
                new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 1,
                    Name = "Pack Type",
                    Code = "001"
                },
                new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 2,
                    Name = "Segment A",
                    Code = "001"
                }
                ,
                new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 3,
                    Name = "Segment B",
                    Code = "003"
                },
                new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 4,
                    Name = "Local",
                    Code = "004"
                },
                new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 5,
                    Name = "Special",
                    Code = "005"
                }
                , new ProductServiceTest.AttributeTypeDomain()
                {
                    Id = 6,
                    Name = "Shelf",
                    Code = "006"
                }
            };
        }

        public static IEnumerable<ProductServiceTest.AttributeValueDomain> GetAttributeValueDomains()
        {
            return new List<ProductServiceTest.AttributeValueDomain>()
            {
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 1,
                    TypeId = 1,
                    Code = "001",
                    Name = "Large"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 2,
                    TypeId = 1,
                    Code = "002",
                    Name = "p[lfgp[ljf"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 3,
                    TypeId = 1,
                    Code = "003",
                    Name = ",dsglkdfhk"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 4,
                    TypeId = 2,
                    Code = "001",
                    Name = "hghjfgj"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 5,
                    TypeId = 2,
                    Code = "002",
                    Name = "jghkgk"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 6,
                    TypeId = 2,
                    Code = "003",
                    Name = "Largggggggggggge"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 7,
                    TypeId = 3,
                    Code = "001",
                    Name = "Lashdfhhjjrge"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 8,
                    TypeId = 3,
                    Code = "002",
                    Name = "gdsfhdh"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 9,
                    TypeId = 3,
                    Code = "003",
                    Name = "Larasdasfge"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 10,
                    TypeId = 4,
                    Code = "001",
                    Name = "fdsfsdgg"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 11,
                    TypeId = 4,
                    Code = "002",
                    Name = "Largdfgge"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 12,
                    TypeId = 4,
                    Code = "003",
                    Name = "Larsdfsdfge"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 13,
                    TypeId = 5,
                    Code = "001",
                    Name = "sgddfsgdfg"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 14,
                    TypeId = 5,
                    Code = "002",
                    Name = "gdfh"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 15,
                    TypeId = 5,
                    Code = "003",
                    Name = "sdffsdf"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 16,
                    TypeId = 6,
                    Code = "001",
                    Name = "dsf"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 17,
                    TypeId = 6,
                    Code = "002",
                    Name = "sdf"
                },
                new ProductServiceTest.AttributeValueDomain()
                {
                    Id = 18,
                    TypeId = 6,
                    Code = "003",
                    Name = "gt"
                },

            };
        }

        public static IEnumerable<ProductServiceTest.ProductAttributeHeader> GetaProductAttributeHeaders()
        {
            return new List<ProductServiceTest.ProductAttributeHeader>()
            {
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 1,
                    ApoClass = 200,
                    TypeId = 1,
                    ValueId = 1,
                    ProductId = 1

                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 2,
                    ApoClass = 200,
                    TypeId = 1,
                    ValueId = 2,
                    ProductId = 1
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 3,
                    ApoClass = 200,
                    TypeId = 1,
                    ValueId = 3,
                    ProductId = 1
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 4,
                    ApoClass = 201,
                    TypeId = 1,
                    ValueId = 3,
                    ProductId = 5
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 5,
                    ApoClass = 201,
                    TypeId = 2,
                    ValueId = 2,
                    ProductId = 5
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 6,
                    ApoClass = 201,
                    TypeId = 3,
                    ValueId = 3,
                    ProductId = 5
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 7,
                    ApoClass = 201,
                    TypeId = 6,
                    ValueId = 1,
                    ProductId = 5
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 8,
                    ApoClass = 200,
                    TypeId = 1,
                    ValueId = 1,
                    ProductId = 3
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 9,
                    ApoClass = 200,
                    TypeId = 1,
                    ValueId = 3,
                    ProductId = 3
                },
                new ProductServiceTest.ProductAttributeHeader()
                {
                    Id = 10,
                    ApoClass = 201,
                    TypeId = 4,
                    ValueId = 1
                },

            };
        }
    }
}