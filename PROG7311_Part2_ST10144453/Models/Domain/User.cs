//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_Part2_ST10144453.Models.Domain
{
    //------------------------------....................User Class....................------------------------------//
    public class User
    {
        //oooooooooo............Declarations............oooooooooo//
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("profile_photo")]
        public string? ProfilePhoto { get; set; }
        [Column("account_type")]
        public string AccountType { get; set; }

    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
