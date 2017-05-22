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
    public class ApoClassService : IApoClassService
    {
        private readonly IApoClassRepository _apoClassRepository;
        private readonly IApoDepartmentRepository _apoDepartmentRepository;

        public ApoClassService(IApoClassRepository apoClassRepository, IApoDepartmentRepository apoDepartmentRepository)
        {
            _apoClassRepository = apoClassRepository;
            _apoDepartmentRepository = apoDepartmentRepository;
        }

        public PagedList<IApoClassDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IApoClassDataTranferObject> GetAll()
        {
            var selectedApoDivision = _apoClassRepository.GetAll();

            var mapToDto = Mapper.Map<IEnumerable<ApoClassDto>>(selectedApoDivision).ToList();

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoClassDataTranferObject GetById(int id)
        {
            var apoClassFromRepository = _apoClassRepository.GetById(id);

            if (apoClassFromRepository == null)
            {
                return null;
            }

            var MapToDomain = Mapper.Map<ApoClassDto>(apoClassFromRepository);

            MapDeptToDto(MapToDomain);

            return MapToDomain;
        }

        public IApoClassDataTranferObject Create(IApoClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoClassDomain>(item);


            if (_apoClassRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoGroupFromRepository = _apoClassRepository.Add(mapToDomain);

            var mapToDto = Mapper.Map<ApoClassDto>(apoGroupFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoClassDataTranferObject Edit(int id, IApoClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoClassDomain>(item);

            var selectedApoClass = _apoClassRepository.GetByName(item);

            if (selectedApoClass != null
                && selectedApoClass.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && id != selectedApoClass.Id)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoClassFromRepository = _apoClassRepository.Update(id, mapToDomain);

            var mapToDto = Mapper.Map<ApoClassDto>(apoClassFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public bool Delete(int id)
        {
            if (_apoClassRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoClassRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }
        }

        public IApoClassDataTranferObject GetByName(IApoClassForCreateOrEdit item)
        {
            var selectedApoClass = _apoClassRepository.GetByName(item);

            if (selectedApoClass == null)
            {
                return null;
            }

            var mapToDto = Mapper.Map<ApoClassDto>(selectedApoClass);

            MapDeptToDto(mapToDto);


            return mapToDto;
        }

        public PagedList<IApoClassDataTranferObject> GetAll(IApoClassResourceParameter apoClassResourceParameter)
        {
            var apoClassFromRepository = _apoClassRepository.GetAll(apoClassResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoClassDto>>(apoClassFromRepository);

            MapDeptToDto(mapDomainToDto);

            return PagedList<IApoClassDataTranferObject>.Create(mapDomainToDto.AsQueryable(), apoClassResourceParameter.Page,
                apoClassResourceParameter.PageSize);
        }

        public IEnumerable<IApoClassDataTranferObject> GetApoClassByApoDepartment(int id)
        {
            var apoGroupFromRepository = _apoClassRepository.GetByApoDepartment(id);

            var mapDomainToDto = Mapper.Map<List<ApoClassDto>>(apoGroupFromRepository);

            MapDeptToDto(mapDomainToDto);

            return mapDomainToDto;
        }

        private void MapDeptToDto(List<ApoClassDto> mapDomainToDto)
        {
            mapDomainToDto.ForEach(MapDeptToDto);
        }

        private void MapDeptToDto(ApoClassDto domain)
        {
            domain.DepartmentName = ApoDepartmentInstances.GetInstance(_apoDepartmentRepository).GetApoDepartmentDomains.Single(x => x.Id == domain.DepartmentId).Name;
        }

    }

}
