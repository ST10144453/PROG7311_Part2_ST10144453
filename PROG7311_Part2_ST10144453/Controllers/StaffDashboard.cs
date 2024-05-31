using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models;
using PROG7311_Part2_ST10144453.Models.Domain;
using PROG7311_Part2_ST10144453.ViewModels;

namespace PROG7311_Part2_ST10144453.Controllers
{
    public class StaffDashboard : Controller
    {
        private readonly Part2DbContext _context;

        public StaffDashboard(Part2DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> StaffDash(string email)
        {
            // Fetch the user from the database using the email.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // No user with the provided email exists in the database.
                // You can redirect the user to an error page or show a message.
                Console.WriteLine("No user with the provided email exists.");
            }

            // Fetch the staff associated with the user.
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == user.UserId);

            // Fetch all the products.
            var products = await _context.Products.ToListAsync();

            // Create the ViewModel.
            var viewModel = new StaffDashboardViewModel
            {
                User = user,
                Staff = staff,
                Products = products
            };

            // Pass the ViewModel to the view.
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFarmer(Guid userId)
        {
            // Find the user and farmer in the database
            var user = await _context.Users.FindAsync(userId);
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId);

            if (user == null || staff == null)
            {
                // Handle the case where the user or farmer is not found
                ViewBag.ErrorMessage = "User or Farmer not found.";
                return View("Error");
            }

            // Remove the user and farmer from the database
            _context.Users.Remove(user);
            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            // Redirect to the Home action
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProduct(Guid UserId, Guid productId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);

            if (user == null)
            {
                // No user with the provided UserId exists in the database.
                // You can redirect the user to an error page or show a message.
                Console.WriteLine("No user with the provided UserId exists.");
            }

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
            return RedirectToAction("StaffDash", new { email = user.Email });
        }


        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StaffSettings(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId);

            if (user == null || staff == null)
            {
                // Handle the case where the user or staff is not found
                ViewBag.ErrorMessage = "User or Staff not found.";
                return View("Error");
            }

            var viewModel = new EditStaffProfileViewModel
            {
                User = user,
                Staff = staff
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> StaffSettings(EditStaffProfileViewModel viewModel, IFormFile ProfilePhoto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == viewModel.User.UserId);
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == viewModel.User.UserId);

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



            if (user == null || staff == null)
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

                _context.Users.Update(user);
                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(viewModel);
            }

            return RedirectToAction("StaffDash", new { email = user.Email });
        }

    }
}
