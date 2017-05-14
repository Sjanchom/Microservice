using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using AutoMapper;
using Tops.Test.Helper;
using TopsInterface;
using TopsInterface.Core;
using TopsInterface.Entities;
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
            Assert.True(sut.Count() <= 5);
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignInCorrectPage()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(10, 5, null);

           
            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.Count,0);
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnCorrectValueWhenAssignExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(1, 5, "Food");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.True(sut.All(x => x.Code.Contains("Food") || x.Name.Contains("Food")));
            Assert.Equal(sut.Count, _apoDivision.Count(x => x.Code.ToLowerInvariant().Contains("Food".ToLowerInvariant()) 
            || x.Name.ToLowerInvariant().Contains("Food".ToLowerInvariant())));
        }

        [Fact]
        public void ApoDivisionServiceShouldReturnNullWhenAssignNoExistNameOrCodeInDatabase()
        {
            var service = new ApoDivisionService(_apoDivisionRepository);

            var sut = service.GetAll(1, 5, "ddsifjdigjs");


            Assert.IsType<PagedList<IApoDivisionDataTranferObject>>(sut);
            Assert.Equal(sut.Count, 0);
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


    }

    public class ApoDivisionForCreateOrEdit: IApoDivisionForCreateOrEdit
    {
        public string Name { get; set; }
    }

    public interface IApoDivisionForCreateOrEdit
    {
         string Name { get; set; }
    }

    public interface IApoDivisionDataTranferObject : IApoDivisionDomain
    {
    }

    public interface IApoBaseRepository<T>
    {
        IEnumerable<T> GetAll(IBaseResourceParameter resourceParameter);
        T GetById(int id);
        T Add(T entity);
        T Update(int id, T entity);
        bool Edit(T item);
        bool Delete(int id);
    }

    public interface IApoDivisionRepository : IApoBaseRepository<IApoDivisionDomain>
    {
    }


    public class ApoDivisionService:IApoBaseService<IApoDivisionDataTranferObject,IApoDivisionForCreateOrEdit>
    {
        private IApoDivisionRepository _apoDivisionRepository;


        public ApoDivisionService(IApoDivisionRepository apoDivisionRepository)
        {
            _apoDivisionRepository = apoDivisionRepository;
        }

        public PagedList<IApoDivisionDataTranferObject> GetAll(int page,int pageSize,string searchText)
        {
            var apoDivisionFromRepository =
                _apoDivisionRepository.GetAll(new ResourceParamater(page, pageSize, searchText));

            return PagedList<IApoDivisionDataTranferObject>.Create(Mapper.Map<List<ApoDivisionDto>>(apoDivisionFromRepository).AsQueryable(),page,pageSize);
        }

        public IApoDivisionDataTranferObject GetById(int id)
        {
            var apoDivisionFromRepository = _apoDivisionRepository.GetById(id);

            if (apoDivisionFromRepository == null)
            {
                return null;
            }

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);
        }

        public IApoDivisionDataTranferObject Create(IApoDivisionForCreateOrEdit item)
        {
            var mapToDomain = Mapper.Map<ApoDivisionDto>(item);
            var apoDivisionFromRepository = _apoDivisionRepository.Add(mapToDomain);

            return Mapper.Map<ApoDivisionDto>(apoDivisionFromRepository);
        }

        public IApoDivisionDataTranferObject Edit(IApoDivisionForCreateOrEdit item)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ApoDivisionDto: IApoDivisionDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ResourceParamater : IBaseResourceParameter
    {
        public ResourceParamater(int page, int pageSize, string searchText)
        {
            Page = page;
            PageSize = pageSize;
            SearchText = searchText;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
    }

    public class ApoDivisionDomain : IApoDivisionDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
