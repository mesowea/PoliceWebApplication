using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PoliceWebApplication
{
    public partial class Investigator
    {
        public Investigator()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Введіть, будь ласка, ім'я слідчого")]
        public string Name { get; set; }
        [Display(Name = "Дата народження")]
        [Required(ErrorMessage = "Введіть, будь ласка, дату народження слідчого")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Характеристика")]
        public string Characteristic { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
    }
}
