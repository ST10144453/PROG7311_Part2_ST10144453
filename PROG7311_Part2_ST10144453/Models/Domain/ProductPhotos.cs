//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_Part2_ST10144453.Models.Domain
{
    //------------------------------....................ProductPhotos Class....................------------------------------//
    public class ProductPhoto
    {
        //oooooooooo............Declarations............oooooooooo//
        [Column("product_photo_id")]
        public Guid ProductPhotoId { get; set; }
        [Column("photo")]
        public string Photo { get; set; }
        [Column("product_id")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
