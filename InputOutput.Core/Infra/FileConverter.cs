using InputOutput.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace InputOutput.Core.Infra
{
    public class FileConverter
    {
        private const string SalesmanType = "001";
        private const string CustomerType = "002";
        private const string SaleType = "003";

        public static IEnumerable<IModel> RawContentAsSaleModel(string rawContent)
        {
            var models = new List<IModel>();
            var rows = rawContent.Split('\n');
            foreach (var row in rows)
            {
                models.Add(ExtractRowType(row));
            }

            return models;
        }

        private static IModel ExtractRowType(string row)
        {
            string rowType = row.Substring(0, 3);

            switch (rowType)
            {
                case SalesmanType:
                    var salesman = new Salesman
                    {
                        Id = TakeColContent(row, 1),
                        Cpf = TakeColContent(row, 1),                        
                        Name = TakeColContent(row, 2),
                        Salary = double.Parse((TakeColContent(row, 3)), CultureInfo.InvariantCulture)
                    };
                    return salesman;

                case CustomerType:
                    var customer = new Customer
                    {
                        Id = TakeColContent(row, 1),
                        Cnpj = TakeColContent(row, 1),
                        Name = TakeColContent(row, 2),
                        BusinessArea = TakeColContent(row, 3)
                    };
                    return customer;

                case SaleType:
                    var sale = new Sale
                    {
                        Id = TakeColContent(row, 1),
                        Salesman = TakeColContent(row, 3),
                        Items = ExtractItems(TakeColContent(row, 2))
                    };
                    return sale;

                default:
                    return null;
            }
        }

        private static string TakeColContent(string row, short position)
        {
            return row.Split("ç")[position];
        }

        private static IEnumerable<Item> ExtractItems(string rawItems)
        {
            var saleItems = new List<Item>();
            var items = rawItems.Replace("[", "").Replace("]", "").Split(",");
            foreach (var row in items)
            {
                var itemDetail = row.Split("-");
                saleItems.Add(new Item
                {
                    Id = itemDetail[0],
                    Qtd = double.Parse(itemDetail[1], CultureInfo.InvariantCulture),
                    Prc = double.Parse(itemDetail[2], CultureInfo.InvariantCulture)
                });
            }
            return saleItems;
        }
    }
}
