using InputOutput.Core.Models;
using System.Collections;
using System.Collections.Generic;

namespace InputOutput.Core.Repository
{
    public class SalesRepository : IReadOnlyList<Sale>
    {
        private readonly IList<Sale> _sales;
        private IList<Customer> _customers = new List<Customer>();
        private IList<Salesman> _salesman = new List<Salesman>();

        public SalesRepository(IEnumerable<IModel> salesInfo)
        {
            _sales = new List<Sale>();
            OrganizeSales(salesInfo);
        }

        private void OrganizeSales(IEnumerable<IModel> salesInfo)
        {                  
            foreach (var item in salesInfo)
            {
                switch (item)
                {
                    case Salesman:
                        _salesman.Add((Salesman)item);
                        break;
                    case Customer:
                        _customers.Add((Customer)item);
                        break;
                    case Sale:
                        _sales.Add((Sale)item);
                        break;
                    default:
                        break;
                }
            }
        }

        public Sale this[int index] => _sales[index];

        public int Count => _sales.Count;

        public IEnumerator<Sale> GetEnumerator() => _sales.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerable<Sale> GetSales() => _sales;
        public IEnumerable<Customer> GetCustomers() => _customers;
        public IEnumerable<Salesman> GetSalesman() => _salesman;
    }
}
