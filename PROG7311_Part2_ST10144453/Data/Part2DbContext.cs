//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using PROG7311_Part2_ST10144453.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace PROG7311_Part2_ST10144453.Data
{
    //------------------------------....................Part2DbContext Class....................------------------------------//
    public class Part2DbContext : DbContext
    {
        //oooooooooo............Declarations............oooooooooo//
        public DbSet<User> Users { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }

        //............................................Constructor()............................................//
        public Part2DbContext(DbContextOptions<Part2DbContext> options) : base(options)
        {
            Users = Set<User>();
            Farmers = Set<Farmer>();
            Staffs = Set<Staff>();
            Products = Set<Product>();
            ProductPhotos = Set<ProductPhoto>();
        }
    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
