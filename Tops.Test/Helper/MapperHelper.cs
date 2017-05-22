using AutoMapper;
using Tops.Test.UnitTest;
using TopsInterface.Entities;
using TopsShareClass.Models.DataTranferObjects;
using TopsShareClass.Models.Domain;

namespace Tops.Test.Helper
{
    public class MapperHelper
    {
        public static void SetUpMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductServiceTest.ProductDomain,ProductServiceTest.ProductDto>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.ProductDomain,ProductForCreateOrEdit>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.AttributeTypeDomain, ProductServiceTest.AttributeTypeDto>().ReverseMap();
                cfg.CreateMap<ProductServiceTest.AttributeValueDomain, ProductServiceTest.AttributeValueDto>().ReverseMap();


                cfg.CreateMap<ApoDivisionDomain, ApoDivisionDto>().ReverseMap();
                cfg.CreateMap<ApoDivisionDomain, IApoDivisionDataTranferObject>().ReverseMap();
                cfg.CreateMap<IApoDivisionForCreateOrEdit, ApoDivisionDto>().ReverseMap();
                cfg.CreateMap<IApoDivisionForCreateOrEdit, ApoDivisionDomain>().ReverseMap();


                cfg.CreateMap<ApoGroupDomain, ApoGroupDto>().ReverseMap();
                cfg.CreateMap<IApoGroupForCreateOrEdit, ApoGroupDto>()
                .ForMember(dest => dest.DivisionId,opt => opt.MapFrom(src => src.ApoDivisionId))
                .ReverseMap();
                cfg.CreateMap<IApoGroupForCreateOrEdit, ApoGroupDomain>().ReverseMap();
                cfg.CreateMap<IApoGroupDataTranferObject, ApoDivisionDomain>().ReverseMap();



                cfg.CreateMap<ApoDepartmentDomain,ApoDepartmentDto>().ReverseMap();
                cfg.CreateMap<ApoDepartmentDomain,IApoDepartmentDataTranferObject>().ReverseMap();
                cfg.CreateMap<IApoDepartmentForCreateOrEdit,ApoDepartmentDomain>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src =>src.ApoGroupId
                   ))
                    .ForMember(dest => dest.DivisionId, opt => opt.MapFrom(src => src.ApoDivisionId
                    ))
                .ReverseMap();


                cfg.CreateMap<ApoClassDomain, ApoClassDto>()
                    .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.ApoDepartmentId
                    ))
                .ReverseMap();
                cfg.CreateMap<ApoClassDomain, IApoClassDataTranferObject>().ReverseMap();
                cfg.CreateMap<IApoClassForCreateOrEdit, ApoClassDomain>();

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
