using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ApoSubClassService : IApoSubClassService
    {
        private readonly IApoClassRepository _apoClassRepository;
        private readonly IApoSubClassRepository _apoSubClassRepository;

        public ApoSubClassService(IApoClassRepository apoClassRepository, IApoSubClassRepository apoSubClassRepository)
        {
            _apoClassRepository = apoClassRepository;
            _apoSubClassRepository = apoSubClassRepository;
        }

        public PagedList<IApoSubClassDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IApoSubClassDataTranferObject> GetAll()
        {
            var selectedApoDivision = _apoSubClassRepository.GetAll();

            var mapToDto = Mapper.Map<IEnumerable<ApoSubClassDto>>(selectedApoDivision).ToList();

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoSubClassDataTranferObject GetById(int id)
        {
            var apoSubClassFromRepository = _apoSubClassRepository.GetById(id);

            if (apoSubClassFromRepository == null)
            {
                return null;
            }

            var MapToDomain = Mapper.Map<ApoSubClassDto>(apoSubClassFromRepository);

            MapDeptToDto(MapToDomain);

            return MapToDomain;
        }

        public IApoSubClassDataTranferObject Create(IApoSubClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoSubClassDomain>(item);


            if (_apoSubClassRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoGroupFromRepository = _apoSubClassRepository.Add(mapToDomain);

            var mapToDto = Mapper.Map<ApoSubClassDto>(apoGroupFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoSubClassDataTranferObject Edit(int id, IApoSubClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoSubClassDomain>(item);

            var selectedApoClass = _apoSubClassRepository.GetByName(item);

            if (selectedApoClass != null
                && selectedApoClass.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && id != selectedApoClass.Id)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoClassFromRepository = _apoSubClassRepository.Update(id, mapToDomain);

            var mapToDto = Mapper.Map<ApoSubClassDto>(apoClassFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public bool Delete(int id)
        {
            if (_apoSubClassRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoSubClassRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }
        }

        public IApoSubClassDataTranferObject GetByName(IApoSubClassForCreateOrEdit item)
        {
            var selectedApoClass = _apoSubClassRepository.GetByName(item);

            if (selectedApoClass == null)
            {
                return null;
            }

            var mapToDto = Mapper.Map<ApoSubClassDto>(selectedApoClass);

            MapDeptToDto(mapToDto);


            return mapToDto;
        }

        public PagedList<IApoSubClassDataTranferObject> GetAll(IApoSubClassResourceParameter apoSubClassResourceParameter)
        {
            var apoClassFromRepository = _apoSubClassRepository.GetAll(apoSubClassResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoSubClassDto>>(apoClassFromRepository);

            MapDeptToDto(mapDomainToDto);

            return PagedList<IApoSubClassDataTranferObject>.Create(mapDomainToDto.AsQueryable(), apoSubClassResourceParameter.Page,
                apoSubClassResourceParameter.PageSize);
        }

        public IEnumerable<IApoSubClassDataTranferObject> GetApoGroupByApoDivision(int id)
        {
            var apoGroupFromRepository = _apoSubClassRepository.GetByApoClass(id);

            var mapDomainToDto = Mapper.Map<List<ApoSubClassDto>>(apoGroupFromRepository);

            MapDeptToDto(mapDomainToDto);

            return mapDomainToDto;
        }

        private void MapDeptToDto(List<ApoSubClassDto> mapDomainToDto)
        {
            mapDomainToDto.ForEach(MapDeptToDto);
        }

        private void MapDeptToDto(ApoSubClassDto domain)
        {
            domain.ApoClassName = ApoClassInstances.GetInstance(_apoClassRepository).GetApoDepartmentDomains.Single(x => x.Id == domain.ApoClassId).Name;
        }
    }

}
