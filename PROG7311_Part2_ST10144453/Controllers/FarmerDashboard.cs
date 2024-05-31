//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models.Domain;
using PROG7311_Part2_ST10144453.ViewModels;

namespace PROG7311_Part2_ST10144453.Controllers
{
    //------------------------------....................FarmerDashboard Class....................------------------------------//
    public class FarmerDashboard : Controller
    {
        //oooooooooo............Declarations............oooooooooo//
        private readonly Part2DbContext _context;

        //............................................Constructor()............................................//
        /// <summary>
        /// Default constructor for the FarmerDashboard class.
        /// </summary>
        /// <param name="context"></param>
        public FarmerDashboard(Part2DbContext context)
        {
            _context = context;
        }

        //............................................FarmerDash............................................//
        /// <summary>
        /// The FarmerDash action method is used to display the farmer dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> FarmerDash(Guid userId)
        {
            // Fetch the user from the database using the user's ID.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                // Handle the case where the user is not found.
                ViewBag.ErrorMessage = "User not found.";
                return View("Error");
            }

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
                User = user,
                FarmerId = farmer.FarmerId,
                Farmer = farmer,
                Products = products // Set the Products property
            };

            // Pass the view model to the view.
            return View(viewModel);
        }



        //............................................AddProduct............................................//
        /// <summary>
        /// The Add Product action method is used to display the form for adding a new product.
        /// </summary>
        /// <param name="farmerId"></param>
        /// <returns></returns>
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

        //............................................AddProduct............................................//
        /// <summary>
        /// The Add Product action method is used to add a new product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="photos"></param>
        /// <returns></returns>
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

        //............................................EditProduct............................................//
        /// <summary>
        /// The Edit Product action method is used to display the form for editing a product.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FarmerSettings(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserID == userId);

            if (user == null || farmer == null)
            {
                ViewBag.ErrorMessage = "User or Farmer not found.";
                return View("Error");
            }

            var viewModel = new EditFarmerProfileViewModel
            {
                User = user,
                Farmer = farmer
            };

            return View(viewModel);
        }

        //............................................EditProduct............................................//
        /// <summary>
        /// The Edit Product action method is used to display the form for editing a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid productId)
        {
            // Fetch the product from the database using the product's ID.
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                // Handle the case where the product is not found.
                ViewBag.ErrorMessage = "Product not found.";
                return View("Error");
            }

            // Pass the product to the view.
            return View(product);
        }

        //............................................EditProduct............................................//
        /// <summary>
        /// The Edit Product action method is used to edit a product in the database.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ProfilePhoto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FarmerSettings(EditFarmerProfileViewModel viewModel, IFormFile ProfilePhoto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == viewModel.User.UserId);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserID == viewModel.User.UserId);

            if (ProfilePhoto != null && ProfilePhoto.Length > 0)
            {
                byte[] photoBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await ProfilePhoto.CopyToAsync(memoryStream);
                    photoBytes = memoryStream.ToArray();
                }

                string photoBase64 = Convert.ToBase64String(photoBytes);
                viewModel.User.ProfilePhoto = photoBase64;
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                    }
                }
                return View(viewModel);
            }

            

            if (user == null || farmer == null)
            {
                ViewBag.ErrorMessage = "User or Farmer not found.";
                return View("Error");
            }

            try
            {
                user.Name = viewModel.User.Name;
                user.Surname = viewModel.User.Surname;
                user.Email = viewModel.User.Email;
                user.Password = viewModel.User.Password;
                user.AccountType = viewModel.User.AccountType;
                user.ProfilePhoto = viewModel.User.ProfilePhoto;

                farmer.FarmName = viewModel.Farmer.FarmName;
                farmer.Specialty = viewModel.Farmer.Specialty;
                farmer.About = viewModel.Farmer.About;

                _context.Users.Update(user);
                _context.Farmers.Update(farmer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(viewModel);
            }

            return RedirectToAction("FarmerDash", new { userId = user.UserId });
        }

        //............................................DeleteFarmer............................................//
        // This method is used to delete a farmer from the database
        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(Guid userId)
        {
            // Find the user and farmer in the database
            var user = await _context.Users.FindAsync(userId);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserID == userId);

            if (user == null || farmer == null)
            {
                // Handle the case where the user or farmer is not found
                ViewBag.ErrorMessage = "User or Farmer not found.";
                return View("Error");
            }

            // Remove the user and farmer from the database
            _context.Users.Remove(user);
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();

            // Redirect to the Home action
            return RedirectToAction("Index", "Home");
        }

        //............................................DeleteProduct............................................//
        /// <summary>
        /// This method is used to delete a product from the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            // Find the product in the database and include the Farmer
            var product = await _context.Products.Include(p => p.Farmer).FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                // Handle the case where the product is not found
                ViewBag.ErrorMessage = "Product not found.";
                return View("Error");
            }

            // Remove the product from the database
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            // Redirect to the FarmerDash action
            return RedirectToAction("FarmerDash", new { userId = product.Farmer.UserID });
        }
    }

}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
