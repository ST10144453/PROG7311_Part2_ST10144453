//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.ViewModels
{
    //------------------------------....................FarmerDashboardViewModel Class....................------------------------------//
    public class FarmerDashboardViewModel
    {
        //oooooooooo............Declarations............oooooooooo//
        public Guid UserId { get; set; }
        public Guid FarmerId { get; set; }
        public User User { get; set; }
        public Farmer Farmer { get; set; }
        public List<Product> Products { get; set; } // Add this line
    }

}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
