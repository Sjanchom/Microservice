using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
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

            var resource = new ApoDepartmentResourceParameter(1,10,1,0,"");

            var sut = service.GetAll(resource);


            Assert.IsType<PagedList<IApoDepartmentDataTranferObject>>(sut);
            Assert.True(sut.List.Count == _apoDepartment.Count(x => x.GroupId == 0 && x.DivisionId == 1));
            Assert.Equal(sut.List.Count,_apoDepartment.Count(x => x.GroupId == 0 && x.DivisionId == 0));
        }
    }

  

    public class ApoDepartmentResourceParameter: IApoDepartmentResourceParameter
    {
        public ApoDepartmentResourceParameter(int page, int pageSize, int? apoDivision, int? apoGroup, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
            ApoDivision = apoDivision;
            ApoGroup = apoGroup;

        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDivision { get; set; }
        public int? ApoGroup { get; set; }
    }

    public interface IApoDepartmentResourceParameter : IBaseResourceParameter
    {
        int? ApoDivision { get; set; }
        int? ApoGroup { get; set; }
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

            return PagedList<IApoDepartmentDataTranferObject>.Create(apoMapToDepartment.AsQueryable(),
                apoDepartmentResourceParameter.Page, apoDepartmentResourceParameter.PageSize);
        }

        public IEnumerable<IApoDepartmentDataTranferObject> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IApoDepartmentDataTranferObject GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IApoDepartmentDataTranferObject Create(IApoDepartmentForCreateOrEdit item)
        {
            throw new System.NotImplementedException();
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
        IQueryable<IApoDepartmentDomain> GetAll(IApoGroupResourceParameter resourceParameters);
        IApoDepartmentDomain GetByName(IApoGroupForCreateOrEdit item);
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
