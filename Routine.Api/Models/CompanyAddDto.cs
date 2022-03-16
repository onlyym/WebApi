using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Display(Name ="公司名")]
        [Required(ErrorMessage ="{0}必填的")]
        [MaxLength(100,ErrorMessage ="{0}的最大长度不可以超过{1}")]
        public string Name { get; set; }  //请注意，此处的属性名为 Name ，与视频中的 CompanyName 不同
        [Display(Name = "简介")]
        [StringLength(500,MinimumLength = 10,ErrorMessage = "{0}的范围是从{2}到{1}")]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>(); //这种写法可以避免空引用异常
    }
}
