using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;

namespace Routine.Api.Controllers


/*
* 绑定数据来源：
* [FromBody] 请求的body
* [FromForm]  请求body中的form数据
* [FromHeader] 请求Header
* [FromQuery] Query string 参数
* [FromRoute] 当前请求中的路由数据
* [FromService] 作为Action参数而注入的服务
*/ 


{
    // 可选继承Controller 或者ControllerBase    后者更适用于Webapi  前者有包含视图相关东西
    [ApiController]
    [Route("api/companies")]
    // [Route("api/[controller]")]  相当于取类名去掉controller    即 api/companies
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompaniesController(ICompanyRepository companyRepository,IMapper mapper)
        {
            this.companyRepository = companyRepository ?? 
                throw new ArgumentNullException(nameof(companyRepository)) ;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        [HttpHead] //添加对 Http Head 请求的支持，Http Head 请求只获取 Header 信息，没有 Body（视频P16）
        public async Task<ActionResult<List<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters) { 

            var companies = await companyRepository.GetCompaniesAsync(parameters);


            //entity model 与dto转换  手动映射

            //var companiesDto = new List<CompanyDto>();
            //foreach (var company in companies) {
            //    companiesDto.Add(new CompanyDto
            //    {
            //        Id = company.Id,
            //        Name = company.Name

            //    });
            //}

            //自动映射
            var companiesDto = mapper.Map<List<CompanyDto>>(companies);
            return Ok(companiesDto);

        }
        [HttpGet("{companyId}",Name =nameof(GetCompanyById))]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyId)
        {
            
            var company = await companyRepository.GetCompanyAsync(companyId);
            if (company == null) { 
                return NotFound();  
            }
            var companyDto = mapper.Map<CompanyDto>(company);
            return Ok(companyDto);

        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company) {
           
            //测试一下git
            var entity = mapper.Map<Company>(company);
             companyRepository.AddCompany(entity);
            await companyRepository.SaveAsync();
            var returnDto = mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompanyById), new { companyId = returnDto.Id }, returnDto);
        }
       
        [HttpOptions]
        public  IActionResult GetCompaniesOptions()
        {

            Response.Headers.Add("Allow", "DELETE,GET,PATCH,PUT,OPTIONS");
            return Ok();

        }

    }
}
