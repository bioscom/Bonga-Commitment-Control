using EF.BongaCC.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BongaCC.Codes
{
    public class Printing
    {
        public ExcelPackage excel;
        public ExcelWorksheet oWs;
    }

    public enum exportType
    {
        A3 = 3,
        A4 = 4,
    };
}
