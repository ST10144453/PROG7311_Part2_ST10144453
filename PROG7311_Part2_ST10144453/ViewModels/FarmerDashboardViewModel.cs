using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.ViewModels
{
    public class FarmerDashboardViewModel
    {
        public Guid UserId { get; set; }
        public Guid FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        public List<Product> Products { get; set; } // Add this line
    }

}
