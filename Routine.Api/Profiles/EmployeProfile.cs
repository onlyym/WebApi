using AutoMapper;
using Routine.Api.Entities;
using Routine.Api.Models;
using System;

namespace Routine.Api.Profiles
{
    /// <summary>
    /// AutoMapper 针对 Employee 的映射关系配置文件（视频P12）
    /// </summary>
    public class EmployeProfile : Profile
    {
        public EmployeProfile()
        {
            //创建约束  参数1源类型 参数2 目标类型
            CreateMap<Employee, EmployeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.GenderDisPlay, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.age, opt => opt.MapFrom(src => GetAgeByBirthdate(src.DateOfBirth)));
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<Employee, EmployeeUpdateDto>();

        }

        public static int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
    }
}
