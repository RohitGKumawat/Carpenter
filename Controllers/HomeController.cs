using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Carpenter.Models;
using Microsoft.EntityFrameworkCore;
using Carpenter.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace Carpenter.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly EverytDB _context;

    private readonly PdfService _pdfService;

    private readonly EmailService _emailService;

    public HomeController(ILogger<HomeController> logger, EverytDB context, PdfService pdfService, EmailService emailService) // Inject Both
    {
        _logger = logger;
        _context = context;
        _pdfService = pdfService;
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.UserProfiles
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                // Store user info in session or use authentication system
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetString("UserPhoto", user.Photo ?? "/Media/Images/UserProfile/defaultuser.png");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
        }

        return View(model);
    }

    


    // GET: User/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(UserProfile model)
    {
        if (ModelState.IsValid)
        {
            // Check if user already exists
            if (_context.UserProfiles.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            // Save user (note: password is stored as plain text — fix below!)
            _context.UserProfiles.Add(model);
            _context.SaveChanges();

            // Optional: Log them in immediately
            HttpContext.Session.SetString("UserEmail", model.Email);

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }


    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            ViewBag.Error = "Email not registered.";
            return View();
        }

        // Generate OTP
        var otp = new Random().Next(100000, 999999).ToString();
        user.OTP = otp;
        user.OTPExpiry = DateTime.Now.AddMinutes(10);
        await _context.SaveChangesAsync();

        // Send OTP via email (replace with real service)
        await _emailService.SendEmailAsync(user.Email, "OTP Code", $"Your OTP is: <b>{otp}</b>");


        TempData["Email"] = email;
        return RedirectToAction("VerifyOtp");
    }

    public IActionResult VerifyOtp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> VerifyOtp(string otp)
    {
        var email = TempData["Email"]?.ToString();
        var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || user.OTP != otp || user.OTPExpiry < DateTime.Now)
        {
            ViewBag.Error = "Invalid or expired OTP.";
            return View();
        }

        TempData["ResetEmail"] = email;
        return RedirectToAction("ResetPassword");
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string newPassword)
    {
        var email = TempData["ResetEmail"]?.ToString();
        var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return NotFound();

        user.Password = newPassword;
        user.OTP = null;
        user.OTPExpiry = null;
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }


    [HttpGet]
    public IActionResult MyAccount()
    {
        
        var email = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login");
        }

        var user = _context.UserProfiles.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> MyAccount(UserProfile updatedProfile, IFormFile ProfileImage)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Home");

        var user = _context.UserProfiles.FirstOrDefault(u => u.Id == userId);
        if (user == null) return NotFound();

        // Handle profile photo upload
        if (ProfileImage != null && ProfileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Media/Images/UserProfile");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(stream);
            }

            user.Photo = $"/Media/Images/UserProfile/{uniqueFileName}";
        }

        // Update other fields
        user.FirstName = updatedProfile.FirstName;
        user.LastName = updatedProfile.LastName;
        user.PhoneNumber = updatedProfile.PhoneNumber;
        user.Country = updatedProfile.Country;

        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
        HttpContext.Session.SetString("UserPhoto", user.Photo ?? "/Media/Images/UserProfile/defaultuser.png");

        ViewBag.Message = "Profile updated successfully!";
        return View(user);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session data
        return RedirectToAction("Login", "Home"); // Redirect to User/Login or any login page
    }

    public IActionResult MySubscription()
    {
        return View();
    }
    
    

    public IActionResult AddNewProject()
    {
        return View();
    }

    public IActionResult AddNewItem()
    {
        return View();
    }

    public IActionResult Item()
    {
        return View();
    }

    public IActionResult EstimatedMaterials()
    {
        return View();
    }

    public IActionResult ScanFromPDF()
    {
        return View();
    }

    public IActionResult Dimentions()
    {
        return View();
    }

    public IActionResult SelectProducts()
    {
        return View();
    }

    public IActionResult HowToBuild(int itemId)
    {
        
    
        var item = _context.ProjectItems.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return NotFound();

        return View(item);
    }
        

    public IActionResult DetailHowtobuild()
    {
        return View();
    }

    public IActionResult AllProducts()
    {
        var products = _context.AllProductss.ToList();  // Fetch products from database
        return View(products);
    }


    public IActionResult SingleProduct()
    {
        return View();
    }
    public IActionResult ThankYou()
    {
        return View();
    }

    public IActionResult DownloadReceipt()
    {
        // Sample static values — in a real app, pull these from the database or session
        var userName = "Rohit Kumawat";
        var plan = "Pro Monthly";
        var subscriptionId = "SUB123456";

        var pdfBytes = _pdfService.GenerateReceiptPdf(userName, plan, subscriptionId);
        return File(pdfBytes, "application/pdf", "SubscriptionReceipt.pdf");
    }

    public IActionResult TermAndCondition()
    {
        return View();
    }
    public IActionResult CostumerCare()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult RefundPolicy()
    {
        return View();
    }

    public IActionResult CalculationPolicy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
