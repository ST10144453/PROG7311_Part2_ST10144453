//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_Part2_ST10144453.Models.Domain
{
    //------------------------------....................Product Class....................------------------------------//
    public class Product
    {
        //oooooooooo............Declarations............oooooooooo//
        [Column("product_id")]
        public Guid ProductId { get; set; }
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public float Price { get; set; }
        [Column("category")]
        public string Category { get; set; }
        [Column("production_date")]
        public DateTime productionDate { get; set; }
        [Column("farmer_id")]
        public Guid FarmerId { get; set; }
        public Farmer Farmer { get; set; }
    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
