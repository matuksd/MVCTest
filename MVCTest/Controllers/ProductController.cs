using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTest.Data;
using MVCTest.Helpers;
using MVCTest.Models;
using MVCTest.Models.Enums;
using System.Security.Claims;

namespace MVCTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ProductController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var vat = _config.GetValue<decimal>("VAT");
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var model = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Quantity = p.Quantity,
                Price = p.Price,
                TotalPriceWithVAT = VATCalculator.CalculateTotalWithVAT(p.Quantity, p.Price, vat)
            }).ToList();

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            LogProductChange(model, ProductChangeType.Create);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            var vat = _config.GetValue<decimal>("VAT");

            var model = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price,
                TotalPriceWithVAT = VATCalculator.CalculateTotalWithVAT(product.Quantity, product.Price, vat)
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);
            if (product == null)
            {
                return NotFound();
            }

            var oldProduct = new Product
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                Price = product.Price
            };

            product.Title = model.Title;
            product.Quantity = model.Quantity;
            product.Price = model.Price;

            _context.SaveChanges();

            LogProductChange(oldProduct, ProductChangeType.Edit);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            LogProductChange(product, ProductChangeType.Delete);

            return RedirectToAction("Index");
        }

        private void LogProductChange(Product product, ProductChangeType type)
        {
            var username = User.Identity.Name;
            string changes = string.Empty;

            switch (type)
            {
                case ProductChangeType.Create:
                    changes = $"Title: '' → '{product.Title}'; Quantity: 0 → {product.Quantity}; Price: 0 → {product.Price}";
                    break;

                case ProductChangeType.Edit:
                    var edits = new List<string>();
                    var dbProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);
                    if (dbProduct != null)
                    {
                        if (dbProduct.Title != product.Title)
                            edits.Add($"Title: '{dbProduct.Title}' → '{product.Title}'");
                        if (dbProduct.Quantity != product.Quantity)
                            edits.Add($"Quantity: {dbProduct.Quantity} → {product.Quantity}");
                        if (dbProduct.Price != product.Price)
                            edits.Add($"Price: {dbProduct.Price} → {product.Price}");
                    }
                    changes = string.Join("; ", edits);
                    break;

                case ProductChangeType.Delete:
                    changes = $"Deleted product with Title: '{product.Title}', Quantity: {product.Quantity}, Price: {product.Price}";
                    break;
            }

            var audit = new ProductAudit
            {
                ProductId = product.Id,
                Price = product.Price,
                ChangeType = type,
                ChangedBy = username,
                ChangedAt = DateTime.UtcNow,
                Changes = changes
            };

            _context.ProductAudits.Add(audit);
            _context.SaveChanges();
        }
    }
}