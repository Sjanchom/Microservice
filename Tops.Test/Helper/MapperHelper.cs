using AutoMapper;
using Tops.Test.UnitTest;

namespace Tops.Test.Helper
{
    class MapperHelper
    {
        public static void SetUpMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductServiceTest.ProductDomain,ProductServiceTest.ProductDto>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.ProductDomain,ProductForEdit>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.AttributeTypeDomain, ProductServiceTest.AttributeTypeDto>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.AttributeValueDomain, ProductServiceTest.AttributeValueDto>().ReverseMap();


                cfg.CreateMap<ApoDivisionDomain, ApoDivisionDto>().ReverseMap();
                cfg.CreateMap<IApoDivisionForCreateOrEdit, ApoDivisionDto>().ReverseMap();
                cfg.CreateMap<IApoDivisionForCreateOrEdit, ApoDivisionDomain>().ReverseMap();

                //cfg.CreateMap<Entities.Author, Models.AuthorDto>()
                //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                //    $"{src.FirstName} {src.LastName}"))
                //    .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                //    src.DateOfBirth.GetCurrentAge(src.DateOfDeath)));
                //cfg.CreateMap<Student, StudentDTO>()
                //    .ForMember(desc => desc.BirthDate,
                //        opt => opt.MapFrom(src => src.BirthDate.Value.ToString("dd/MM/yyy")))
                //    .ReverseMap()
                //    .ForMember(desc => desc.BirthDate, opt => opt.MapFrom(src => UtilHelper.PareDateTime(src.BirthDate)));
                //cfg.CreateMap<Student, StudentForUpdateDTO>();
                //cfg.CreateMap<Student, StudentForCreateDTO>();
                //cfg.CreateMap<StudentForManipulationDTO, Student>()
                //    .ForMember(desc => desc.BirthDate,
                //        opt => opt.MapFrom(src => UtilHelper.PareDateTime(src.BirthDate)));
                //cfg.CreateMap<StudentForCreateDTO, Student>()
                //    .ForMember(desc => desc.BirthDate, 
                //    opt => opt.MapFrom(src =>UtilHelper.PareDateTime(src.BirthDate) ));
                //cfg.CreateMap<StudentForUpdateDTO, Student>()
                //      .ForMember(desc => desc.BirthDate,
                //        opt => opt.MapFrom(src => UtilHelper.PareDateTime(src.BirthDate)));
                //cfg.CreateMap<Entities.Book, Models.BookDto>();

                //cfg.CreateMap<Models.AuthorForCreationDto, Entities.Author>();

                //cfg.CreateMap<Models.AuthorForCreationWithDateOfDeathDto, Entities.Author>();

                //cfg.CreateMap<Models.BookForCreationDto, Entities.Book>();

                //cfg.CreateMap<Models.BookForUpdateDto, Entities.Book>();

                //cfg.CreateMap<Entities.Book, Models.BookForUpdateDto>();
            });

        }
    }
}
