using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Tops.Test.UnitTest;
using TopsService.Models.Domain;

namespace Tops.Test.Helper
{
    public class DataInitializer
    {
        //var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //var archiveFolder = Path.Combine(currentDirectory, "Data");
        public static List<ProductServiceTest.ProductDomain> GetProductFromTextFile()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductDomain>>(ReadFile(CONSTAINTSCONFIG.PRODUCT));
        }

        private static string ReadFile(string fileName)
        {
            string text;
            var fileStream = new FileStream(CONSTAINTSCONFIG.DIR_PATH + fileName, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            return text;
        }


        public static List<ProductServiceTest.AttributeTypeDomain> GetAllTypeAttributeTypeDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeTypeDomain>>(ReadFile(CONSTAINTSCONFIG.ATTRIBUTE_TYPE));
        }

        public static List<ProductServiceTest.AttributeValueDomain> GetAttributeValueDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeValueDomain>>(ReadFile(CONSTAINTSCONFIG.ATTRIBUTE_VALUE));
        }

        public static List<ProductServiceTest.ProductAttributeDetail> GetaProductAttributeHeaders()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductAttributeDetail>>(ReadFile(CONSTAINTSCONFIG.PRODUCT_DETAIL));
        }

        public static List<ApoDivisionDomain> GetApoDivisions()
        {
            return JsonConvert.DeserializeObject<List<ApoDivisionDomain>>(ReadFile(CONSTAINTSCONFIG.APO_DIVISION));
        }

        public static List<ApoGroupDomain> GetApoGroup()
        {
            return JsonConvert.DeserializeObject<List<ApoGroupDomain>>(ReadFile(CONSTAINTSCONFIG.APO_GROUP));
        }
    }
}