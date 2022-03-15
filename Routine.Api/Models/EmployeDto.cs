using Routine.Api.Entities;
using System;

namespace Routine.Api.Models
{
    public class EmployeDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string GenderDisPlay { get; set; }
        public int age { get; set; }
    }
}
