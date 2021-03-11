using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PoliceWebApplication
{
    public partial class Type
    {
        public Type()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }
        [Display(Name = "Роль")]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
