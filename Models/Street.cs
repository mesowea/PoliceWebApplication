using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
