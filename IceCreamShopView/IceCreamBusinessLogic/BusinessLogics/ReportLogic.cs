using System;
using IceCreamShopBusinessLogic.BindingModel;
using IceCreamShopBusinessLogic.BindingModels;
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

        private readonly IIceCreamStorage _icecreamStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly IWareHouseStorage _wareHouseStorage;

        public ReportLogic(IIceCreamStorage icecreamStorage, IWareHouseStorage
        wareHouseStorage, IOrderStorage orderStorage)
        {
            _icecreamStorage = icecreamStorage;
            _orderStorage = orderStorage;
            _wareHouseStorage = wareHouseStorage;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>
        public List<ReportIngredientIceCreamViewModel> GetIngredientIceCream()
        {
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
                foreach (var ingredient in icecream.IceCreamIngredients)
                {
                    record.Ingredients.Add(new Tuple<string, int>(ingredient.Value.Item1, ingredient.Value.Item2));
                    record.TotalCount += ingredient.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }
        public List<ReportWareHouseIngredientsViewModel> GetWareHouseIngredient()
        {
            var storeHouses = _wareHouseStorage.GetFullList();

            var list = new List<ReportWareHouseIngredientsViewModel>();

            foreach (var storeHouse in storeHouses)
            {
                var record = new ReportWareHouseIngredientsViewModel
                {
                    WareHouseName = storeHouse.WareHouseName,
                    Ingredients = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var ingredient in storeHouse.WareHouseIngredients)
                {
                    record.Ingredients.Add(new Tuple<string, int>(ingredient.Value.Item1, ingredient.Value.Item2));
                    record.TotalCount += ingredient.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }
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
        public List<ReportOrderByDateViewModel> GetOrdersInfo()
        {
            return _orderStorage.GetFullList()
                .GroupBy(order => order.DateCreate
                .ToShortDateString())
                .Select(rec => new ReportOrderByDateViewModel
                {
                    Date = Convert.ToDateTime(rec.Key),
                    Count = rec.Count(),
                    Sum = rec.Sum(order => order.Sum)
                })
                .ToList();
        }
        public List<ReportOrdersViewModel> GetOrdersGroupByDate()
        {
            return _orderStorage.GetFullList().GroupBy(x => x.DateCreate.Date)
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.Key,
                Count = x.Count(),
                Sum = x.Sum(rec => rec.Sum),
            }).ToList();
        }
        public void SaveIceCreamsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список мороженого",
                IceCreams = _icecreamStorage.GetFullList()
            });
        }
        public void SaveIngredientIceCreamToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список мороженого",
                IngredientIceCreams = GetIngredientIceCream()
            });
        }
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
        public void SaveWareHouseIngredientsToExcel(ReportBindingModel model)
        {
            SaveToExcel.CreateDocForWareHouse(new ExcelInfoForWareHouse
            {
                FileName = model.FileName,
                Title = "Список складов",
                WareHouseIngredients = GetWareHouseIngredient()
            });
        }

        public void SaveOrdersInfoToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDocForWareHouse(new PdfInfoForOrder
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrdersInfo()
            });
        }

        public void SaveWareHousesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDocForWareHouse(new WordInfoForWareHouse
            {
                FileName = model.FileName,
                Title = "Список складов",
                WareHouses = _wareHouseStorage.GetFullList()
            });
        }
    }
}
