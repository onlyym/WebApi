using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Routine.Api.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Routine.Api.Models;
using Routine.Api.Entities;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;
        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        [HttpGet]
        public async Task<ActionResult<List<EmployeDto>>> GetEmployees(Guid companyId,[FromQuery]string genderDisPlay,[FromQuery]string q) //也可写成[FromQuery(Name ="gender")] 指定参数名，用于形参和传入的参数名称不一样的情况
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employees = await companyRepository.GetEmployeesAsync(companyId,genderDisPlay,q);
            var employeesDto = mapper.Map<List<EmployeDto>>(employees);
            return Ok(employeesDto);
        }

        [HttpGet("{employeId}",Name =nameof(GetEmployeForCompany))]
        public async Task<ActionResult<EmployeDto>> GetEmployeForCompany(Guid companyId,Guid employeId)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employe = await companyRepository.GetEmployee(companyId,employeId);
            if(employe == null)
            {
                return NotFound();
            }
            var employeDto = mapper.Map<EmployeDto>(employe);
            return Ok(employeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeDto>> CreateEmployeeForCompany(Guid companyId,EmployeeAddDto employee)
        {
            if(!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = mapper.Map<Employee>(employee);
            companyRepository.AddEmployee(companyId, entity);
            await companyRepository.SaveAsync();
            var returnDto = mapper.Map<EmployeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployeForCompany), new { companyId = companyId, employeId =returnDto.Id}, returnDto);

        }
    }
}
