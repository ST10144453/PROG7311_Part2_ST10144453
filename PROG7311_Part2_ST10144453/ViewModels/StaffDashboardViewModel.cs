//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.ViewModels
{
    //------------------------------....................StaffDashboardViewModel Class....................------------------------------//
    public class StaffDashboardViewModel
    {
        public User User { get; set; }
        public Staff Staff { get; set; }
        public List<Product> Products { get; set; }
    }

}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
