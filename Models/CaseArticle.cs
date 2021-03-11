using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace PoliceWebApplication
{
    public partial class CaseArticle
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        [Display(Name = "№")]
        public int ArticleId { get; set; }
        [Display(Name = "Опис")]
        public virtual CriminalArticle Article { get; set; }
        public virtual Case Case { get; set; }
    }
}
