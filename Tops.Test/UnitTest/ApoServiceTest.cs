using System;
using System.Collections;
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

        public ApoServiceTest()
        {
            MapperHelper.SetUpMapper();
            _apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivision = DataInitializer.GetApoDivisions();
            _apoGroupRepository = SetUpMockHelper.GetApoGroupRepository();
            _apoGroup = DataInitializer.GetApoGroup();
            _apoGroupService = SetUpMockHelper.GetApoGroupService();
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

            AssertObjects.PropertyValuesAreEquals(sut,Mapper.Map<ApoGroupDto>(_apoGroup.Single(x => x.Id == 10)));
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

            Assert.Equal(sut.Id, 1);
            AssertObjects.PropertyValuesAreEquals(sut, Mapper.Map<ApoGroupDto>(_apoGroup.Single(x => x.Id == 1)));
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

            var shouldEqual = _apoGroup.Single(x => x.Name.Equals(searchObj.Name));


            AssertObjects.PropertyValuesAreEquals(sut, Mapper.Map<ApoGroupDto>(shouldEqual));

        }



        #endregion



    }

    public class ApoGroupForCreateOrUpdate : IApoGroupForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApoDivisionId { get; set; }
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

   

  

 

  


    public class ApoGroupService : IApoGroupService
    {
        private readonly IApoGroupRepository _apoGroupRepository;

        public ApoGroupService(IApoGroupRepository apoGroupGroupRepository)
        {
            _apoGroupRepository = apoGroupGroupRepository;
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(IApoGroupResourceParameter apoGroupResourceParameter)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetAll(apoGroupResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoGroupDto>>(apoGroupFromRepository);

            return PagedList<IApoGroupDataTranferObject>.Create(mapDomainToDto.AsQueryable(), apoGroupResourceParameter.Page,
                apoGroupResourceParameter.PageSize);
        }

        public IEnumerable<IApoGroupDataTranferObject> GetApoGroupByApoDivision(int id)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetByApoDivision(id);

            var mapDomainToDto = Mapper.Map<List<ApoGroupDto>>(apoGroupFromRepository);

            return mapDomainToDto;
        }

        public PagedList<IApoGroupDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IApoGroupDataTranferObject GetById(int id)
        {
            var apoGroupFromRepository = _apoGroupRepository.GetById(id);

            if (apoGroupFromRepository == null)
            {
                return null;
            }

            return Mapper.Map<ApoGroupDto>(apoGroupFromRepository);
        }

        public IApoGroupDataTranferObject Create(IApoGroupForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoGroupDomain>(item);


            if (_apoGroupRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoGroupFromRepository = _apoGroupRepository.Add(mapToDomain);

            return Mapper.Map<ApoGroupDto>(apoGroupFromRepository);
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

            return Mapper.Map<ApoGroupDto>(apoDivisionFromRepository);
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

            return Mapper.Map<ApoGroupDto>(selectedApoDivision);
        }

       
    }

    public class ApoGroupDto : IApoGroupDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int DivisionId { get; set; }
    }
    

    public interface IApoGroupRepository : IApoBaseRepository<IApoGroupDomain>
    {
        IQueryable<IApoGroupDomain> GetAll(IApoGroupResourceParameter resourceParameters);
        ApoGroupDomain GetByName(IApoGroupForCreateOrEdit item);
        IQueryable<IApoGroupDomain> GetByApoDivision(int id);
    }
}
