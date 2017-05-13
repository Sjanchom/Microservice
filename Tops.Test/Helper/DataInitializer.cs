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
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductDomain>>(ReadFile(ConstaintsConfig.PRODUCT));
        }

        private static string ReadFile(string fileName)
        {
            string text;
            var fileStream = new FileStream(@"c:\Tops\" + fileName, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            return text;
        }


        //public static List<ProductServiceTest.ProductDomain> GetAllProductDomain()
        //{
        //    return new List<ProductServiceTest.ProductDomain>()
        //    {
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 1,
        //            ApoClass = 200,
        //            BrandId = 1,
        //            ProductCode = "100021",
        //            ProductName = "Cookies"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 2,
        //            ApoClass = 200,
        //            BrandId = 2,
        //            ProductCode = "104021",
        //            ProductName = "Biscuit"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 3,
        //            ApoClass = 200,
        //            BrandId = 3,
        //            ProductCode = "400021",
        //            ProductName = "Be"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 4,
        //            ApoClass = 200,
        //            BrandId = 1,
        //            ProductCode = "234021",
        //            ProductName = "FunO"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 5,
        //            ApoClass = 200,
        //            BrandId = 2,
        //            ProductCode = "220021",
        //            ProductName = "Oreo"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 6,
        //            ApoClass = 201,
        //            BrandId = 3,
        //            ProductCode = "4520021",
        //            ProductName = "Sigha"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 7,
        //            ApoClass = 201,
        //            BrandId = 1,
        //            ProductCode = "1120021",
        //            ProductName = "Leo"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 8,
        //            ApoClass = 201,
        //            BrandId = 2,
        //            ProductCode = "455021",
        //            ProductName = "Tiger"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 9,
        //            ApoClass = 201,
        //            BrandId = 2,
        //            ProductCode = "235021",
        //            ProductName = "Chang"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 10,
        //            ApoClass = 202,
        //            BrandId = 1,
        //            ProductCode = "872021",
        //            ProductName = "U"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 11,
        //            ApoClass = 202,
        //            BrandId = 3,
        //            ProductCode = "234021",
        //            ProductName = "Pepsi"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 12,
        //            ApoClass = 202,
        //            BrandId = 1,
        //            ProductCode = "445021",
        //            ProductName = "Coke"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 13,
        //            ApoClass = 202,
        //            BrandId = 1,
        //            ProductCode = "864021",
        //            ProductName = "Fanta"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 14,
        //            ApoClass = 203,
        //            BrandId = 1,
        //            ProductCode = "231021",
        //            ProductName = "Sprite"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 15,
        //            ApoClass = 203,
        //            BrandId = 1,
        //            ProductCode = "2340021",
        //            ProductName = "Miranda"
        //        },
        //        new ProductServiceTest.ProductDomain
        //        {
        //            Id = 16,
        //            ApoClass = 203,
        //            BrandId = 1,
        //            ProductCode = "963021",
        //            ProductName = "Something"
        //        },
        //    };
        //}

        public static IEnumerable<ProductServiceTest.AttributeTypeDomain> GetAllTypeAttributeTypeDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeTypeDomain>>(ReadFile(ConstaintsConfig.ATTRIBUTE_TYPE));
        }

        public static IEnumerable<ProductServiceTest.AttributeValueDomain> GetAttributeValueDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeValueDomain>>(ReadFile(ConstaintsConfig.ATTRIBUTE_VALUE));
        }

        public static IEnumerable<ProductServiceTest.ProductAttributeDetail> GetaProductAttributeHeaders()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductAttributeDetail>>(ReadFile(ConstaintsConfig.PRODUCT_DETAIL));
        }
    }
}