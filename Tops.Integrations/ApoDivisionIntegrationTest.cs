using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TopsDataAccessLayer.Persistence.Contexts;

namespace Tops.Integrations
{
    [TestClass]
    public class ApoDivisionIntegrationTest
    {
        private readonly TopsContext db;

        public ApoDivisionIntegrationTest()
        {
            db = new TopsContext();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var apoList = db.ApoDivision.ToList();
        }
    }
}
