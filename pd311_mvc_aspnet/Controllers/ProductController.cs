using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pd311_mvc_aspnet.Data;
using pd311_mvc_aspnet.Models;

using Microsoft.EntityFrameworkCore;

namespace pd311_mvc_aspnet.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile ImageFile)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                model.Image = "/images/" + uniqueFileName;
            }

            model.Id = Guid.NewGuid().ToString();
            _context.Products.Add(model);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Edit(string id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == model.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

        
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                existingProduct.Image = "/images/" + uniqueFileName;
            }

           
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.Amount = model.Amount;
            existingProduct.CategoryId = model.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

