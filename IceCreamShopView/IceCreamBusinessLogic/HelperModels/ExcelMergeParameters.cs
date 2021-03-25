using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;

namespace IceCreamShopBusinessLogic.HelperModels
{
    class ExcelMergeParameters
    {
        public Worksheet Worksheet { get; set; }

        public string CellFromName { get; set; }

        public string CellToName { get; set; }

        public string Merge => $"{CellFromName}:{CellToName}";
    }
}
