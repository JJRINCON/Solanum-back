
namespace Solanum_back.Models
{
    public class EditIncomeInfo
    {
        public int incomeId { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public int value { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
    }
}