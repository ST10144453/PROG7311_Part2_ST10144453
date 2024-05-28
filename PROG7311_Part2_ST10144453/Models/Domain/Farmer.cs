//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_Part2_ST10144453.Models.Domain
{
    //------------------------------....................Farmer Class....................------------------------------//
    public class Farmer
    {
        //oooooooooo............Declarations............oooooooooo//
        [Column("farmer_id")]
        public Guid FarmerId { get; set; }
        [Column("farm_name")]
        public string FarmName { get; set; }
        [Column("specialty")]
        public string Specialty { get; set; }
        [Column("about")]
        public string About { get; set; }
        [Column("user_id")]
        public Guid UserID { get; set; }
        public User? User { get; set; }

    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
