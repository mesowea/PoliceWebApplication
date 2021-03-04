using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Characteristic { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
    }
}
