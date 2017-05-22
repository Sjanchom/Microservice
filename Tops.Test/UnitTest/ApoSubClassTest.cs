using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;
using TopsShareClass.Models.Domain;
using Xunit;

namespace Tops.Test.UnitTest
{
    public class ApoSubClassTest
    {
        private readonly IApoClassRepository _apoClassRepository;
        private readonly List<ApoClassDomain> _apoClass;
        private readonly IApoSubClassRepository _apoSubClassRepository;
        private readonly List<ApoSubClassDomain> _apoSubClass;

        public ApoSubClassTest()
        {
            MapperHelper.SetUpMapper();
            _apoSubClassRepository = SetUpMockHelper.GetApoSubClassRepository();
            _apoSubClass = DataInitializer.GetApoSubClass();
            _apoClass = DataInitializer.GetApoClass();
            _apoClassRepository = SetUpMockHelper.GetApoClassRepository();
        }

        [Fact]
        public void ApoSubClassReturnCorrectValueWhenAssignCriteria()
        {
            var service = new ApoSubClassService(_apoClassRepository,_apoSubClassRepository);

            var resource = new ApoSubClassResourceParameter()
            {
                Page = 2,
                PageSize = 5,
                ApoClassId = 1,
                SearchText = ""
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoSubClassDataTranferObject>>(sut);
            Assert.Equal(sut.CurrentPage, 2);
            Assert.Equal(sut.HasPrevious, true);
            Assert.Equal(sut.HasNext, sut.List.Count > (1 * 10));
        }

        [Fact]
        public void ApoSubClassServiceReturnCorrectListWhenAssingSearchtext()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassResourceParameter()
            {
                Page = 2,
                PageSize = 5,
                ApoClassId = 1,
                SearchText = "so"
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoSubClassDataTranferObject>>(sut);
            Assert.Equal(sut.CurrentPage, 2);
            Assert.Equal(sut.HasPrevious, true);
            Assert.Equal(sut.HasNext, sut.List.Count > (1 * 10));
            Assert.True(sut.List.All(x => x.Name.ToLowerInvariant().Contains("so")));

        }

        [Fact]
        public void ApoSubClassServiceReturnNullWhenCollectionNotExist()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassResourceParameter()
            {
                Page = 2,
                PageSize = 5,
                ApoClassId = 1,
                SearchText = "sossssss"
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoSubClassDataTranferObject>>(sut);
            Assert.True(!sut.List.Any());

        }

        [Fact]
        public void ApoClassServiceReturnCorrectId()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);
            var selectedClass = Mapper.Map<ApoSubClassDto>(_apoSubClass.Single(x => x.Id == 2));
            selectedClass.ApoClassName = _apoClass.Single(x => x.Id == selectedClass.ApoClassId).Name;

            var sut = service.GetById(2);


            AssertObjects.PropertyValuesAreEquals(sut, selectedClass);
        }

        [Fact]
        public void ApoClassServiceReturnNullWhenIdNotExist()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var sut = service.GetById(200000000);


            Assert.Null(sut);

        }

        [Fact]
        public void ApoClassReturnAllDataInCollection()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var sut = service.GetAll();


            Assert.IsType<List<ApoSubClassDto>>(sut);
            Assert.Equal(sut.Count(), _apoSubClass.Count(x => x.IsActive == 1));
        }


        [Fact]
        public void ApoClassReturnNewObjectWhenCreateSuccess()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Miscellaneous -- test",
                ApoClassId = 1
            };

            var className = _apoClass.Single(x => x.Id == 1).Name;

            var lastId = _apoSubClass.OrderByDescending(x => x.Id).First();


            var sut = service.Create(resource);

            Assert.IsType<ApoSubClassDto>(sut);
            Assert.Equal(sut.Id, lastId.Id + 1);
            Assert.Equal(sut.Name, resource.Name);
            Assert.Equal(sut.ApoClassId, 1);
            Assert.Equal(sut.ApoClassName, className);
        }

        [Fact]
        public void ApoClassServiceThrowErrorWhenAddDuplicateData()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Yeast",
                ApoClassId = 1
            };

            var exception = Record.Exception(() => service.Create(resource));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {resource.Name} is Already exist."));
        }

        [Fact]
        public void ApoClassServiceReturnEditObjectWhenUpdateSuccess()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Import Frozen Appetizer -- Update",
                ApoClassId = 106
            };

            var compareObj = new ApoSubClassDto()
            {
                Name = resource.Name,
                ApoClassId = resource.ApoClassId,
                Id = 105,
                IsActive = 1,
                Code = _apoSubClass.Single(x => x.Id == 105).Code,
                ApoClassName = _apoClass.Single(x => x.Id == resource.ApoClassId).Name
            };


            var sut = service.Edit(105, resource);

            Assert.IsType<ApoSubClassDto>(sut);
            AssertObjects.PropertyValuesAreEquals(sut, compareObj);

        }

        [Fact]
        public void UpdateShouldThrowErrorWhenUpdateDuplicateValueToCollection()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Import Frozen Appetizer",
                ApoClassId = 1
            };

            var exception = Record.Exception(() => service.Edit(2, resource));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {resource.Name} is Already exist."));
        }

        [Fact]
        public void UpdateShouldReturnCorrectValueWhenUpdateDuplicateValueButSameId()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Import Frozen Appetizer",
                ApoClassId = 106
            };

            var compareObj = new ApoSubClassDto()
            {
                Name = resource.Name,
                ApoClassId = resource.ApoClassId,
                Id = 105,
                IsActive = 1,
                Code = _apoSubClass.Single(x => x.Id == 105).Code,
                ApoClassName = _apoClass.Single(x => x.Id == resource.ApoClassId).Name
            };


            var sut = service.Edit(105, resource);

            Assert.IsType<ApoSubClassDto>(sut);
            AssertObjects.PropertyValuesAreEquals(sut, compareObj);
        }

        [Fact]
        public void ApoClassServiceShouldReturnSuccessWhenDeleteSuccess()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var sut = service.Delete(150);

            Assert.True(sut);
        }


        [Fact]
        public void ApoClassServiceReturnCorrectWhenDeleteFailed()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var sut = service.Delete(1508415123);

            Assert.False(sut);
        }

        [Fact]
        public void ApoDepartmentShouldReturnCorrectValueWhenSearchMatched()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Import Frozen Appetizer"
            };

            var selectedApo = _apoSubClass.Single(x => x.Name.Equals("Import Frozen Appetizer"));
            var className = _apoClass.Single(x => x.Id == selectedApo.ApoClassId).Name;

            var sut = service.GetByName(resource);


            Assert.Equal(sut.Name, "Import Frozen Appetizer");
            Assert.Equal(sut.ApoClassId, selectedApo.ApoClassId);
            Assert.Equal(sut.ApoClassName, className);
            Assert.Equal(sut.Id, selectedApo.Id);

        }

        [Fact]
        public void ApoDepartmentServiceShouldReturnNullWhenNameNotMatched()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var resource = new ApoSubClassForCreateOrEdit()
            {
                Name = "Miscellaneousssss"
            };

            var sut = service.GetByName(resource);


            Assert.Null(sut);
        }

        [Fact]
        public void ApoDepartmentserviceShouldReturnCorrectGroupWhenGetByGroup()
        {
            var service = new ApoSubClassService(_apoClassRepository, _apoSubClassRepository);

            var sut = service.GetApoGroupByApoDivision(1);

            Assert.IsType<List<ApoSubClassDto>>(sut);
            Assert.True(sut.Any());
            Assert.True(sut.All(x => x.ApoClassId == 1));
        }

    }

}
