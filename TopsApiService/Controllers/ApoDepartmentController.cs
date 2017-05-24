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
    [RoutePrefix("api/beta/apodepartment")]
    public class ApoDepartmentController : ApiController
    {
        private IApoDepartmentService _apoDepartmentService;

        public ApoDepartmentController()
        {
            _apoDepartmentService = new ApoDepartmentService(SetUpMockHelper.GetApoDivisionRepository(),
                SetUpMockHelper.GetApoGroupRepository(),SetUpMockHelper.GetApoDepartmentRepository());
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int page = 1, int pageSize = 15, int? apoDivisionId = null,int? apoGroupId = null, string searchText = "")
        {
            return Request.CreateResponse(HttpStatusCode.OK, _apoDepartmentService.GetAll(new ApoDepartmentResourceParameter(page, pageSize, apoDivisionId, apoGroupId, searchText)));
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var selectedApo = _apoDepartmentService.GetById(id);

            if (selectedApo == null)
            {
                HttpError err = new HttpError($"ID : {id} Not Exist.");
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ApoDepartmentCreateOrEdit apoDepartmentForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoDepartmentService.Create(apoDepartmentForCreateOrEdit);

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
        public HttpResponseMessage Update(int id, [FromBody]ApoDepartmentCreateOrEdit apoDepartmentForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoDepartmentService.Edit(id, apoDepartmentForCreateOrEdit);

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
                if (_apoDepartmentService.Delete(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            }
            catch (InvalidOperationException e)
            {
                HttpError err = new HttpError(e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,err);
            }


        }

        [HttpGet]
        [Route("getbyname/{name}")]
        public HttpResponseMessage GetByName(string name)
        {
            var selectedApo = _apoDepartmentService.GetByName(new ApoDepartmentCreateOrEdit()
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
            return Ok(_apoDepartmentService.GetAll());
        }
    }
}
