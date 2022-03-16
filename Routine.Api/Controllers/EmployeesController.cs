using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Routine.Api.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Routine.Api.Models;
using Routine.Api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        public async Task<ActionResult<List<EmployeDto>>> GetEmployees(Guid companyId, [FromQuery] string genderDisPlay, [FromQuery] string q) //也可写成[FromQuery(Name ="gender")] 指定参数名，用于形参和传入的参数名称不一样的情况
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employees = await companyRepository.GetEmployeesAsync(companyId, genderDisPlay, q);
            var employeesDto = mapper.Map<List<EmployeDto>>(employees);
            return Ok(employeesDto);
        }

        [HttpGet("{employeId}", Name = nameof(GetEmployeForCompany))]
        public async Task<ActionResult<EmployeDto>> GetEmployeForCompany(Guid companyId, Guid employeId)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employe = await companyRepository.GetEmployee(companyId, employeId);
            if (employe == null)
            {
                return NotFound();
            }
            var employeDto = mapper.Map<EmployeDto>(employe);
            return Ok(employeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = mapper.Map<Employee>(employee);
            companyRepository.AddEmployee(companyId, entity);
            await companyRepository.SaveAsync();
            var returnDto = mapper.Map<EmployeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployeForCompany), new { companyId = companyId, employeId = returnDto.Id }, returnDto);

        }
        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeDto>> UpdateEmployeeFroCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto employee)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await companyRepository.GetEmployee(companyId, employeeId);
            if (employeeEntity == null)
            {
                //不允许客户端生成 Guid
                //return NotFound();
                var employeeToAddEntity = mapper.Map<Employee>(employee);
                employeeToAddEntity.Id = employeeId;
                companyRepository.AddEmployee(companyId, employeeToAddEntity);
                await companyRepository.SaveAsync();
                var returnDto = mapper.Map<EmployeDto>(employeeToAddEntity);
                return CreatedAtRoute(nameof(GetEmployeForCompany), new { companyId = companyId, employeId = returnDto.Id }, returnDto);
            }
            mapper.Map(employee, employeeEntity);
            companyRepository.UpdateEmployee(employeeEntity);
            await companyRepository.SaveAsync();
            return NoContent();


        }

        #region HttpPatch
        /*
      * HTTP PATCH 举例（视频P32）
      * 原资源：
      *      {
      *        "baz":"qux",
      *        "foo":"bar"
      *      }
      * 
      * 请求的 Body:
      *      [
      *        {"op":"replace","path":"/baz","value":"boo"},
      *        {"op":"add","path":"/hello","value":["world"]},
      *        {"op":"remove","path":"/foo"}
      *      ]
      *      
      * 修改后的资源：
      *      {
      *        "baz":"boo",
      *        "hello":["world"]
      *      }
      *      
      * JSON PATCH Operations:
      * Add:
      *   {"op":"add","path":"/biscuits/1","value":{"name","Ginger Nut"}}
      * Replace:
      *   {"op":"replace","path":"/biscuits/0/name","value":"Chocolate Digestive"}
      * Remove:
      *   {"op":"remove","path":"/biscuits"}
      *   {"op":"remove","path":"/biscuits/0"}
      * Copy:
      *   {"op":"copy","from":"/biscuits/0","path":"/best_biscuit"}
      * Move:
      *   {"op":"move","from":"/biscuits","path":"/cookies"}
      * Test:
      *   {"op":"test","path":"/best_biscuit","value":"Choco Leibniz}
      */
        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeFroCompany(
            Guid companyId,
            Guid employeeId,
            JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (!await companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await companyRepository.GetEmployee(companyId, employeeId);
            if (employeeEntity == null)
            {
                var employeeDto = new EmployeeUpdateDto();
                patchDocument.ApplyTo(employeeDto, ModelState);
                if (!TryValidateModel(employeeDto))
                {
                    return ValidationProblem(ModelState);
                }

                var employeeToAdd = mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;
                companyRepository.AddEmployee(companyId, employeeToAdd);
                await companyRepository.SaveAsync();

                var returnDto = mapper.Map<EmployeDto>(employeeToAdd);
                return CreatedAtRoute(nameof(GetEmployeForCompany), new { companyId = companyId, employeId = employeeId }, returnDto);
            }
            var dtoTopatch = mapper.Map<EmployeeUpdateDto>(employeeEntity);

            //需要处理验证
            patchDocument.ApplyTo(dtoTopatch,ModelState);

            if (!TryValidateModel(dtoTopatch)) {

                return ValidationProblem(ModelState);
            }
            mapper.Map(dtoTopatch, employeeEntity);
            await companyRepository.SaveAsync();

            return NoContent();

        }
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                                        .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
        #endregion HttpPatch
    }
}
