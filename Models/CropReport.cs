namespace Solanum_back.Models {
    public class CropReport {
        public string cropName { get; set; }
        public int totalIncomes { get; set; }
        public int totalExpenses { get; set; }
        public int totalLaborExpenses { get; set; }
        public int totalSuppliesExpenses { get; set; }
        public int totalOthersExpenses { get; set; }
        public int profit { get; set; }
    }
}
