using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.ViewModels
{
    public class StaffDashboardViewModel
    {
        public User User { get; set; }
        public Staff Staff { get; set; }
        public List<Product> Products { get; set; }
    }

}
