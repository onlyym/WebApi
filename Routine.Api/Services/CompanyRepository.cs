using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        //构造函数注入数据库上下文
        private readonly RoutesDbContext  _context;

        public CompanyRepository(RoutesDbContext context)
        {
            _context = context??throw new ArgumentNullException(nameof(context));
        }

        public  void AddCompany(Company company)
        {
            if (company == null) { 
            throw new ArgumentNullException(nameof(company));
            }
            company.Id= Guid.NewGuid();
            if (company.Employees != null) {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }
          
             _context.Companies.Add(company);
        }

        public async void AddEmployee(Guid companyId, Employee employee)
        {
            if (employee == null) { 
                throw new ArgumentNullException(nameof(employee));
            }

            if (companyId == Guid.Empty) { 
                throw new ArgumentNullException(nameof(companyId));
            }
            employee.CompanyId = companyId;
            await _context.AddAsync(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty) {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
            
        }

        public void DeleteCompany(Company company)
        {
            if (company == null) { 
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null) { 
                throw new ArgumentNullException(nameof(employee));
            }
            _context.Employees.Remove(employee);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            var items = _context.Companies as IQueryable<Company>;

            if (string.IsNullOrWhiteSpace(parameters.CompanyName)&& string.IsNullOrWhiteSpace(parameters.SearchTerm)) {
                return await items.ToListAsync();
            }
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName)) {
                items= items.Where(x => x.Name ==parameters.CompanyName);
            }
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                items= items.Where(x => x.Name.Contains(parameters.SearchTerm)|| x.Introduction.Contains(parameters.SearchTerm));
            }
            //最后才真正执行查询数据库
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<Company> > GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null) { 
                throw new ArgumentNullException(nameof(companyIds));
            }
            return await _context.Companies
                .Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid CompanyId)
        {

            if (CompanyId == Guid.Empty)
            {
                 throw new ArgumentNullException(nameof(CompanyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == CompanyId);
        }

        public async Task<Employee> GetEmployee(Guid companyId, Guid employId)
        {

            if (companyId == Guid.Empty) { 
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employId == Guid.Empty) {
                throw new ArgumentNullException(nameof(employId));
            }
            return await  _context.Employees
                .Where(x => x.Id == employId && x.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,string genderDisPlay,string q)
        {
            if (companyId == Guid.Empty) { 
                throw new ArgumentNullException(nameof(companyId));
            }
            
            if (string.IsNullOrWhiteSpace(genderDisPlay)&&string.IsNullOrWhiteSpace(q))
            {
                return   await _context.Employees
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.EmployeeNo)
                .ToListAsync();
            }
            var items = _context.Employees.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(genderDisPlay))
            {
                 genderDisPlay = genderDisPlay.Trim();
                var gender = Enum.Parse<Gender>(genderDisPlay);
                items = items.Where(x => x.Gender == gender);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                 q = q.Trim();
                items = items.Where(x =>x.EmployeeNo.Contains(q)
                ||x.FirstName.Contains(q)
                ||x.LastName.Contains(q));
            }

            return await items
               .OrderBy(x => x.EmployeeNo)
               .ToListAsync();
           
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdateCompany(Company company)
        {
            //EF自动有跟踪，不用显示指定
        }

        public void UpdateEmployee(Employee employee)
        {
           //EF自动有跟踪，不用显示指定
        }
    }
}
