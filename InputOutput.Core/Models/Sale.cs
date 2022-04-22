using System.Collections.Generic;
using System.Diagnostics;

namespace InputOutput.Core.Models
{
    [DebuggerDisplay("{Id}")]
    public class Sale : BaseModel
    {        
        public double TotalValue { get; set; }
        public string Salesman { get; set; }
        public IEnumerable<Item> Items { get; set; }

    }
}
