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


        public static List<ProductServiceTest.AttributeTypeDomain> GetAllTypeAttributeTypeDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeTypeDomain>>(ReadFile(ConstaintsConfig.ATTRIBUTE_TYPE));
        }

        public static List<ProductServiceTest.AttributeValueDomain> GetAttributeValueDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeValueDomain>>(ReadFile(ConstaintsConfig.ATTRIBUTE_VALUE));
        }

        public static List<ProductServiceTest.ProductAttributeDetail> GetaProductAttributeHeaders()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductAttributeDetail>>(ReadFile(ConstaintsConfig.PRODUCT_DETAIL));
        }
    }
}