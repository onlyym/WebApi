using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Routine.Api.DtoParameters;

namespace Routine.Api.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters); 
        Task<Company> GetCompanyAsync(Guid CompanyId);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExistsAsync(Guid companyId);
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,string genderDisPlay,string q);
        Task<Employee> GetEmployee(Guid companyId, Guid employId);
        void AddEmployee(Guid companyId,Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<bool> SaveAsync();
    }
}
