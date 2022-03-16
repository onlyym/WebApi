using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Routine.Api.ValidationAttributes;

namespace Routine.Api.Models
{
    /// <summary>
    /// Create Employee 使用的Dto
    /// </summary>
    [EmployeeNoMustDifferentFromFirstName(ErrorMessage = "员工编号必须和名不一样")]
    public class EmployeeAddDto: EmployeeAddOrUpdateDto
    {

        
    }
}
