//-------00000000000000000000oooooooooooooooooooo..........Start of File..........oooooooooooooooooooo00000000000000000000------//
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
    //------------------------------....................UserController Class....................------------------------------//
    public class UserController : Controller
    {
        //oooooooooo............Declarations............oooooooooo//
        private readonly Part2DbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //............................................Constructor()............................................//
        /// <summary>
        /// The constructor for the UserController class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webHostEnvironment"></param>
        public UserController(Part2DbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //............................................Add()............................................//
        /// <summary>
        /// The Add action method for the UserController class.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        //............................................Add()............................................//
        /// <summary>
        /// The Add action method for the UserController class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="profilePhotoUpload"></param>
        /// <returns></returns>
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

        //............................................LogModelStateErrors()............................................//
        /// <summary>
        /// The LogModelStateErrors method for the UserController class.
        /// </summary>
        private void LogModelStateErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

        //............................................SetDefaultProfilePhoto()............................................//
        /// <summary>
        /// The SetDefaultProfilePhoto method for the UserController class.
        /// </summary>
        /// <param name="user"></param>
        private void SetDefaultProfilePhoto(User user)
        {
            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Assets", "defaultPfp.png");
            if (System.IO.File.Exists(defaultImagePath))
            {
                var defaultImage = System.IO.File.ReadAllBytes(defaultImagePath);
                user.ProfilePhoto = Convert.ToBase64String(defaultImage);
            }
        }

        //............................................AddNewFarmer()............................................//
        /// <summary>
        /// The AddNewFarmer action method for the UserController class.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddNewFarmer(Guid userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        //............................................AddNewFarmer()............................................//
        /// <summary>
        /// The AddNewFarmer action method for the UserController class.
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
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
//-------00000000000000000000oooooooooooooooooooo..........End of File..........oooooooooooooooooooo00000000000000000000------//
