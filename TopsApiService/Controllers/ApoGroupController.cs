﻿using System;
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
    [RoutePrefix("api/beta/apogroup")]
    public class ApoGroupController : ApiController
    {
        private IApoGroupService _apoGroupService;

        public ApoGroupController()
        {
            _apoGroupService = new ApoGroupService(SetUpMockHelper.GetApoGroupRepository(), SetUpMockHelper.GetApoDivisionRepository());
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get(int page = 1, int pageSize = 15, int? apoDivisionId = null, string searchText = "")
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                _apoGroupService.GetAll(new ApoGroupResourceParameter(page, pageSize, apoDivisionId, searchText)));
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            var selectedApo = _apoGroupService.GetById(id);

            if (selectedApo == null)
            {
                HttpError err = new HttpError($"ID : {id} Not Exist.");
                return Request.CreateResponse(HttpStatusCode.NotFound, err);

            }

            return Request.CreateResponse(HttpStatusCode.OK, selectedApo);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody]ApoGroupForCreateOrUpdate apoGroupForCreateOrEdit)
        {
            try
            {
                var createdApo = _apoGroupService.Create(apoGroupForCreateOrEdit);
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
        public HttpResponseMessage Update(int id, [FromBody]ApoGroupForCreateOrUpdate apoGroupForCreateOrEdit)
        {

            try
            {
                var updateApo = _apoGroupService.Edit(id, apoGroupForCreateOrEdit as IApoGroupForCreateOrEdit);

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
                if (_apoGroupService.Delete(id))
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
            var selectedApo = _apoGroupService.GetByName(new ApoGroupForCreateOrUpdate()
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
            return Ok(_apoGroupService.GetAll());
        }

    }
}
