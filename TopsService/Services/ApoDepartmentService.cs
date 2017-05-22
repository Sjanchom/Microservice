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
    public class ApoDepartmentService : IApoDepartmentService
    {
        private readonly IApoDepartmentRepository _apoDepartmentRepository;
        private readonly IApoGroupRepository _apoGroupRepository;
        private readonly IApoDivisionRepository _apoDivisionRepository;

        public ApoDepartmentService(IApoDivisionRepository apoDivisionRepository
            , IApoGroupRepository apoGroupRepository, IApoDepartmentRepository apoDepartmentRepository)
        {
            _apoDivisionRepository = apoDivisionRepository;
            _apoGroupRepository = apoGroupRepository;
            _apoDepartmentRepository = apoDepartmentRepository;
        }

        public PagedList<IApoDepartmentDataTranferObject> GetAll(IApoDepartmentResourceParameter apoDepartmentResourceParameter)
        {
            var selectedApo = _apoDepartmentRepository.GetAll(apoDepartmentResourceParameter);

            var apoMapToDepartment = Mapper.Map<List<ApoDepartmentDto>>(selectedApo);

            MapDivisionAndGroupToDto(apoMapToDepartment);

            return PagedList<IApoDepartmentDataTranferObject>.Create(apoMapToDepartment.AsQueryable(),
                apoDepartmentResourceParameter.Page, apoDepartmentResourceParameter.PageSize);
        }

        public IEnumerable<IApoDepartmentDataTranferObject> GetAll()
        {
            var selectedApo = _apoDepartmentRepository.GetAll();

            var mapToObj = Mapper.Map<List<ApoDepartmentDto>>(selectedApo);

            MapDivisionAndGroupToDto(mapToObj);

            return mapToObj;
        }

        public IApoDepartmentDataTranferObject GetById(int id)
        {
            var apoDepartmentFromRepository = _apoDepartmentRepository.GetById(id);

            if (apoDepartmentFromRepository == null)
            {
                return null;
            }

            var mapToObj = Mapper.Map<ApoDepartmentDto>(apoDepartmentFromRepository);


            MapDivisionAndGroupToDto(mapToObj);


            return mapToObj;
        }

        public IApoDepartmentDataTranferObject Create(IApoDepartmentForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoDepartmentDomain>(item);

            if (_apoDepartmentRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoDepathmentFromRepository = _apoDepartmentRepository.Add(mapToDomain);

            var maptoDto = Mapper.Map<ApoDepartmentDto>(apoDepathmentFromRepository);

            MapDivisionAndGroupToDto(maptoDto);

            return maptoDto;
        }

        public IApoDepartmentDataTranferObject Edit(int id, IApoDepartmentForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoDepartmentDomain>(item);

            var selectedApodepartment = _apoDepartmentRepository.GetByName(item);

            if (selectedApodepartment != null
                && selectedApodepartment.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && selectedApodepartment.DivisionId == item.ApoDivisionId
                && selectedApodepartment.GroupId == item.ApoGroupId)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoDepartmentFromRepository = _apoDepartmentRepository.Update(id, mapToDomain);

            var mapDomainToDto = Mapper.Map<ApoDepartmentDto>(apoDepartmentFromRepository);


            MapDivisionAndGroupToDto(mapDomainToDto);

            return mapDomainToDto;
        }

        public bool Delete(int id)
        {

            if (_apoDepartmentRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoDepartmentRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }
        }

        public IApoDepartmentDataTranferObject GetByName(IApoDepartmentForCreateOrEdit item)
        {
            var apoDepartmentDomain = _apoDepartmentRepository.GetByName(item);

            if (apoDepartmentDomain == null)
            {
                return null;
            }

            var mapToDto = Mapper.Map<ApoDepartmentDto>(apoDepartmentDomain);
            MapDivisionAndGroupToDto(mapToDto);

            return mapToDto;
        }



        public PagedList<IApoDepartmentDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IApoDepartmentDataTranferObject> GetApoDepartmentByApoGroup(int id)
        {
            var apoDepartmentDomains = _apoDepartmentRepository.GetByApoGroup(id);

            var mapDomainToDto = Mapper.Map<List<ApoDepartmentDto>>(apoDepartmentDomains);

            MapDivisionAndGroupToDto(mapDomainToDto);

            return mapDomainToDto;

        }

        private void MapDivisionAndGroupToDto(List<ApoDepartmentDto> apoDepartmentDtos)
        {
            foreach (var apoDepartmentDto in apoDepartmentDtos)
            {
                MapDivisionToDto(apoDepartmentDto);
                MapGroupToDto(apoDepartmentDto);
            }
        }

        private void MapDivisionAndGroupToDto(ApoDepartmentDto apoDepartmentDto)
        {
            MapDivisionToDto(apoDepartmentDto);
            MapGroupToDto(apoDepartmentDto);
        }

        private void MapDivisionToDto(ApoDepartmentDto apoDepartmentDto)
        {
            try
            {
                apoDepartmentDto.DivisionName = ApoDivisionInstances.GetInstance(_apoDivisionRepository).GetApoDivisionDomains.Single(x => x.Id == apoDepartmentDto.DivisionId).Name;
            }
            catch (Exception e)
            {
                apoDepartmentDto.DivisionName = "-.-";
            }
        }


        private void MapGroupToDto(ApoDepartmentDto apoDepartmentDto)
        {
            apoDepartmentDto.GroupName = ApoGroupInstances.GetInstance(_apoGroupRepository).GetApoGroupDomains.Single(x => x.Id == apoDepartmentDto.GroupId).Name;
        }


    }

}
