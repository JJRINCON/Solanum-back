namespace Solanum_back.Models
{
    public class IncomeInfo
    {
        public string? name { get; set; }
        public DateTime date { get; set; }
        public int value { get; set; }
        public int quantity { get; set; }
        public string? description { get; set; }
        public int cropId { get; set; }
    }
}