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
    [RoutePrefix("api/beta/apoclass")]
    public class ApoClassController : ApiController
    {
        private IApoClassService _apoClassService;

        public ApoClassController()
        {
            _apoClassService = new ApoClassService(SetUpMockHelper.GetApoClassRepository(),SetUpMockHelper.GetApoDepartmentRepository());
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int page = 1, int pageSize = 15, int? apoDepartmentId = null, string searchText = "")
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _apoClassService.GetAll(new ApoClassResourceParameter(page, 
                pageSize, apoDepartmentId, searchText)));
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var selectedApo = _apoClassService.GetById(id);

            if (selectedApo == null)
            {
                HttpError err = new HttpError($"ID : {id} Not Exist.");
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
              
            }

            return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ApoClassForCreateOrEdit apoClassForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoClassService.Create(apoClassForCreateOrEdit);

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
        public HttpResponseMessage Update(int id, [FromBody]ApoClassForCreateOrEdit apoClassForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoClassService.Edit(id, apoClassForCreateOrEdit);

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
                if (_apoClassService.Delete(id))
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
            var selectedApo = _apoClassService.GetByName(new ApoClassForCreateOrEdit()
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
            return Ok(_apoClassService.GetAll());
        }

    }
}
