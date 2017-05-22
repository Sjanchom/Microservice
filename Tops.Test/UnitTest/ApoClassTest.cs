using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
using TopsInterface.Repositories;
using TopsService.Models.Instance;
using TopsService.Services;
using TopsShareClass.Models.DataTranferObjects;
using TopsShareClass.Models.Domain;
using Xunit;

namespace Tops.Test.UnitTest
{
    public class ApoClassTest
    {
        private readonly IApoDepartmentRepository _apoDepartmentRepository;
        private readonly List<ApoDepartmentDomain> _apoDepartment;
        private readonly IApoClassRepository _apoClassRepository;
        private readonly List<ApoClassDomain> _apoClass;


        public ApoClassTest()
        {
            MapperHelper.SetUpMapper();
            _apoDepartmentRepository = SetUpMockHelper.GetApoDepartmentRepository();
            _apoDepartment = DataInitializer.GetApoDepartment();
            _apoClass = DataInitializer.GetApoClass();
            _apoClassRepository = SetUpMockHelper.GetApoClassRepository();
        }

        [Fact]
        public void ApoClassServiceShouldReturn()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassResourceParameter()
            {
                Page = 1,
                PageSize = 10,
                ApoDepartmentId = 2,
                SearchText = "c"
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoClassDataTranferObject>>(sut);
            Assert.Equal(sut.CurrentPage, 1);
            Assert.Equal(sut.HasPrevious, false);
            Assert.Equal(sut.HasNext, sut.List.Count > (1*10));
        }

        [Fact]
        public void ApoClassServiceReturnCorrectListWhenAssingSearchtext()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassResourceParameter()
            {
                Page = 1,
                PageSize = 10,
                SearchText = "na"
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoClassDataTranferObject>>(sut);
            Assert.Equal(sut.CurrentPage, 1);
            Assert.Equal(sut.HasPrevious, false);
            Assert.Equal(sut.HasNext, sut.List.Count > (1 * 10));
            Assert.True(sut.List.All(x => x.Name.ToLowerInvariant().Contains("na")));

        }

        [Fact]
        public void ApoClassServiceReturnNullWhenCollectionNotExist()
        {
            var service = new ApoClassService(_apoClassRepository,_apoDepartmentRepository);

            var resource = new ApoClassResourceParameter()
            {
                Page = 100,
                PageSize = 10,
                SearchText = "na"
            };

            var sut = service.GetAll(resource);

            Assert.IsType<PagedList<IApoClassDataTranferObject>>(sut);
            Assert.True(!sut.List.Any());

        }

        [Fact]
        public void ApoClassServiceReturnCorrectId()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);
            var selectedClass = Mapper.Map<ApoClassDto>(_apoClass.Single(x => x.Id == 2));
            selectedClass.DepartmentName = _apoDepartment.Single(x => x.Id == selectedClass.DepartmentId).Name;

            var sut = service.GetById(2);


            AssertObjects.PropertyValuesAreEquals(sut,selectedClass);
        }

        [Fact]
        public void ApoClassServiceReturnNullWhenIdNotExist()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var sut = service.GetById(2000000);


            Assert.Null(sut);

        }

        [Fact]
        public void ApoClassReturnAllDataInCollection()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var sut = service.GetAll();


            Assert.IsType<List<ApoClassDto>>(sut);
            Assert.Equal(sut.Count(), _apoClass.Count(x => x.IsActive == 1));
        }


        [Fact]
        public void ApoClassReturnNewObjectWhenCreateSuccess()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous -- test",
                ApoDepartmentId = 1
            };

            var deptName = _apoDepartment.Single(x => x.Id == 1).Name;

            var lastId = _apoClass.OrderByDescending(x => x.Id).First();


            var sut = service.Create(resource);

            Assert.IsType<ApoClassDto>(sut);
            Assert.Equal(sut.Id, lastId.Id +1 );
            Assert.Equal(sut.Name,resource.Name);
            Assert.Equal(sut.DepartmentId, 1);
            Assert.Equal(sut.DepartmentName, deptName);
        }

        [Fact]
        public void ApoClassServiceThrowErrorWhenAddDuplicateData()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous",
                ApoDepartmentId = 1
            };

            var exception = Record.Exception(() => service.Create(resource));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {resource.Name} is Already exist."));
        }

        [Fact]
        public void ApoClassServiceReturnEditObjectWhenUpdateSuccess()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous -- Update",
                ApoDepartmentId = 1
            };

            var compareObj = new ApoClassDto()
            {
                Name = resource.Name,
                DepartmentId = resource.ApoDepartmentId,
                Id = 91,
                IsActive = 1,
                Code = _apoClass.Single(x => x.Id == 91).Code,
                DepartmentName = _apoDepartment.Single(x => x.Id == resource.ApoDepartmentId).Name
            };


            var sut = service.Edit(91, resource);

            Assert.IsType<ApoClassDto>(sut);
            AssertObjects.PropertyValuesAreEquals(sut,compareObj);

        }

        [Fact]
        public void UpdateShouldThrowErrorWhenUpdateDuplicateValueToCollection()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous",
                ApoDepartmentId = 1
            };

            var exception = Record.Exception(() => service.Edit(2, resource));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.True(exception.Message.Contains($"Name {resource.Name} is Already exist."));
        }

        [Fact] 
        public void UpdateShouldReturnCorrectValueWhenUpdateDuplicateValueButSameId()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous",
                ApoDepartmentId = 1
            };

            var compareObj = new ApoClassDto()
            {
                Name = resource.Name,
                DepartmentId = resource.ApoDepartmentId,
                Id = 91,
                IsActive = 1,
                Code = _apoClass.Single(x => x.Id == 91).Code,
                DepartmentName = _apoDepartment.Single(x => x.Id == resource.ApoDepartmentId).Name
            };


            var sut = service.Edit(91, resource);

            Assert.IsType<ApoClassDto>(sut);
            AssertObjects.PropertyValuesAreEquals(sut,compareObj);
        }

        [Fact]
        public void ApoClassServiceShouldReturnSuccessWhenDeleteSuccess()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var sut = service.Delete(150);

            Assert.True(sut);
        }


        [Fact]
        public void ApoClassServiceReturnCorrectWhenDeleteFailed()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var sut = service.Delete(1508415123);

            Assert.False(sut);
        }

        [Fact]
        public void ApoDepartmentShouldReturnCorrectValueWhenSearchMatched()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneous"
            };

            var selectedApo = _apoClass.Single(x => x.Name.Equals("Miscellaneous"));
            var deptName = _apoDepartment.Single(x => x.Id == selectedApo.ApoDepartmentId).Name;

            var sut = service.GetByName(resource);


            Assert.Equal(sut.Name, "Miscellaneous");
            Assert.Equal(sut.DepartmentId, selectedApo.ApoDepartmentId);
            Assert.Equal(sut.DepartmentName, deptName);
            Assert.Equal(sut.Id, selectedApo.Id);

        }

        [Fact]
        public void ApoDepartmentServiceShouldReturnNullWhenNameNotMatched()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var resource = new ApoClassForCreateOrEdit()
            {
                Name = "Miscellaneousssss"
            };

            var sut = service.GetByName(resource);


            Assert.Null(sut);
        }

        [Fact]
        public void ApoDepartmentserviceShouldReturnCorrectGroupWhenGetByGroup()
        {
            var service = new ApoClassService(_apoClassRepository, _apoDepartmentRepository);

            var sut = service.GetApoClassByApoDepartment(1);

            Assert.IsType<List<ApoClassDto>>(sut);
            Assert.True(sut.Any());
            Assert.True(sut.All(x => x.DepartmentId == 1));
        }


    }
   
}
