using System;
using System.Collections.Generic;
using System.Text;
using IceCreamShopBusinessLogic.ViewModels;

namespace IceCreamShopBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<IceCreamViewModel> IceCreams { get; set; }
    }
}
