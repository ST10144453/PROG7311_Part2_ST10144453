using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models.Domain;
using PROG7311_Part2_ST10144453.ViewModels;

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
            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.UserID == userId);

            if (farmer == null)
            {
                // Handle the case where the farmer is not found.
                ViewBag.ErrorMessage = "Farmer not found.";
                return View("Error");
            }

            // Fetch the products associated with the farmer
            var products = await _context.Products
                .Where(p => p.FarmerId == farmer.FarmerId)
                .ToListAsync();

            // Create a view model to pass both UserId and FarmerId to the view.
            var viewModel = new FarmerDashboardViewModel
            {
                UserId = userId,
                FarmerId = farmer.FarmerId,
                Farmer = farmer,
                Products = products // Set the Products property
            };

            // Pass the view model to the view.
            return View(viewModel);
        }




        [HttpGet]
        public IActionResult AddProduct(Guid farmerId)
        {
            // Create a new Product and set the FarmerId
            var product = new Product
            {
                FarmerId = farmerId
            };

            // Pass the product to the view
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, List<IFormFile> photos)
        {
            // Ensure the FarmerId is valid
            if (product.FarmerId == Guid.Empty)
            {
                Console.WriteLine("Invalid Farmer ID.");
                return View(product);
            }

            // Verify if the farmer exists
            var farmer = await _context.Farmers.FindAsync(product.FarmerId);
            if (farmer == null)
            {
                Console.WriteLine("The specified farmer doesn't exist.");
                return View(product);
            }

            try
            {
                int photoCount = 1; // Counter for tracking the photo number
                foreach (var photo in photos)
                {
                    if (photo.Length > 0)
                    {
                        byte[] photoBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            await photo.CopyToAsync(memoryStream);
                            photoBytes = memoryStream.ToArray();
                        }

                        // Convert the byte array to a Base64 string
                        string photoBase64 = Convert.ToBase64String(photoBytes);

                        // Determine the photo property to set based on the photoCount
                        var photoProperty = typeof(Product).GetProperty($"photo{photoCount}");
                        if (photoProperty != null)
                        {
                            photoProperty.SetValue(product, photoBase64);
                            photoCount++;
                        }
                        else
                        {
                            Console.WriteLine($"Maximum number of photos reached. Ignoring additional photos.");
                            break;
                        }
                    }
                }

                // Add the product to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Redirect to the FarmerDash action with the userId
                return RedirectToAction("FarmerDash", new { userId = farmer.UserID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(product);
            }
        }

        [HttpGet]
        public IActionResult FarmerSettings()
        {
            return View();
        }


    }

}
