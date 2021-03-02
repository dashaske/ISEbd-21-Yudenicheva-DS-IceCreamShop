﻿using System;
using IceCreamFileImplement.Models;
using IceCreamShopBusinessLogic.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IceCreamFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;

        private readonly string IngredientFileName = "Ingredient.xml";
        private readonly string IceCreamFileName = "IceCream.xml";
        private readonly string OrderFileName = "Order.xml";

        public List<Ingredient> Ingredients { get; set; }

        public List<IceCream> IceCreams { get; set; }

        public List<Order> Orders { get; set; }

        private FileDataListSingleton()
        {
            Ingredients = LoadIngredients();
            IceCreams = LoadIceCreams();
            Orders = LoadOrders();
        }

        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }

            return instance;
        }

        ~FileDataListSingleton()
        {
            SaveIngredients();
            SaveIceCreams();
            SaveOrders();
        }

        private List<Ingredient> LoadIngredients()
        {
            var list = new List<Ingredient>();

            if (File.Exists(IngredientFileName))
            {
                XDocument xDocument = XDocument.Load(IngredientFileName);

                var xElements = xDocument.Root.Elements("Ingredient").ToList();

                foreach (var ingredient in xElements)
                {
                    list.Add(new Ingredient
                    {
                        Id = Convert.ToInt32(ingredient.Attribute("Id").Value),
                        IngredientName = ingredient.Element("IngredientName").Value
                    });
                }
            }
            return list;
        }

        private List<IceCream> LoadIceCreams()
        {
            var list = new List<IceCream>();

            if (File.Exists(IceCreamFileName))
            {
                XDocument xDocument = XDocument.Load(IceCreamFileName);

                var xElements = xDocument.Root.Elements("IceCream").ToList();

                foreach (var icecream in xElements)
                {
                    var icecreamIngresients = new Dictionary<int, int>();

                    foreach (var ingredient in icecream.Element("IceCreamIngredients").Elements("IceCreamIngredients").ToList())
                    {
                        icecreamIngresients.Add(Convert.ToInt32(ingredient.Element("Key").Value), Convert.ToInt32(ingredient.Element("Value").Value));
                    }

                    list.Add(new IceCream
                    {
                        Id = Convert.ToInt32(icecream.Attribute("Id").Value),
                        IceCreamName = icecream.Element("IceCreamName").Value,
                        Price = Convert.ToDecimal(icecream.Element("Price").Value),
                        IceCreamIngredients = icecreamIngresients
                    });
                }
            }
            return list;
        }

        private List<Order> LoadOrders()
        {
            var list = new List<Order>();

            if (File.Exists(OrderFileName))
            {
                XDocument xDocument = XDocument.Load(OrderFileName);

                var xElements = xDocument.Root.Elements("Order").ToList();

                foreach (var order in xElements)
                {
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(order.Attribute("Id").Value),
                        IceCreamId = Convert.ToInt32(order.Element("IceCreamId").Value),
                        Count = Convert.ToInt32(order.Element("Count").Value),
                        Sum = Convert.ToDecimal(order.Element("Sum").Value),
                        Status = (OrderStatus)Convert.ToInt32(order.Element("Status").Value),
                        DateCreate = Convert.ToDateTime(order.Element("DateCreate").Value),
                        DateImplement = !string.IsNullOrEmpty(order.Element("DateImplement").Value) ? Convert.ToDateTime(order.Element("DateImplement").Value) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        private void SaveIngredients()
        {
            if (Ingredients != null)
            {
                var xElement = new XElement("Ingredients");

                foreach (var ingredient in Ingredients)
                {
                    xElement.Add(new XElement("Ingredient",
                        new XAttribute("Id", ingredient.Id),
                        new XElement("IngredientName", ingredient.IngredientName)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(IngredientFileName);
            }
        }

        private void SaveIceCreams()
        {
            if (IceCreams != null)
            {
                var xElement = new XElement("IceCreams");

                foreach (var icecream in IceCreams)
                {
                    var ingredientsElement = new XElement("IceCreamIngredients");

                    foreach (var ingredient in icecream.IceCreamIngredients)
                    {
                        ingredientsElement.Add(new XElement("IceCreamIngredient",
                            new XElement("Key", ingredient.Key),
                            new XElement("Value", ingredient.Value)));
                    }

                    xElement.Add(new XElement("IceCream",
                        new XAttribute("Id", icecream.Id),
                        new XElement("IceCreamName", icecream.IceCreamName),
                        new XElement("Price", icecream.Price),
                        ingredientsElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(IceCreamFileName);
            }
        }

        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");

                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                        new XAttribute("Id", order.Id),
                        new XElement("IceCreamId", order.IceCreamId),
                        new XElement("Count", order.Count),
                        new XElement("Sum", order.Sum),
                        new XElement("Status", (int)order.Status),
                        new XElement("DateCreate", order.DateCreate.ToString()),
                        new XElement("DateImplement", order.DateImplement.ToString())));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }
    }
}
