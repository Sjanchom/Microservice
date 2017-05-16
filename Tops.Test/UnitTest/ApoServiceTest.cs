using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Models.DataTranferObjects;
using TopsService.Models.Domain;
using TopsService.Services;
using Xunit;

namespace Tops.Test.UnitTest
{
    public class ApoServiceTest
    {
        private readonly IApoDivisionRepository _apoDivisionRepository;
        private readonly IApoGroupRepository _apoGroupRepository;
        private readonly List<ApoDivisionDomain> _apoDivision;
        private readonly List<ApoGroupDomain> _apoGroup;

        public ApoServiceTest()
        {
            MapperHelper.SetUpMapper();
            _apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivision = DataInitializer.GetApoDivisions();
            _apoGroupRepository = SetUpMockHelper.GetApoGroupRepository();
            _apoGroup = DataInitializer.GetApoGroup();
        }

        #region ApoDivision

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrect()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(1, 5, null);

            Assert.Equal(sut.CurrentPage,1);
            Assert.Equal(sut.HasNext,true);
            Assert.Equal(sut.HasPrevious,false);
            Assert.True(sut.List.Count() <= 5);
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignInCorrectPage()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(10, 5, null);

           
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.List.Count,0);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenAssignExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(1, 5, "Food");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.True(sut.List.All(x => x.Code.Contains("Food") || x.Name.Contains("Food")));
            Assert.Equal(sut.List.Count, _apoDivision.Count(x => x.Code.ToLowerInvariant().Contains("Food".ToLowerInvariant()) 
            || x.Name.ToLowerInvariant().Contains("Food".ToLowerInvariant())));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignNoExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(1, 5, "ddsifjdigjs");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.List.Count, 0);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectIdWhenExist()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetById(1);


            Assert.IsType<ApoDivisionDto>(sut);
            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoDivisionDto>(_apoDivision.Single(x => x.Id == 1)));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenIdIsNotExistInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetById(100);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNewElementWhenCreateSuccess()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var newApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "New Apo"
            };
            
            var sut = service.Create(newApo);

            Assert.Equal(sut.Id, _apoDivision.Last().Id + 1);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenNameIsAlreadyExist()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var newApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "food"
            };

            var sut = service.Create(newApo);

            Assert.Null(sut);

        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenEditSuccess()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var editApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "FOOD EDIT",
            };

            var sut = service.Edit(0,editApo);

            Assert.Equal(sut.Id, 0);
            Assert.Equal(sut.Name, "FOOD EDIT");
        }

        [Fact]
        public void ApoDivisonServiceShouldReturnNullWhenUpdateDuplicateValueButNotOwnId()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var editApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "FOOD",
            };

            var sut = service.Edit(10, editApo);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenUpdateSameValueToSameId()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var editApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "Food",
            };

            var sut = service.Edit(0, editApo);

            Assert.Equal(sut.Id, 0);
            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoDivisionDto>(_apoDivision.Single(x => x.Id ==0)));
        }

        [Fact]
        public void ApoDivisionShouldReturnTrueWhenDelteSuccess()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.Delete(0);

            Assert.True(sut);
        }

        [Fact]
        public void ApoDivionShouldReturnFalseWhenDeleteFailed()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.Delete(150);

            Assert.False(sut);
        }

        [Fact]
        public void ApoDivisionShouldReturnCorrectvaluewhenSearchByName()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var searchObj = new ApoDivisionForCreateOrEdit()
            {
                Name = "Food"
            };

            var sut = service.GetByName(searchObj);

            var shouldEqual = _apoDivision.Single(x => x.Name.Equals(searchObj.Name));


            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoDivisionDto>(shouldEqual));

        }

        #endregion


        #region ApoGroup
        [Fact]
        public void ApoGroupShouldReturnCorrectValueWhenGetAll()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var parameter = new ApoGroupResourceParameter(1, 10, null, "");

            var sut = service.GetAll(parameter);


            Assert.Equal(sut.CurrentPage, 1);
            Assert.Equal(sut.HasNext, true);
            Assert.Equal(sut.HasPrevious, false);
            Assert.True(sut.List.Count() <= 10);
            Assert.IsType<PagedList<IApoGroupDataTranferObject>>(sut);
        }

        [Fact]
        public void ApoGroupShouldReturnCorrectCriteriaWhenGivenDivision()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var parameter = new ApoGroupResourceParameter(1, 10, 1, "");

            var sut = service.GetAll(parameter);

            Assert.Equal(sut.CurrentPage, 1);
            Assert.Equal(sut.HasNext, (_apoGroup.Count(x => x.DivisionId == parameter.ApoDivsionId) / 10) > 1);
            Assert.Equal(sut.HasPrevious, false);
            Assert.True(sut.List.Count() <= 10);
            Assert.IsType<PagedList<IApoGroupDataTranferObject>>(sut);
            Assert.True(sut.List.All(x => x.DivisionId == parameter.ApoDivsionId));

        }

        [Fact]
        public void ApoGroupReturnCorrectValueWhenSearchText()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var parameter = new ApoGroupResourceParameter(1,5,1, "Tobacco");

            var sut = service.GetAll(parameter);

            Assert.True(sut.List.All(x => x.Name.ToLowerInvariant().Contains(parameter.SearchText.ToLowerInvariant())
            || x.Code.ToLowerInvariant().Contains(parameter.SearchText.ToLowerInvariant())));

        }

        [Fact]
        public void ApoGroupReturnNullWhenGivenNoExistValue()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var parameter = new ApoGroupResourceParameter(1, 5, 1, "Tobacasdasdco");

            var sut = service.GetAll(parameter);

            Assert.True(!sut.List.Any());

        }

        [Fact]
        public void ApoGroupReturnCorrectObjectWhenAssignCorrectId()
        {
            var service = new ApoGroupService(_apoGroupRepository);


            var sut = service.GetById(10);

            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<IApoGroupDataTranferObject>(_apoGroup.Single(x => x.Id == 10)));
        }


        #endregion



    }

    public class ApoGroupDomain : IApoGroupDomain
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int EditBy { get; set; }
        public int LastEditBy { get; set; }
        public int IsActive { get; set; }
        public string Remark { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DivisionId { get; set; }
    }

    public class ApoGroupResourceParameter : IApoGroupResourceParameter
    {
        public ApoGroupResourceParameter(int page, int pageSize, int? apoDivisionId, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            ApoDivsionId = apoDivisionId;
            SearchText = searchText;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDivsionId { get; set; }
    }

    public interface IApoGroupResourceParameter : IBaseResourceParameter
    {
        int? ApoDivsionId { get; set; }
    }

    public interface IApoGroupService : IApoBaseService<IApoGroupDataTranferObject, IApoGroupDataTranferObject>
    {
        PagedList<IApoGroupDataTranferObject> GetAll(IApoGroupResourceParameter apoGroupResourceParameter);
    }

    public interface IApoGroupForCreateOrEdit:IApoBaseForCreateOrEdit
    {
        int ApoDivisionId { get; set; }
    }

    public interface IApoGroupDataTranferObject : IApoBase
    {
        int DivisionId { get; set; }
    }


    public class ApoGroupService : IApoGroupService
    {
        private readonly IApoGroupRepository _apoRepository;

        public ApoGroupService(IApoGroupRepository apoGroupRepository)
        {
            _apoRepository = apoGroupRepository;
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(IApoGroupResourceParameter apoGroupResourceParameter)
        {
            var apoGroupFromRepository = _apoRepository.GetAll(apoGroupResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoGroupDto>>(apoGroupFromRepository);

            return PagedList<IApoGroupDataTranferObject>.Create(mapDomainToDto.AsQueryable(), apoGroupResourceParameter.Page,
                apoGroupResourceParameter.PageSize);
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IApoGroupDataTranferObject GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IApoGroupDataTranferObject Create(IApoGroupDataTranferObject item)
        {
            throw new System.NotImplementedException();
        }

        public IApoGroupDataTranferObject Edit(int id, IApoGroupDataTranferObject item)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IApoGroupDataTranferObject GetByName(IApoGroupDataTranferObject item)
        {
            throw new System.NotImplementedException();
        }

       
    }

    public class ApoGroupDto : IApoGroupDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DivisionId { get; set; }
    }
    

    public interface IApoGroupRepository : IApoBaseRepository<IApoGroupDomain>
    {
        IQueryable<IApoGroupDomain> GetAll(IApoGroupResourceParameter resourceParameters);
    }
}
