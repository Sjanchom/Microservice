using System.Linq;
using TopsDataAccessLayer.Persistence.Contexts;
using System.Data.Entity;

namespace Repository.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TopsContext();

            var apoList = db.ApoDivision.Include(x => x.ApoGroups).ToList();
        }
    }
}
