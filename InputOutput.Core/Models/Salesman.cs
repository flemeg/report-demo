using System.Diagnostics;

namespace InputOutput.Core.Models
{
    [DebuggerDisplay("{Name}")]
    public class Salesman : BaseModel
    {      
        public string Cpf { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }
}
