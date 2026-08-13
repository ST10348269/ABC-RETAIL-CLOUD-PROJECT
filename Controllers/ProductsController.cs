using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageService _tableService;
        private readonly BlobStorageService _blobService;

        public ProductsController(TableStorageService tableService, BlobStorageService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        // GET: /Products
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetAllProductsAsync();
            return View(products);
        }

        // GET: /Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    product.ImageUrl = await _blobService.UploadImageAsync(imageFile);
                }

                await _tableService.AddProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Products/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var product = await _tableService.GetProductAsync(partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Products/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // If a new image is uploaded, replace the old one
                if (imageFile != null && imageFile.Length > 0)
                {
                    product.ImageUrl = await _blobService.UploadImageAsync(imageFile);
                }

                await _tableService.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Products/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var product = await _tableService.GetProductAsync(partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: /Products/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableService.DeleteProductAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}