using System;
using System.Web.Http;
using Tops.Test.Helper;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;

namespace TopsApiService.Controllers
{
    [RoutePrefix("api/beta/apogroup")]
    public class ApoGroupController : ApiController
    {
        private IApoGroupService _apoGroupService;

        public ApoGroupController()
        {
            _apoGroupService = new ApoGroupService(SetUpMockHelper.GetApoGroupRepository());
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(int page = 1, int pageSize = 15, int? apoDivisionId = null, string searchText = "")
        {
            return Ok(_apoGroupService.GetAll(new ApoGroupResourceParameter(page,pageSize, apoDivisionId, searchText)));
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var selectedApo = _apoGroupService.GetById(id);

            if (selectedApo == null)
            {
                return NotFound();
            }


            return Ok(selectedApo);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]ApoGroupForCreateOrUpdate apoGroupForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoGroupService.Create(apoGroupForCreateOrEdit as IApoGroupForCreateOrEdit);

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
        public IHttpActionResult Update(int id, [FromBody]ApoGroupForCreateOrUpdate apoGroupForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoGroupService.Edit(id, apoGroupForCreateOrEdit as IApoGroupForCreateOrEdit);

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
                if (_apoGroupService.Delete(id))
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
            var selectedApo = _apoGroupService.GetByName(new ApoGroupForCreateOrUpdate()
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
            return Ok(_apoGroupService.GetAll());
        }

    }
}
