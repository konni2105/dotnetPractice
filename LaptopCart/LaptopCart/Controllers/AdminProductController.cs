using LaptopCart.Data;
using LaptopCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopCart.Controllers
{
    public class AdminProductController : Controller
    {
        // Inject AppDbContext
        private readonly AppDbContext _context;

        public AdminProductController(AppDbContext context)
        {
            _context = context;
        }


       
        // INDEX - DISPLAY ALL PRODUCTS
        // GET: /AdminProduct/Index
        

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();

            return View(products);
        }


        // CREATE - SHOW FORM
        // GET: /AdminProduct/Create
  

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // CREATE - SAVE PRODUCT
        // POST: /AdminProduct/Create
       

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // Validate Name, Description, Price
            if (!ModelState.IsValid)
            {
                return View(product);
            }


            // Check whether image was selected
            if (product.ImageFile != null)
            {
                // Allowed image extensions
                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".jfif"
                };

                // Get uploaded file extension
                string extension =
                    Path.GetExtension(product.ImageFile.FileName)
                    .ToLowerInvariant();


                // Validate image extension
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "ImageFile",
                        "Only JPG, JPEG, PNG, and JFIF images are allowed."
                    );

                    return View(product);
                }


                // Create unique file name
                string fileName =
                    Guid.NewGuid().ToString() + extension;


                // Create physical file path
                string filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    fileName
                );


                // Save image into wwwroot/images
                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }


                // Store image path in database
                product.ImagePath = "/images/" + fileName;
            }


            // Add product to database
            _context.Products.Add(product);


            // Save changes
            await _context.SaveChangesAsync();


            // Go back to product list
            return RedirectToAction(nameof(Index));
        }


        // EDIT - SHOW EXISTING PRODUCT
        // GET: /AdminProduct/Edit/1
       

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        // EDIT - UPDATE PRODUCT
        // POST: /AdminProduct/Edit
       

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            // Validate Name, Description, Price
            if (!ModelState.IsValid)
            {
                return View(product);
            }


            // Find existing product
            var existingProduct =
                await _context.Products.FindAsync(product.ProductId);

            if (existingProduct == null)
            {
                return NotFound();
            }


            // Update product details
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;


            // NEW IMAGE UPLOAD
        

            if (product.ImageFile != null)
            {
                // Store old image path
                string? oldImagePath =
                    existingProduct.ImagePath;


                // Allowed image extensions
                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".jfif"
                };


                // Get new image extension
                string extension =
                    Path.GetExtension(product.ImageFile.FileName)
                    .ToLowerInvariant();


                // Validate new image
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "ImageFile",
                        "Only JPG, JPEG, PNG, and JFIF images are allowed."
                    );

                    return View(product);
                }


                // Create unique file name
                string fileName =
                    Guid.NewGuid().ToString() + extension;


                // Create physical path
                string filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    fileName
                );


                // Save new image
                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }


                // Update database with new image path
                existingProduct.ImagePath =
                    "/images/" + fileName;


                // DELETE OLD IMAGE
                
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    string oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        oldImagePath.TrimStart('/')
                    );


                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }


            // Save changes
            await _context.SaveChangesAsync();


            // Go back to product list
            return RedirectToAction(nameof(Index));
        }


        
        // DELETE - SHOW CONFIRMATION
        // GET: /AdminProduct/Delete/1
        

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


       
        // DELETE - DELETE PRODUCT
        // POST: /AdminProduct/DeleteConfirmed
        

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int ProductId)
        {
            // Find product
            var product =
                await _context.Products.FindAsync(ProductId);

            if (product == null)
            {
                return NotFound();
            }


            
            // DELETE IMAGE FROM wwwroot/images
            

            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                string imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    product.ImagePath.TrimStart('/')
                );


                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }


            // DELETE PRODUCT FROM DATABASE
            

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();


            // Go back to product list
            return RedirectToAction(nameof(Index));
        }
    }
}