using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace PoliceWebApplication
{
    public partial class Street
    {
        public Street()
        {
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва вулиці")]
        [Required(ErrorMessage = "Введіть, будь ласка, назву вулиці")]
        public string Name { get; set; }
        [Display(Name ="Місто")]
        public int CityId { get; set; }
        [Display(Name = "Місто")]
        public virtual City City { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
