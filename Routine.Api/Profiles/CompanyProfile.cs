using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
namespace Routine.Api.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            //创建约束  参数1源类型 参数2 目标类型
            CreateMap<Company,CompanyDto>()
                .ForMember(dest=>dest.CompanyName,
                opt=>opt.MapFrom(src=>src.Name));
            CreateMap<CompanyAddDto, Company>();
        }
    }
}
