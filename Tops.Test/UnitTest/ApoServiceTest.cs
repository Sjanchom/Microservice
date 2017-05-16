using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
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
        private List<ApoDivisionDomain> _apoDivision;

        public ApoServiceTest()
        {
            MapperHelper.SetUpMapper();
            _apoDivisionRepository = SetUpMockHelper.GetApoDivisionRepository();
            _apoDivision = DataInitializer.GetApoDivisions();
        }

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

    }

}
