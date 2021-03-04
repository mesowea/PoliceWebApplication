using System;
using System.Collections.Generic;

#nullable disable

namespace PoliceWebApplication
{
    public partial class CaseArticle
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int ArticleId { get; set; }

        public virtual CriminalArticle Article { get; set; }
        public virtual Case Case { get; set; }
    }
}
