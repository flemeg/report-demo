using InputOutput.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace InputOutput.Core.Report
{
    public class ReportBuilder
    {
        private IEnumerable<Sale> _sales;
        private IEnumerable<Customer> _customers;
        private IEnumerable<Salesman> _salesman;

        public ReportBuilder AddSales(IEnumerable<Sale> sales)
        {
            this._sales = sales;
            return this;
        }

        public ReportBuilder AddSalesman(IEnumerable<Salesman> salesmen)
        {
            this._salesman = salesmen;
            return this;
        }

        public ReportBuilder AddCustomers(IEnumerable<Customer> customers)
        {
            this._customers = customers;
            return this;
        }

        public IEnumerable<Report> BuildReport()
        {
            var reports = new List<Report>();

            #region Report 1            
            reports.Add(new Report
            {
                Content = CountUniqueOccurencies(_customers).ToString(),
                Title = "quantidade_de_clientes"
            });
            #endregion

            #region Report 2            
            reports.Add(new Report
            {
                Content = CountUniqueOccurencies(_salesman).ToString(),
                Title = "quantidade_de_vendedores"
            });
            #endregion

            #region Report 3
            var saleWithValue = _sales.GroupBy(s => s.Id)
                .Select(
                    g => new
                    {
                        key = g.Key,
                        Total = g.Sum(i => i.Items.Sum(i => i.Prc * i.Qtd)),
                    }
                );
            if (saleWithValue != null && saleWithValue.Any())
            {
                var saleId = saleWithValue.OrderByDescending(g => g.Total).FirstOrDefault().key;
                reports.Add(new Report
                {
                    Content = saleId.ToString(),
                    Title = "id_venda_mais_cara"
                });
            }           
            #endregion

            #region Report 4
            var saleBySalesman = _sales.GroupBy(s => s.Salesman)
                .Select(
                    g => new
                    {
                        key = g.Key,
                        Total = g.Sum(i => i.Items.Sum(i => i.Prc * i.Qtd)),
                    }
                );
            if (saleBySalesman != null && saleBySalesman.Any())
            {
                var salesmanName = saleBySalesman.OrderBy(g => g.Total).FirstOrDefault().key;
                if (salesmanName != null)
                {
                    reports.Add(new Report
                    {
                        Content = salesmanName,
                        Title = "pior_vendedor"
                    });
                }
            }
            #endregion

            return reports;
        }

        private int CountUniqueOccurencies(IEnumerable<IModel> entries)
        {
            var count = entries.GroupBy(e => e.Id).Count();
            return count;
        }
    }
}
