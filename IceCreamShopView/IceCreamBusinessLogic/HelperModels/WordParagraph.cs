using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamShopBusinessLogic.HelperModels
{
    class WordParagraph
    {
        public List<(string, WordParagraphProperties)> Texts { get; set; }

        public WordParagraphProperties TextProperties { get; set; }
    }
}
