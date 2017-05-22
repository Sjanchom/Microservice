using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Tops.Test.UnitTest;
using TopsShareClass.Models.Domain;

namespace Tops.Test.Helper
{
    public class DataInitializer
    {
        //var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //var archiveFolder = Path.Combine(currentDirectory, "Data");
        public static List<ProductServiceTest.ProductDomain> GetProductFromTextFile()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductDomain>>(ReadFile(CONSTAINTSCONFIG
                .PRODUCT));
        }

        private static string ReadFile(string fileName)
        {
            string text;
            FileStream file = null;

            foreach (var s in CONSTAINTSCONFIG.DIR_PATH)
            {
                try
                {
                    file = new FileStream(s + fileName, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                }
            }
             
            using (var streamReader = new StreamReader(file, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }


            return text;
        }


        public static List<ProductServiceTest.AttributeTypeDomain> GetAllTypeAttributeTypeDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeTypeDomain>>(ReadFile(CONSTAINTSCONFIG
                .ATTRIBUTE_TYPE));
        }

        public static List<ProductServiceTest.AttributeValueDomain> GetAttributeValueDomains()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.AttributeValueDomain>>(
                ReadFile(CONSTAINTSCONFIG.ATTRIBUTE_VALUE));
        }

        public static List<ProductServiceTest.ProductAttributeDetail> GetaProductAttributeHeaders()
        {
            return JsonConvert.DeserializeObject<List<ProductServiceTest.ProductAttributeDetail>>(
                ReadFile(CONSTAINTSCONFIG.PRODUCT_DETAIL));
        }

        public static List<ApoDivisionDomain> GetApoDivisions()
        {
            return JsonConvert.DeserializeObject<List<ApoDivisionDomain>>(ReadFile(CONSTAINTSCONFIG.APO_DIVISION));
        }

        public static List<ApoGroupDomain> GetApoGroup()
        {
            return JsonConvert.DeserializeObject<List<ApoGroupDomain>>(ReadFile(CONSTAINTSCONFIG.APO_GROUP));
        }

        public static List<ApoDepartmentDomain> GetApoDepartment()
        {
            return JsonConvert.DeserializeObject<List<ApoDepartmentDomain>>(ReadFile(CONSTAINTSCONFIG.APO_DEPT));
        }

        public static List<ApoClassDomain> GetApoClass()
        {
            return JsonConvert.DeserializeObject<List<ApoClassDomain>>(ReadFile(CONSTAINTSCONFIG.APO_CLASS));
        }

        public static List<ApoDepartmentDomain> GetApoSubClass()
        {
            return JsonConvert.DeserializeObject<List<ApoDepartmentDomain>>(ReadFile(CONSTAINTSCONFIG.APO_SUBCLASS));
        }
    }
}