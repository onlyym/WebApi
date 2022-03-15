using Routine.Api.Entities;
using System;

namespace Routine.Api.Models
{
    /// <summary>
    /// Create Employee 使用的Dto
    /// </summary>
    public class EmployeeAddDto
    {
       
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
