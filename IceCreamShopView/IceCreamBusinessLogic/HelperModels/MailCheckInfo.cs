using System;
using System.Collections.Generic;
using IceCreamShopBusinessLogic.Interfaces;

namespace IceCreamShopBusinessLogic.HelperModels
{
    public class MailCheckInfo
    {
        public string PopHost { get; set; }

        public int PopPort { get; set; }

        public IMessageInfoStorage MessageStorage { get; set; }

        public IClientStorage ClientStorage { get; set; }
    }
}
