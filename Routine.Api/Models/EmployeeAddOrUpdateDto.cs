﻿using Routine.Api.Entities;
using Routine.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    //因为该项目的 EmployeeAddDto 与 EmployeeUpdateDto 高度一致
    //因此使用这个抽象类，可以减少重复代码

    /// <summary>
    /// Create 或 Update Employee 时的 Dto 抽象类，不能直接使用
    /// </summary>
    [EmployeeNoMustDifferentFromFirstNameAttribute(ErrorMessage = "员工号必须与名不同")] //作用于类
    public abstract class EmployeeAddOrUpdateDto : IValidatableObject
    {
        [Display(Name = "员工号")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "{0}的长度是{1}")]
        public string EmployeeNo { get; set; }

        [Display(Name = "名")]
        [Required(ErrorMessage = "{0}是必填项")]
        [MaxLength(50, ErrorMessage = "{0}的长度不能超过{1}")]
        public string FirstName { get; set; }

        [Display(Name = "姓"), Required(ErrorMessage = "{0}是必填项"), MaxLength(50, ErrorMessage = "{0}的长度不能超过{1}")]
        public string LastName { get; set; }

        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        [Display(Name = "出生日期")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 通过实现 IValidatableObject 接口，自定义验证规则（视频P27）
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //FirstName 与 FirstName 不能相同
            if (FirstName == LastName)
            {
                //错误信息与引起错误的位置
                yield return new ValidationResult("姓和名不能一样", new[] { nameof(LastName), nameof(FirstName) });
            }
            if (!(Gender.ToString().Equals("男")  || Gender.ToString().Equals("女"))) {
                //错误信息与引起错误的位置
                yield return new ValidationResult("性别格式不正确", new[] { nameof(Gender)});
            }
            if (DateOfBirth.Year > 2080 || DateOfBirth.Year < 1800)
            {
                //错误信息与引起错误的位置
                yield return new ValidationResult("年龄超出范围", new[] { nameof(DateOfBirth) });

            }
        }
    }
}
