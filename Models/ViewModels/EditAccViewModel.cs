using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PoliceWebApplication.Models.ViewModels
{
    public class EditAccViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Електронна пошта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Код запрошення")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Пароль повинен мати мінімум {2} и максимум {1} символів.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}