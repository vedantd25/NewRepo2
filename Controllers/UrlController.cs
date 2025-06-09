using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize] // Require login for all actions in this controller
    public class UrlController : Controller
    {
        private readonly AppDbContext _context;

        public UrlController(AppDbContext context)
        {
            _context = context;
        }

        // Show input form to shorten URL
        public IActionResult Index()
        {
            return View();
        }

        // Handles POST request to shorten a given URL
        [HttpPost]
        public IActionResult Shorten(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return RedirectToAction("Index");

            // Check if the same original URL is already stored
            var existing = _context.UrlMappings.FirstOrDefault(u => u.OriginalUrl == originalUrl);
            if (existing != null)
            {
                ViewBag.ShortUrl = $"{Request.Scheme}://{Request.Host}/Url/Go/{existing.ShortCode}";
                return View("Index");
            }

            // Generate new short code
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

        // Public redirection using short URL
        [AllowAnonymous]
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
