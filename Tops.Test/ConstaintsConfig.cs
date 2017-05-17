using System;
using System.Collections.Generic;

namespace Tops.Test
{
    internal class CONSTAINTSCONFIG
    {
        //public const string DIR_PATH = @"c:\Tops\";
        //public const string DIR_PATH = @"C:\inetpub\wwwroot\topsservice\TopsJson\";
        public static  List<string> DIR_PATH = new List<string> { @"\\Mac\Home\Desktop\TopsJson\", @"C:\inetpub\wwwroot\topsservice\TopsJson\", @"c:\Tops\" } ;


        public const string APO_CLASS = "ApoClass";
        public const string APO_SUBCLASS = "ApoSubClass";
        public const string APO_DEPT = "ApoDept";
        public const string APO_GROUP = "ApoGroup";
        public const string APO_DIVISION = "ApoDivision";


        public const string PRODUCT = "Product";
        public const string PRODUCT_DETAIL = "ProductDetail";


        public const string ATTRIBUTE_TYPE = "AttributeType";
        public const string ATTRIBUTE_VALUE = "AttributeValue";


        public const string PRODUCT_ATTR = "ApoClassMapAttribute";


        public const string STORE = "Store";
        public const string STORE_DETAIL = "storeDetailList";
        public const string STORE_GROUP = "StoreGroup";
        public const string STORE_TYPE = "StoreType";
        public const string PRODUCT_VALUE = "StoreValue";


        public const string MARKET_DATA_CATERGORY = "MarketCatergory";
        public const string MARKET_DATA_SUBCATERGORY = "MarketSubCatergory";
        public const string MARKET_DATA_DETAIL = "MarketDetail";
    }
}
