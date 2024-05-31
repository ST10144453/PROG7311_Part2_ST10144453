//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models;
using PROG7311_Part2_ST10144453.Models.Domain;
using PROG7311_Part2_ST10144453.ViewModels;

namespace PROG7311_Part2_ST10144453.Controllers
{
    //------------------------------....................Part2DbContext Class....................------------------------------//
    public class StaffDashboard : Controller
    {
        //oooooooooo............Declarations............oooooooooo//
        private readonly Part2DbContext _context;

        //............................................Constructor()............................................//
        /// <summary>
        /// The constructor for the StaffDashboard class.
        /// </summary>
        /// <param name="context"></param>
        public StaffDashboard(Part2DbContext context)
        {
            _context = context;
        }

        //............................................StaffDash()............................................//
        /// <summary>
        /// The StaffDash action method for the StaffDashboard class.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        //............................................DeleteFarmer()............................................//
        /// <summary>
        /// The DeleteFarmer action method for the StaffDashboard class.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        //............................................DeleteProduct()............................................//
        /// <summary>
        /// The DeleteProduct action method for the StaffDashboard class.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
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

        //............................................AddFarmer()............................................//
        /// <summary>
        /// The AddFarmer action method for the StaffDashboard class.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

    }
}
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
