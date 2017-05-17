using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tops.Test.Helper;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;

namespace TopsApiService.Controllers
{
    [RoutePrefix("api/beta/apodivision")]
    public class ApoDivisonController : ApiController
    {
        private IApoBaseService<IApoDivisionDataTranferObject, IApoDivisionForCreateOrEdit> _apoDivisionService;

        public ApoDivisonController()
        {
            _apoDivisionService = new ApoDivisionService(SetUpMockHelper.GetApoDivisionRepository(),SetUpMockHelper.GetApoGroupService());
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(int page =1,int pageSize = 15,string searchText = "")
        {
            return Ok(_apoDivisionService.GetAll(page, pageSize, searchText));
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var selectedApo = _apoDivisionService.GetById(id);

            if (selectedApo == null)
            {
                return NotFound();
            }


            return Ok(selectedApo);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]ApoDivisionForCreateOrEdit apoDivisionForCreateOrEdit)
        {
            var createdApo = _apoDivisionService.Create(apoDivisionForCreateOrEdit as IApoDivisionForCreateOrEdit);

            if (createdApo != null)
            {
                return Ok(createdApo);
            }

            return InternalServerError();
        }

        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Update(int id,[FromBody]ApoDivisionForCreateOrEdit apoDivisionForCreateOrEdit)
        {
            var updateApo = _apoDivisionService.Edit(id, apoDivisionForCreateOrEdit as IApoDivisionForCreateOrEdit);


            if (updateApo != null)
            {
                return Ok(updateApo);
            }

            return InternalServerError();

        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (_apoDivisionService.Delete(id))
            {
                return Ok();
            }

            return InternalServerError();
        }

        [HttpGet]
        [Route("getbyname/{name}")]
        public IHttpActionResult GetByName(string name)
        {
            var selectedApo = _apoDivisionService.GetByName(new ApoDivisionForCreateOrEdit()
            {
                Name = name
            });

            if (selectedApo != null)
            {
                return Ok(selectedApo);
            }

            //return Request.CreateResponse(HttpStatusCode.NotFound, name);
            return NotFound();
        }
    }
}
