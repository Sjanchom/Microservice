using System.Web.Http;
using Tops.Test.Helper;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Services;

namespace TopsApiService.Controllers
{
    [Route("api/beta/apodivision")]
    public class ApoDivisonController : ApiController
    {
        private IApoBaseService<IApoDivisionDataTranferObject, IApoDivisionForCreateOrEdit> _apoDivisionService;

        public ApoDivisonController()
        {
            _apoDivisionService = new ApoDivisionService(SetUpMockHelper.GetApoDivisionRepository());
        }

        public IHttpActionResult Get(int page =1,int pageSize = 15,string searchText = "")
        {
            return Ok(_apoDivisionService.GetAll(page, pageSize, searchText));
        }
    }
}
