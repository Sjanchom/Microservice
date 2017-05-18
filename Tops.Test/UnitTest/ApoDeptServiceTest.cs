﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Models.Instance;
using TopsShareClass.Models.DataTranferObjects;
using TopsShareClass.Models.Domain;
using Xunit;

namespace Tops.Test.UnitTest
{
    public class ApoDeptServiceTest
    {
        private readonly IApoDivisionRepository _apoDivisionRepository;
        private readonly IApoGroupRepository _apoGroupRepository;
        private readonly IApoDepartmentRepository _apoDepartmentRepository;
        private readonly List<ApoDivisionDomain> _apoDivision;
        private readonly List<ApoGroupDomain> _apoGroup;
        private readonly List<ApoDepartmentDomain> _apoDepartment;


        public ApoDeptServiceTest()
        {
            MapperHelper.SetUpMapper();
            _apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivision = DataInitializer.GetApoDivisions();
            _apoGroupRepository = SetUpMockHelper.GetApoGroupRepository();
            _apoGroup = DataInitializer.GetApoGroup();
            _apoDepartmentRepository = SetUpMockHelper.GetApoDepartmentRepository();
            _apoDepartment = DataInitializer.GetApoDepartment();

        }

        [Fact]
        public void ApoDeptShouldReturnCorrectValue()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var resource = new ApoDepartmentResourceParameter(1,10,1,0,"a");

            var sut = service.GetAll(resource);


            Assert.IsType<PagedList<IApoDepartmentDataTranferObject>>(sut);
            Assert.Equal(sut.List.Count , _apoDepartment.Count(x => x.GroupId == 0 && x.DivisionId == 1) > 10 ? 10 : _apoDepartment.Count(x => x.GroupId == 0 && x.DivisionId == 1));

            Assert.True(sut.List.All(x => _apoGroup.Single(g => g.Id == x.GroupId).Name.Equals(x.GroupName) 
            && _apoDivision.Single(d => d.Id == x.DivisionId).Name.Equals(x.DivisionName)
            && (x.Name.ToLowerInvariant().Contains("a") ||x.Code.ToLowerInvariant().Contains("a")) ));
        }

        [Fact]
        public void ApoDepartmentReturnCorrectValueWhenCriteriaNull()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);



            var resource = new ApoDepartmentResourceParameter(1, 10, null, null, null);

            var sut = service.GetAll(resource);


            Assert.IsType<PagedList<IApoDepartmentDataTranferObject>>(sut);
            Assert.True(sut.List.All(x => x.IsActive == 1));
        }

        [Fact]
        public void ApoDepartmentServiceReturnAllList()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var sut = service.GetAll();

            Assert.IsType<List<ApoDepartmentDto>>(sut);
            Assert.Equal(sut.Count(), _apoDepartment.Count());
        }

        [Fact]
        public void ApoDepartmentServiceGetByIdReturnCorrectApo()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var sut = service.GetById(10);

            var compareObj = Mapper.Map<ApoDepartmentDto>(_apoDepartment.Single(x => x.Id == 10));
            compareObj.DivisionName = _apoDivision.Single(x => x.Id == compareObj.DivisionId).Name;
            compareObj.GroupName = _apoGroup.Single(x => x.Id == compareObj.GroupId).Name;

            AssertObjects.PropertyValuesAreEquals(sut, compareObj);
        }


        [Fact]
        public void ApoDepartmentServiceGetByIdReturnNullWhenNoExistIdinDatabse()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var sut = service.GetById(1000);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoDepartmentServiceReturnCorrectIdAndValueWhenCreateSuccess()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var resource = new ApoDepartmentCreateOrEdit()
            {
                ApoDivisionId = 1,
                ApoGroupId = 0,
                Name = "New Dept"
            };

            var sut = service.Create(resource);

            var lastId = _apoDepartment.Where(x => x.DivisionId == 1 && x.GroupId == 0).Select(x => x.Id).Max() +1 ;
        

            Assert.IsType<ApoDepartmentDto>(sut);
            Assert.Equal(sut.Id,lastId);
            Assert.Equal(sut.DivisionId,1);
            Assert.Equal(sut.GroupId,0);
            Assert.Equal(sut.Name,"New Dept");

        }

        [Fact]
        public void ApoDepartmentServiceReturnErrorWhenCreateDuplicateName()
        {
            var service = new ApoDepartmentService(_apoDivisionRepository, _apoGroupRepository, _apoDepartmentRepository);

            var resource = new ApoDepartmentCreateOrEdit()
            {
                ApoDivisionId = 1,
                ApoGroupId = 0,
                Name = "Beverages"
            };

            var exception = Record.Exception(() => service.Create(resource));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {resource.Name} is Already exist."));

        }


    }

    public class ApoDepartmentCreateOrEdit : IApoDepartmentForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApoDivisionId { get; set; }
        public int ApoGroupId { get; set; }
    }


    public class ApoDepartmentResourceParameter: IApoDepartmentResourceParameter
    {
        public ApoDepartmentResourceParameter(int page, int pageSize, int? apoDivision, int? apoGroup, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
            ApoDivisionId = apoDivision;
            ApoGroupId = apoGroup;

        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDivisionId { get; set; }
        public int? ApoGroupId { get; set; }
    }

    public interface IApoDepartmentResourceParameter : IBaseResourceParameter
    {
        int? ApoDivisionId { get; set; }
        int? ApoGroupId { get; set; }
    }

    public class ApoDepartmentService: IApoDepartmentService
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

            MapDivisionToDto(mapToObj);
            MapGroupToDto(mapToObj);

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

            MapDivisionToDto(maptoDto);

            return maptoDto;
        }

        public IApoDepartmentDataTranferObject Edit(int id, IApoDepartmentForCreateOrEdit item)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IApoDepartmentDataTranferObject GetByName(IApoDepartmentForCreateOrEdit item)
        {
            throw new System.NotImplementedException();
        }

     

        public PagedList<IApoDepartmentDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IApoDepartmentDataTranferObject> GetApoDepartmentByApoGroup(int id)
        {
            throw new System.NotImplementedException();
        }

        private void MapDivisionAndGroupToDto(List<ApoDepartmentDto> apoDepartmentDtos)
        {
            foreach (var apoDepartmentDto in apoDepartmentDtos)
            {
                MapDivisionToDto(apoDepartmentDto);
                MapGroupToDto(apoDepartmentDto);
            }
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

    public class ApoDepartmentDto : IApoDepartmentDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int DivisionId { get; set; }
        public int GroupId { get; set; }
        public string DivisionName { get; set; }
        public string GroupName { get; set; }
    }

    public interface IApoDepartmentRepository : IApoBaseRepository<IApoDepartmentDomain>
    {
        IQueryable<IApoDepartmentDomain> GetAll(IApoDepartmentResourceParameter resourceParameters);
        IApoDepartmentDomain GetByName(IApoDepartmentForCreateOrEdit item);
        IQueryable<IApoDepartmentDomain> GetByApoGroup(int id);
    }

    public interface IApoDepartmentService:IApoBaseService<IApoDepartmentDataTranferObject, IApoDepartmentForCreateOrEdit>
    {
        PagedList<IApoDepartmentDataTranferObject> GetAll(IApoDepartmentResourceParameter apoDepartmentResourceParameter);
        IEnumerable<IApoDepartmentDataTranferObject> GetApoDepartmentByApoGroup(int id);
    }

    public interface IApoDepartmentForCreateOrEdit: IApoBaseForCreateOrEdit
    {
        int ApoDivisionId { get; set; }
        int ApoGroupId { get; set; }
    }

    public interface IApoDepartmentDataTranferObject: IApoBase
    {
        int DivisionId { get; set; }
        int GroupId { get; set; }
        string DivisionName { get; set; }
        string GroupName { get; set; }
    }

    public class ApoDepartmentDomain:IApoDepartmentDomain
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int EditBy { get; set; }
        public int LastEditBy { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public string Remark { get; set; }
        public int GroupId { get; set; }
        public int DivisionId { get; set; }
    }
}
