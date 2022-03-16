using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Routine.Api.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Routine.Api.Models;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using System.Linq;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanyCollectionsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository companyRepository;

        public CompanyCollectionsController(IMapper mapper,ICompanyRepository companyRepository)
        {
          
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.companyRepository = companyRepository?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet("Ids",Name =nameof(GetComapanyCollection))]
        public async Task<IActionResult> GetComapanyCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        IEnumerable<Guid> ids) {

            if (ids == null)
            {
                return BadRequest();
            }

            var entities = await companyRepository.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count()) {
                return NotFound();
            }
            //使用 Company Full Dto
            var dtosToReturn = mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtosToReturn);
        }


        #region HttpPost
        //同时创建父子关系的资源（视频P23）
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyClooection 
            (IEnumerable<CompanyAddDto> companyCollection) { 
            var companyEntities = mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                companyRepository.AddCompany(company);

            }
            var dtosToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var idsString = string.Join(",", dtosToReturn.Select(x => x.Id));
            await  companyRepository.SaveAsync();
            return CreatedAtRoute(nameof(GetComapanyCollection),new { ids=idsString}, dtosToReturn);

        }
        #endregion
    }

}
