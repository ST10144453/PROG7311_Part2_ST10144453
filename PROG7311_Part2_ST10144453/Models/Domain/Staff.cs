//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_Part2_ST10144453.Models.Domain
{
    //------------------------------....................Staff Class....................------------------------------//
    public class Staff
    {
        //oooooooooo............Declarations............oooooooooo//
        [Column("staff_id")]
        public Guid StaffId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }

        public User User { get; set; }

    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
