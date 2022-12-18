namespace Solanum_back.Models {
    public class EditExpenseInfo {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string paidState { get; set; }
        public string workerName { get; set; }
        public int cropID { get; set; }
        public string type { get; set; }
        public DateTime date { get; set; }
        public int value { get; set; }
        public int unitValue { get; set; }
        public int jorbulNumber { get; set; }
    }
}
