namespace InputOutput.Core.Models
{
    public class Customer: BaseModel
    {      
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string BusinessArea { get; set; }
    }
}
