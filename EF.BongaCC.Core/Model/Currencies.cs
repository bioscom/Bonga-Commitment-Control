using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class Currencies : BaseEntity
    {
        public string Country { get; set; }
        public string CurrencyName { get; set; }
        public string Codes { get; set; }
        public int Number { get; set; }

        public virtual ICollection<ActivityDetails> ActivityDetails { get; set; }

        //public CurrencyX()
        //{

        //}
    }
}
