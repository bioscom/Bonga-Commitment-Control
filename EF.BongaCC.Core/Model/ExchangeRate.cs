using System;
using System.Collections.Generic;
using System.Text;

namespace EF.BongaCC.Core.Model
{
    public class ExchangeRate : BaseEntity
    {
        public decimal FloatingExchangeRate { get; set; }
        public int YYear { get; set; }
        public int MMonth { get; set; }
        public int iDay { get; set; }
    }
}
