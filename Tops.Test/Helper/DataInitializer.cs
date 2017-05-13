using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Tops.Test.UnitTest;

namespace Tops.Test.Helper
{
    public class DataInitializer
    {
        //var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //var archiveFolder = Path.Combine(currentDirectory, "Data");
        public static List<ProductServiceTest.ProductDomain> GetProductFromTextFile()
        {
            string text;
            var fileStream = new FileStream(@"c:\Tops\"+ConstaintsConfig.PRODUCT, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            var test = JsonConvert.DeserializeObject<List<ProductServiceTest.ProductDomain>>(text);
            return null;
        }


        //public static List<ProductServiceTest.ProductDomain> GetAllProductDomain()
        //{
        //    return new List<ProductServiceTest.ProductDomain>()
        //    {
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 1,
        //            ApoClassCode = 200,
        //            BrandId = 1,
        //            ProductCode = "100021",
        //            ProductName = "Cookies"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 2,
        //            ApoClassCode = 200,
        //            BrandId = 2,
        //            ProductCode = "104021",
        //            ProductName = "Biscuit"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 3,
        //            ApoClassCode = 200,
        //            BrandId = 3,
        //            ProductCode = "400021",
        //            ProductName = "Be"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 4,
        //            ApoClassCode = 200,
        //            BrandId = 1,
        //            ProductCode = "234021",
        //            ProductName = "FunO"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 5,
        //            ApoClassCode = 200,
        //            BrandId = 2,
        //            ProductCode = "220021",
        //            ProductName = "Oreo"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 6,
        //            ApoClassCode = 201,
        //            BrandId = 3,
        //            ProductCode = "4520021",
        //            ProductName = "Sigha"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 7,
        //            ApoClassCode = 201,
        //            BrandId = 1,
        //            ProductCode = "1120021",
        //            ProductName = "Leo"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 8,
        //            ApoClassCode = 201,
        //            BrandId = 2,
        //            ProductCode = "455021",
        //            ProductName = "Tiger"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 9,
        //            ApoClassCode = 201,
        //            BrandId = 2,
        //            ProductCode = "235021",
        //            ProductName = "Chang"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 10,
        //            ApoClassCode = 202,
        //            BrandId = 1,
        //            ProductCode = "872021",
        //            ProductName = "U"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 11,
        //            ApoClassCode = 202,
        //            BrandId = 3,
        //            ProductCode = "234021",
        //            ProductName = "Pepsi"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 12,
        //            ApoClassCode = 202,
        //            BrandId = 1,
        //            ProductCode = "445021",
        //            ProductName = "Coke"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 13,
        //            ApoClassCode = 202,
        //            BrandId = 1,
        //            ProductCode = "864021",
        //            ProductName = "Fanta"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 14,
        //            ApoClassCode = 203,
        //            BrandId = 1,
        //            ProductCode = "231021",
        //            ProductName = "Sprite"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 15,
        //            ApoClassCode = 203,
        //            BrandId = 1,
        //            ProductCode = "2340021",
        //            ProductName = "Miranda"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 16,
        //            ApoClassCode = 203,
        //            BrandId = 1,
        //            ProductCode = "963021",
        //            ProductName = "Something"
        //        },
        //    };
        //}

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