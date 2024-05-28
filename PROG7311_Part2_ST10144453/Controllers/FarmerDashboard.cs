using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.Controllers
{
    public class FarmerDashboard : Controller
    {
        private readonly Part2DbContext _context;

        public FarmerDashboard(Part2DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> FarmerDash(Guid userId)
        {
            // Fetch the farmer from the database using the user's ID.
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserID == userId);

            // Pass the farmer to the view.
            return View(farmer);
        }


        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            // Check if the farmer exists in the database.
            var farmer = _context.Farmers.Find(product.FarmerId);

            if (farmer == null)
            {
                // If the farmer doesn't exist, display an error message.
                Console.WriteLine("The specified farmer doesn't exist.");
                return View("AddProduct");
            }

            try
            {
                // Add the product to the database.
                _context.Products.Add(product);
                _context.SaveChanges();

                // Redirect to the AddProductPhoto view.
                return View("AddProductPhoto", new { productId = product.ProductId });
            }
            catch (Exception ex)
            {
                // Handle the exception and display an error message.
                Console.WriteLine(ex.InnerException.Message);
                return View("AddProduct");
            }
        }


        [HttpGet]
        public IActionResult AddProductPhoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductPhoto(Guid productId, List<IFormFile> photos)
        {
            foreach (var photo in photos)
            {
                if (photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await photo.CopyToAsync(memoryStream);

                    // Convert the MemoryStream to a byte array.
                    var photoBytes = memoryStream.ToArray();

                    var productPhoto = new ProductPhoto
                    {
                        ProductPhotoId = Guid.NewGuid(),
                        Photo = Convert.ToBase64String(photoBytes), // Convert byte array to string
                        ProductId = productId
                    };

                    _context.ProductPhotos.Add(productPhoto);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("FarmerDash");
        }

    }
}
