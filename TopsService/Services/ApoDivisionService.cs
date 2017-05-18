using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Models.Instance;
using TopsShareClass.Models.DataTranferObjects;
using TopsShareClass.Models.Domain;

namespace TopsService.Services
{
 

    public class ApoDivisionService : IApoDivisionService
    {
        private readonly IApoDivisionRepository _apoDivisionRepository;
        private readonly IApoGroupService _apoGroupService;


        public ApoDivisionService(IApoDivisionRepository apoDivisionRepository, IApoGroupService apoGroupService)
        {
            _apoDivisionRepository = apoDivisionRepository;
            _apoGroupService = apoGroupService;
        }

        public PagedList<IApoDivisionDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            var apoDivisionFromRepository =
                _apoDivisionRepository.GetAll(new ResourceParamater(page, pageSize, searchText));

            return PagedList<IApoDivisionDataTranferObject>.Create(Mapper.Map<List<ApoDivisionDto>>(apoDivisionFromRepository).AsQueryable(), page, pageSize);
        }

        public IApoDivisionDataTranferObject GetById(int id)
        {
            var apoDivisionFromRepository = _apoDivisionRepository.GetById(id);

            if (apoDivisionFromRepository == null)
            {
                return null;
            }

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);
        }

        public IApoDivisionDataTranferObject Create(IApoDivisionForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoDivisionDomain>(item);

            if (_apoDivisionRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoDivisionFromRepository = _apoDivisionRepository.Add(mapToDomain);

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);
        }

        public IApoDivisionDataTranferObject Edit(int id, IApoDivisionForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoDivisionDomain>(item);

            var selectedApoDivision = _apoDivisionRepository.GetByName(item);

            if (selectedApoDivision != null
                && selectedApoDivision.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && id != selectedApoDivision.Id)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoDivisionFromRepository = _apoDivisionRepository.Update(id, mapToDomain);

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);

        }

        public bool Delete(int id)
        {
            if (_apoDivisionRepository.GetById(id) == null)
                return false;

            var listOfChildHierachy = _apoGroupService.GetApoGroupByApoDivision(id);

            if (listOfChildHierachy.Any())
            {
                throw new InvalidOperationException($"Id :{id} has child hierachy.");
            }

            try
            {
                var status = _apoDivisionRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }

        }

        public IApoDivisionDataTranferObject GetByName(IApoDivisionForCreateOrEdit item)
        {

            var selectedApoDivision = _apoDivisionRepository.GetByName(item);

            if (selectedApoDivision == null)
            {
                return null;
            }

            return Mapper.Map<ApoDivisionDto>(selectedApoDivision);
        }

        public IEnumerable<IApoDivisionDataTranferObject> GetAll()
        {
            var selectedApoDivision = _apoDivisionRepository.GetAll();

            return Mapper.Map<IEnumerable<ApoDivisionDto>>(selectedApoDivision);
        }
    }

}
