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
    public class ApoGroupService : IApoGroupService
    {
        private readonly IApoGroupRepository _apoGroupRepository;
        private readonly IApoDivisionRepository _apoDivisionRepository;

        public ApoGroupService(IApoGroupRepository apoGroupGroupRepository, IApoDivisionRepository apoDivisionRepository)
        {
            _apoGroupRepository = apoGroupGroupRepository;
            _apoDivisionRepository = apoDivisionRepository;
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(IApoGroupResourceParameter apoGroupResourceParameter)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetAll(apoGroupResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoGroupDto>>(apoGroupFromRepository);


            return PagedList<IApoGroupDataTranferObject>.Create(MapDivisionToDto(mapDomainToDto).AsQueryable(), apoGroupResourceParameter.Page,
                apoGroupResourceParameter.PageSize);
        }

        public IEnumerable<IApoGroupDataTranferObject> GetApoGroupByApoDivision(int id)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetByApoDivision(id);

            var mapDomainToDto = Mapper.Map<List<ApoGroupDto>>(apoGroupFromRepository);

            return MapDivisionToDto(mapDomainToDto);
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IApoGroupDataTranferObject> GetAll()
        {
            var selectedApoDivision = _apoGroupRepository.GetAll();

            return MapDivisionToDto(Mapper.Map<IEnumerable<ApoGroupDto>>(selectedApoDivision).ToList());
        }

        public IApoGroupDataTranferObject GetById(int id)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetById(id);

            if (apoGroupFromRepository == null)
            {
                return null;
            }

            return MapDivisionToDto(Mapper.Map<ApoGroupDto>(apoGroupFromRepository));
        }

        public IApoGroupDataTranferObject Create(IApoGroupForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoGroupDomain>(item);


            if (_apoGroupRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoGroupFromRepository = _apoGroupRepository.Add(mapToDomain);

            return MapDivisionToDto(Mapper.Map<ApoGroupDto>(apoGroupFromRepository));
        }

        public IApoGroupDataTranferObject Edit(int id, IApoGroupForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoGroupDomain>(item);

            var selectedApoDivision = _apoGroupRepository.GetByName(item);

            if (selectedApoDivision != null
                && selectedApoDivision.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && id != selectedApoDivision.Id)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoDivisionFromRepository = _apoGroupRepository.Update(id, mapToDomain);

            return MapDivisionToDto(Mapper.Map<ApoGroupDto>(apoDivisionFromRepository));
        }

        public bool Delete(int id)
        {
            if (_apoGroupRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoGroupRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }
        }

        public IApoGroupDataTranferObject GetByName(IApoGroupForCreateOrEdit item)
        {
            var selectedApoDivision = _apoGroupRepository.GetByName(item);

            if (selectedApoDivision == null)
            {
                return null;
            }

            return MapDivisionToDto(Mapper.Map<ApoGroupDto>(selectedApoDivision));
        }

        private List<ApoGroupDto> MapDivisionToDto(List<ApoGroupDto> apoGroupDtos)
        {
            foreach (var apoGroupDto in apoGroupDtos)
            {
                apoGroupDto.DivisionName = ApoDivisionInstances.GetInstance(_apoDivisionRepository).GetApoDivisionDomains.Single(x => x.Id == apoGroupDto.DivisionId).Name;
            }

            return apoGroupDtos;
        }

        private ApoGroupDto MapDivisionToDto(ApoGroupDto apoGroupDto)
        {
            apoGroupDto.DivisionName = ApoDivisionInstances.GetInstance(_apoDivisionRepository).GetApoDivisionDomains.Single(x => x.Id == apoGroupDto.DivisionId).Name;
            return apoGroupDto;
        }


    }

}
