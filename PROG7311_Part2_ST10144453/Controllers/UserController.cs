using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PROG7311_Part2_ST10144453.Data;
using PROG7311_Part2_ST10144453.Models.Domain;

namespace PROG7311_Part2_ST10144453.Controllers
{
    public class UserController : Controller
    {
        private readonly Part2DbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(Part2DbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user, IFormFile? profilePhotoUpload)
        {
            if (profilePhotoUpload != null && profilePhotoUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(profilePhotoUpload.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

                if (allowedExtensions.Contains(fileExtension))
                {
                    try
                    {
                        using (var ms = new MemoryStream())
                        {
                            await profilePhotoUpload.CopyToAsync(ms);
                            user.ProfilePhoto = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "File upload failed: " + ex.Message);
                        LogModelStateErrors();
                        return View(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("profilePhotoUpload", "Invalid file type. Please choose a JPG, JPEG, or PNG file.");
                }
            }
            else if (string.IsNullOrEmpty(user.ProfilePhoto))
            {
                SetDefaultProfilePhoto(user);
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid");
                LogModelStateErrors();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Database save failed: " + ex.Message);
                    LogModelStateErrors();
                    return View(user);
                }

                var userId = user.UserId;
                TempData["UserId"] = userId;

                if (user.AccountType == "Farmer")
                {
                    return RedirectToAction("AddNewFarmer", new { userId });
                }
                else if (user.AccountType == "Staff Member")
                {
                    var staffId = Guid.NewGuid();
                    var staff = new Staff
                    {
                        StaffId = staffId,
                        UserId = userId
                    };
                    _context.Staffs.Add(staff);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    return RedirectToAction("SomeOtherAction"); // Define an action for other account types if needed
                }
            }
            else
            {
                LogModelStateErrors();
                return View(user);
            }
        }

        private void LogModelStateErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

        private void SetDefaultProfilePhoto(User user)
        {
            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Assets", "defaultPfp.png");
            if (System.IO.File.Exists(defaultImagePath))
            {
                var defaultImage = System.IO.File.ReadAllBytes(defaultImagePath);
                user.ProfilePhoto = Convert.ToBase64String(defaultImage);
            }
        }

        [HttpGet]
        public IActionResult AddNewFarmer(Guid userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewFarmer(Farmer farmer)
        {
            if (TempData["UserId"] != null)
            {
                farmer.UserID = (Guid)TempData["UserId"];
                farmer.FarmerId = Guid.NewGuid();
            }

            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                LogModelStateErrors();
                return View(farmer);
            }
        }
    }
}
