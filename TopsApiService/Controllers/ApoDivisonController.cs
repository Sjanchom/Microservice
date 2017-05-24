using System;
using System.Net;
using System.Net.Http;
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
                , new ApoGroupService(SetUpMockHelper.GetApoGroupRepository(), SetUpMockHelper.GetApoDivisionRepository()));
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int page =1,int pageSize = 15,string searchText = "")
        {
            return Request.CreateResponse(HttpStatusCode.OK, _apoDivisionService.GetAll(page, pageSize, searchText));
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var selectedApo = _apoDivisionService.GetById(id);
            if (selectedApo == null)
            {
                HttpError err = new HttpError($"ID : {id} Not Exist.");
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            }

            return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ApoDivisionForCreateOrEdit apoDivisionForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoDivisionService.Create(apoDivisionForCreateOrEdit);

                if (createdApo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, createdApo);
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (ArgumentException e)
            {
                HttpError err = new HttpError(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }

        }

        [HttpPost]
        [Route("{id}")]
        public HttpResponseMessage Update(int id,[FromBody]ApoDivisionForCreateOrEdit apoDivisionForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoDivisionService.Edit(id, apoDivisionForCreateOrEdit as IApoDivisionForCreateOrEdit);

                if (updateApo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, updateApo);
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (ArgumentException e)
            {
                HttpError err = new HttpError(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);

            }

        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (_apoDivisionService.Delete(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
            catch (InvalidOperationException e)
            {
                HttpError err = new HttpError(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }


        }

        [HttpGet]
        [Route("getbyname/{name}")]
        public HttpResponseMessage GetByName(string name)
        {
           
            var selectedApo = _apoDivisionService.GetByName(new ApoDivisionForCreateOrEdit()
            {
                Name = name
            });

            if (selectedApo != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
            }

            HttpError err = new HttpError($"{name} : Not Exists");
            return Request.CreateResponse(HttpStatusCode.NotFound, err);
        }

        [HttpGet]
        [Route("getall")]
        public IHttpActionResult GetAllApo()
        {
            return Ok(_apoDivisionService.GetAll());
        }
    }
}
