using System;
using System.Collections.Generic;

#nullable disable

namespace PoliceWebApplication
{
    public partial class Person
    {
        public Person()
        {
            CasesPeople = new HashSet<CasePerson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TypeId { get; set; }

        public virtual Type Type { get; set; }
        public virtual ICollection<CasePerson> CasesPeople { get; set; }
    }
}
