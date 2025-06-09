using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class UrlController : Controller
    {
        private readonly AppDbContext _context;

        public UrlController(AppDbContext context)
        {
            _context = context;
        }

        // Load the input form
        public IActionResult Index()
        {
            return View();
        }

        // Handles POST request to shorten the URL
        [HttpPost]
        public IActionResult Shorten(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl)) return RedirectToAction("Index");

            // ✅ Check if URL already exists
            var existing = _context.UrlMappings.FirstOrDefault(u => u.OriginalUrl == originalUrl);
            if (existing != null)
            {
                ViewBag.ShortUrl = $"{Request.Scheme}://{Request.Host}/Url/Go/{existing.ShortCode}";
                return View("Index");
            }

            // ❌ Otherwise, generate a new code
            string code = Guid.NewGuid().ToString("n").Substring(0, 6);

            var mapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortCode = code
            };

            _context.UrlMappings.Add(mapping);
            _context.SaveChanges();

            ViewBag.ShortUrl = $"{Request.Scheme}://{Request.Host}/Url/Go/{code}";
            return View("Index");
        }


        // Redirects to original URL based on short code
        [HttpGet("/Url/Go/{code}")]
        public IActionResult Go(string code)
        {
            var mapping = _context.UrlMappings.FirstOrDefault(u => u.ShortCode == code);
            if (mapping == null)
                return NotFound("Short URL not found.");

            return Redirect(mapping.OriginalUrl);
        }
    }
}
