using System;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.HelperModels;
using IceCreamShopBusinessLogic.Interfaces;
using IceCreamShopBusinessLogic.ViewModel;
using IceCreamShopBusinessLogic.Enums;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IceCreamShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IIngredientStorage _ingredientStorage;

        private readonly IIceCreamStorage _icecreamStorage;

        private readonly IOrderStorage _orderStorage;

        public ReportLogic(IIceCreamStorage icecreamStorage, IIngredientStorage
        ingredientStorage, IOrderStorage orderStorage)
        {
            _icecreamStorage = icecreamStorage;
            _ingredientStorage = ingredientStorage;
            _orderStorage = orderStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>
        public List<ReportIngredientIceCreamViewModel> GetIngredientIceCream()
        {
            var ingredients = _ingredientStorage.GetFullList();
            var icecreams = _icecreamStorage.GetFullList();
            var list = new List<ReportIngredientIceCreamViewModel>();
            foreach (var icecream in icecreams)
            {
                var record = new ReportIngredientIceCreamViewModel
                {
                    IceCreamName = icecream.IceCreamName,
                    Ingredients = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var ingredient in ingredients)
                {
                    if (icecream.IceCreamIngredients.ContainsKey(ingredient.Id))
                    {
                        record.Ingredients.Add(new Tuple<string, int>(ingredient.IngredientName,
                        icecream.IceCreamIngredients[ingredient.Id].Item2));
                        record.TotalCount += icecream.IceCreamIngredients[ingredient.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }
        /// <summary>
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                IceCreamName = x.IceCreamName,
                Count = x.Count,
                Sum = x.Sum,
                Status = ((OrderStatus)Enum.Parse(typeof(OrderStatus), x.Status.ToString())).ToString()
            })
            .ToList();
        }
        /// <summary>
        /// Сохранение изделия в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveIceCreamsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список мороженого",
                IceCreams = _icecreamStorage.GetFullList()
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveIngredientIceCreamToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список мороженого",
                IngredientIceCreams = GetIngredientIceCream()
            });
        }
        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }
}
