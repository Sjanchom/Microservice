using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Models.DataTranferObjects;
using TopsService.Models.Domain;

namespace TopsService.Services
{
    public class ApoDivisionService : IApoBaseService<IApoDivisionDataTranferObject, IApoDivisionForCreateOrEdit>
    {
        private IApoDivisionRepository _apoDivisionRepository;


        public ApoDivisionService(IApoDivisionRepository apoDivisionRepository)
        {
            _apoDivisionRepository = apoDivisionRepository;
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
                return null;
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
                return null;
            }

            var apoDivisionFromRepository = _apoDivisionRepository.Update(id, mapToDomain);

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);

        }

        public bool Delete(int id)
        {
            if (_apoDivisionRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoDivisionRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                return false;
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
    }

}
