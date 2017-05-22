using System;
using System.Web.Http;
using Tops.Test.Helper;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;

namespace TopsApiService.Controllers
{
    [RoutePrefix("api/beta/apodivision")]
    public class ApoDivisonController : ApiController
    {
        private IApoDivisionService _apoDivisionService;

        public ApoDivisonController()
        {
            var apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivisionService = new ApoDivisionService(apoDivisionRepository
                , new ApoGroupService(SetUpMockHelper.GetApoGroupRepository()));
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
            try
            {
                var createdApo = _apoDivisionService.Create(apoDivisionForCreateOrEdit as IApoDivisionForCreateOrEdit);

                if (createdApo != null)
                {
                    return Ok(createdApo);
                }

                return InternalServerError();
            }
            catch (ArgumentException e)
            {
                return InternalServerError(e);
            }
           
        }

        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult Update(int id,[FromBody]ApoDivisionForCreateOrEdit apoDivisionForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoDivisionService.Edit(id, apoDivisionForCreateOrEdit as IApoDivisionForCreateOrEdit);

                if (updateApo != null)
                {
                    return Ok(updateApo);
                }

                return InternalServerError();
            }
            catch (ArgumentException e)
            {
                return InternalServerError(e);

            }
           
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_apoDivisionService.Delete(id))
                {
                    return Ok();
                }
                return InternalServerError();

            }
            catch (InvalidOperationException e)
            {
                return InternalServerError(e);
            }
           

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

        [HttpGet]
        [Route("getall")]
        public IHttpActionResult GetAllApo()
        {
            return Ok(_apoDivisionService.GetAll());
        }
    }
}
