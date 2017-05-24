using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tops.Test.Helper;
using TopsInterface.Core;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;

namespace TopsApiService.Controllers
{
    public class ApoSubClassController : ApiController
    {
        private IApoSubClassService _apoSubClassService;

        public ApoSubClassController()
        {
            _apoSubClassService = new ApoSubClassService(SetUpMockHelper.GetApoClassRepository(), SetUpMockHelper.GetApoSubClassRepository());
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int page = 1, int pageSize = 15, int? apoClassId = null, string searchText = "")
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _apoSubClassService.GetAll(new ApoSubClassResourceParameter(page,
                    pageSize, apoClassId, searchText)));
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var selectedApo = _apoSubClassService.GetById(id);

            if (selectedApo == null)
            {
                HttpError err = new HttpError($"ID : {id} Not Exist.");
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            }

            return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ApoSubClassForCreateOrEdit apoClassForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoSubClassService.Create(apoClassForCreateOrEdit);

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
        public HttpResponseMessage Update(int id, [FromBody]ApoSubClassForCreateOrEdit apoClassForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoSubClassService.Edit(id, apoClassForCreateOrEdit);

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
                if (_apoSubClassService.Delete(id))
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
            var selectedApo = _apoSubClassService.GetByName(new ApoSubClassForCreateOrEdit()
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
            return Ok(_apoSubClassService.GetAll());
        }

    }
}
