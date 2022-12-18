namespace Solanum_back.Models {
    public class ExpenseInfo {
        public string Name { get; set; }
        public string type { get; set; }
        public int value { get; set; }
        public string description { get; set; }
        public string stage { get; set; }
        public string paidState { get; set; }
        public int cropID { get; set; }
        public string? workerName { get; set; }
        public DateTime date { get; set; }
        public int jorbulNumber { get; set; }
        public int unitValue { get; set; }
    }
}
