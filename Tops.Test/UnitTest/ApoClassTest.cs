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

    public class ApoClassForCreateOrEdit : IApoClassForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApoDepartmentId { get; set; }
    }

    public class ApoClassDomain:IApoClassDomain
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
        [JsonProperty("ApoDeptId")]
        public int ApoDepartmentId { get; set; }
    }

    public class ApoClassDto: IApoClassDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class ApoClassResourceParameter:IApoClassResourceParameter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int? ApoDepartmentId { get; set; }

        public ApoClassResourceParameter()
        {
            
        }

        public ApoClassResourceParameter(int page,int pageSize,int? apoDepartmentId,string searchText)
        {
            Page = page;
            PageSize = pageSize;
            ApoDepartmentId = apoDepartmentId;
            SearchText = searchText;
        }
    }

    public interface IApoClassResourceParameter : IBaseResourceParameter
    {
        int? ApoDepartmentId { get; set; }
    }

    public class ApoClassService:IApoClassService
    {
        private readonly IApoClassRepository _apoClassRepository;
        private readonly IApoDepartmentRepository _apoDepartmentRepository;

        public ApoClassService(IApoClassRepository apoClassRepository, IApoDepartmentRepository apoDepartmentRepository)
        {
            _apoClassRepository = apoClassRepository;
            _apoDepartmentRepository = apoDepartmentRepository;
        }

        public PagedList<IApoClassDataTranferObject> GetAll(int page, int pageSize, string searchText)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IApoClassDataTranferObject> GetAll()
        {
            var selectedApoDivision = _apoClassRepository.GetAll();

            var mapToDto = Mapper.Map<IEnumerable<ApoClassDto>>(selectedApoDivision).ToList();

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoClassDataTranferObject GetById(int id)
        {
            var apoClassFromRepository = _apoClassRepository.GetById(id);

            if (apoClassFromRepository == null)
            {
                return null;
            }

            var MapToDomain = Mapper.Map<ApoClassDto>(apoClassFromRepository);

            MapDeptToDto(MapToDomain);

            return MapToDomain;
        }

        public IApoClassDataTranferObject Create(IApoClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoClassDomain>(item);


            if (_apoClassRepository.GetByName(item) != null)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoGroupFromRepository = _apoClassRepository.Add(mapToDomain);

            var mapToDto = Mapper.Map<ApoClassDto>(apoGroupFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public IApoClassDataTranferObject Edit(int id, IApoClassForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoClassDomain>(item);

            var selectedApoClass = _apoClassRepository.GetByName(item);

            if (selectedApoClass != null
                && selectedApoClass.Name.ToLowerInvariant().Equals(item.Name.Trim().ToLowerInvariant())
                && id != selectedApoClass.Id)
            {
                throw new ArgumentException($"Name {item.Name} is Already exist.");
            }

            var apoClassFromRepository = _apoClassRepository.Update(id, mapToDomain);

            var mapToDto = Mapper.Map<ApoClassDto>(apoClassFromRepository);

            MapDeptToDto(mapToDto);

            return mapToDto;
        }

        public bool Delete(int id)
        {
            if (_apoClassRepository.GetById(id) == null)
                return false;
            try
            {
                var status = _apoClassRepository.Delete(id);
                return status;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Delete not Success with Internal Error");
            }
        }

        public IApoClassDataTranferObject GetByName(IApoClassForCreateOrEdit item)
        {
            var selectedApoClass = _apoClassRepository.GetByName(item);

            if (selectedApoClass == null)
            {
                return null;
            }

            var mapToDto = Mapper.Map<ApoClassDto>(selectedApoClass);

            MapDeptToDto(mapToDto);


            return mapToDto;
        }

        public PagedList<IApoClassDataTranferObject> GetAll(IApoClassResourceParameter apoClassResourceParameter)
        {
            var apoClassFromRepository = _apoClassRepository.GetAll(apoClassResourceParameter);

            var mapDomainToDto = Mapper.Map<List<ApoClassDto>>(apoClassFromRepository);

            MapDeptToDto(mapDomainToDto);

            return PagedList<IApoClassDataTranferObject>.Create(mapDomainToDto.AsQueryable(), apoClassResourceParameter.Page,
                apoClassResourceParameter.PageSize);
        }

        public IEnumerable<IApoClassDataTranferObject> GetApoClassByApoDepartment(int id)
        {
            var apoGroupFromRepository = _apoClassRepository.GetByApoDepartment(id);

            var mapDomainToDto = Mapper.Map<List<ApoClassDto>>(apoGroupFromRepository);

            MapDeptToDto(mapDomainToDto);

            return mapDomainToDto;
        }

        private void MapDeptToDto(List<ApoClassDto> mapDomainToDto)
        {
            mapDomainToDto.ForEach(MapDeptToDto);
        }

        private void MapDeptToDto(ApoClassDto domain)
        {
            domain.DepartmentName = ApoDepartmentInstances.GetInstance(_apoDepartmentRepository).GetApoDepartmentDomains.Single(x => x.Id == domain.DepartmentId).Name;
        }

    }

    public interface IApoClassRepository:IApoBaseRepository<IApoClassDomain>
    {
        IQueryable<IApoClassDomain> GetAll(IApoDepartmentResourceParameter resourceParameters);
        IApoClassDomain GetByName(IApoClassForCreateOrEdit item);
        IQueryable<IApoClassDomain> GetByApoDepartment(int id);
    }

    public interface IApoClassService:IApoBaseService<IApoClassDataTranferObject,IApoClassForCreateOrEdit>
    {
        PagedList<IApoClassDataTranferObject> GetAll(IApoClassResourceParameter apoGroupResourceParameter);
        IEnumerable<IApoClassDataTranferObject> GetApoClassByApoDepartment(int id);
    }

    public interface IApoClassForCreateOrEdit: IApoBaseForCreateOrEdit
    {
        int ApoDepartmentId { get; set; }
    }

    public interface IApoClassDataTranferObject : IApoBase
    {
        int DepartmentId { get; set; }
        string DepartmentName { get; set; }
    }
}
