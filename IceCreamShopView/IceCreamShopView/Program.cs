using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IceCreamShopBusinessLogic.BusinessLogics;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamFileImplement.Implements;
using System.Windows.Forms;
using Unity.Lifetime;
using Unity;

namespace IceCreamShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IIngredientStorage, IngredientStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIceCreamStorage, IceCreamStorage>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IngredientLogic>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IceCreamLogic>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
