using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Services;
using Xunit;
using TopsShareClass.Models.Domain;
using TopsShareClass.Models.DataTranferObjects;

namespace Tops.Test.UnitTest
{
    public class ApoServiceTest
    {
        private readonly IApoDivisionRepository _apoDivisionRepository;
        private readonly IApoGroupRepository _apoGroupRepository;
        private readonly List<ApoDivisionDomain> _apoDivision;
        private readonly List<ApoGroupDomain> _apoGroup;
        private readonly IApoGroupService _apoGroupService;
        private readonly IApoDivisionService _apoDivisionService;

        public ApoServiceTest()
        {
            MapperHelper.SetUpMapper();
            _apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivision = DataInitializer.GetApoDivisions();
            _apoGroupRepository = SetUpMockHelper.GetApoGroupRepository();
            _apoGroup = DataInitializer.GetApoGroup();
            _apoGroupService = SetUpMockHelper.GetApoGroupService();
            _apoDivisionService = SetUpMockHelper.GetApoDivisionService();
        }

        #region ApoDivision

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrect()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetAll(1, 10, null);

            Assert.Equal(sut.CurrentPage,1);
            Assert.Equal(sut.HasNext,false);
            Assert.Equal(sut.HasPrevious,false);
            Assert.True(sut.List.Count() <= 10);
            Assert.True(sut.List.All(x => x.IsActive == 1));
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignInCorrectPage()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetAll(10, 5, null);

           
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.List.Count,0);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenAssignExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetAll(1, 5, "Food");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.True(sut.List.All(x => x.IsActive == 1));
            Assert.True(sut.List.Count > 0);
            Assert.True(sut.List.All(x => x.Code.Contains("Food") || x.Name.Contains("Food")));
            Assert.Equal(sut.List.Count, _apoDivision.Count(x => x.Code.ToLowerInvariant().Contains("Food".ToLowerInvariant()) 
            || x.Name.ToLowerInvariant().Contains("Food".ToLowerInvariant())));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignNoExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetAll(1, 5, "ddsifjdigjs");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.List.Count, 0);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectIdWhenExist()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetById(1);


            Assert.IsType<ApoDivisionDto>(sut);
            Assert.True(sut.IsActive == 1);
            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoDivisionDto>(_apoDivision.Single(x => x.Id == 1)));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenIdIsNotExistInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetById(100);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenIdIsIsNotActive()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.GetById(9);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNewElementWhenCreateSuccess()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var newApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "New Apo"
            };
            

            var nextCode = (Convert.ToInt32(_apoDivision.Last().Code) + 1).ToString("D2");
            var compareEqualObject = Mapper.Map<ApoDivisionDto>(newApo);
            compareEqualObject.Code = nextCode;
            compareEqualObject.Id = _apoDivision.Last().Id + 1;

            var sut = service.Create(newApo);

            Assert.Equal(sut.Id, _apoDivision.Last().Id + 1);
            AssertObjects.PropertyValuesAreEquals(sut, compareEqualObject);

        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenNameIsAlreadyExist()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var newApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "food"
            };



            var exception = Record.Exception(() => service.Create(newApo));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {newApo.Name} is Already exist."));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenEditSuccess()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

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
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var editApo = new ApoDivisionForCreateOrEdit()
            {
                Name = "FOOD",
            };


            var exception = Record.Exception(() => service.Edit(10, editApo));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {editApo.Name} is Already exist."));

        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenUpdateSameValueToSameId()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

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
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.Delete(9);

            Assert.False(sut);
        }

        [Fact]
        public void ApoDivisionShouldReturnFailWhenDeleteObjectThatHasChild()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var exception = Record.Exception(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
            Assert.True(exception.Message.Contains($"Id :{0} has child hierachy."));
        }

        [Fact]
        public void ApoDivionShouldReturnFalseWhenDeleteFailed()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var sut = service.Delete(150);

            Assert.False(sut);
        }

        [Fact]
        public void ApoDivisionShouldReturnCorrectvaluewhenSearchByName()
        {
            var service = new ApoDivisionService(_apoDivisionRepository, _apoGroupService);

            var searchObj = new ApoDivisionForCreateOrEdit()
            {
                Name = "Food"
            };

            var sut = service.GetByName(searchObj);

            var shouldEqual = _apoDivision.Single(x => x.Name.Equals(searchObj.Name));


            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoDivisionDto>(shouldEqual));

        }


        [Fact]
        public void ApoDivisionServiceShouldReturnAllItem()
        {
            var service = new ApoDivisionService(_apoDivisionRepository,_apoGroupService);

            var sut = service.GetAll();

            Assert.IsType<List<ApoDivisionDto>>(sut);
            Assert.Equal(sut.Count(), _apoDivision.Count());

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
            Assert.True(sut.List.All(x => x.DivisionName.Equals(_apoDivision.Single(a => a.Id == x.DivisionId).Name)));
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
            Assert.True(sut.List.All(x => x.DivisionName.Equals(_apoDivision.SingleOrDefault(a => a.Id == x.DivisionId)?.Name)));


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

            var compareObj = Mapper.Map<ApoGroupDto>(_apoGroup.Single(x => x.Id == 10));
            compareObj.DivisionName = _apoDivision.Single(x => x.Id == compareObj.DivisionId).Name;

            AssertObjects.PropertyValuesAreEquals(sut, compareObj);
        }

        [Fact]
        public void ApoGroupShouldReturnNullWhenIdIsNotExistInDatabase()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var sut = service.GetById(100);

            Assert.Null(sut);
        }

        [Fact]
        public void ApoGroupSouldReturnCreatedApoWhenCreateSuccess()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var createdApoGroup = new ApoGroupForCreateOrUpdate()
            {
                Name = "New Group",
            };

            var nextCode = (Convert.ToInt32(_apoGroup.Last().Code) + 1).ToString("D2");
            var compareEqualObject = Mapper.Map<ApoGroupDto>(createdApoGroup);
            compareEqualObject.Code = nextCode;
            compareEqualObject.Id = _apoGroup.Last().Id + 1;
            compareEqualObject.DivisionName = _apoDivision.Single(x => x.Id == compareEqualObject.DivisionId).Name;

            var sut = service.Create(createdApoGroup);

            Assert.Equal(sut.Id, _apoGroup.Last().Id + 1);
            AssertObjects.PropertyValuesAreEquals(sut,compareEqualObject);
        }

        [Fact]
        public void ApoGroupSouldReturnErrorWhenCreateAlreadyExistValue()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var createdApoGroup = new ApoGroupForCreateOrUpdate()
            {
                Name = "Packaged",
            };

           
            var exception = Record.Exception(() => service.Create(createdApoGroup));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {createdApoGroup.Name} is Already exist."));
        }

        [Fact]
        public void ApoGroupServiceShouldReturnCorrectValueWhenEditSuccess()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var updateApo = new ApoGroupForCreateOrUpdate()
            {
                Name = "Packaged -- new",
            };

            var sut = service.Edit(1, updateApo);

            Assert.Equal(sut.Name, "Packaged -- new");
            Assert.Equal(sut.Id,1);
        }

        [Fact]
        public void ApoGroupServiceShouldReturnNullWhenUpdateDuplicateValueButNotOwnId()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var editApo = new ApoGroupForCreateOrUpdate()
            {
                Name = "Packaged",
            };


            var exception = Record.Exception(() => service.Edit(10, editApo));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {editApo.Name} is Already exist."));

        }

        [Fact]
        public void ApoGroupServiceShouldReturnCorrectValueWhenUpdateSameValueToSameId()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var editApo = new ApoGroupForCreateOrUpdate()
            {
                Name = "Packaged",
            };

            var sut = service.Edit(1, editApo);

            var compareObj = Mapper.Map<ApoGroupDto>(_apoGroup.Single(x => x.Id == 1));
            compareObj.DivisionName = _apoDivision.Single(x => x.Id == compareObj.DivisionId).Name;

            Assert.Equal(sut.Id, 1);
            AssertObjects.PropertyValuesAreEquals(sut, compareObj);
        }

        [Fact]
        public void ApoGroupShouldReturnTrueWhenDelteSuccess()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var sut = service.Delete(0);

            Assert.True(sut);
        }

        [Fact]
        public void ApoGroupShouldReturnFalseWhenDeleteFailed()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var sut = service.Delete(150);

            Assert.False(sut);
        }

        [Fact]
        public void ApoGroupShouldReturnCorrectvaluewhenSearchByName()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var searchObj = new ApoGroupForCreateOrUpdate()
            {
                Name = "Packaged"
            };

            var sut = service.GetByName(searchObj);

            var compareObj = Mapper.Map<ApoGroupDto>(_apoGroup.Single(x => x.Name.Equals(searchObj.Name)));
            compareObj.DivisionName = _apoDivision.Single(x => x.Id == compareObj.DivisionId).Name;

            AssertObjects.PropertyValuesAreEquals(sut, Mapper.Map<ApoGroupDto>(compareObj));

        }

        [Fact]
        public void ApoGroupServiceShouldReturnAllItem()
        {
            var service = new ApoGroupService(_apoGroupRepository);

            var sut = service.GetAll();

            Assert.IsType<List<ApoGroupDto>>(sut);
            Assert.Equal(sut.Count(),_apoGroup.Count());

        }



        #endregion



    }

}
