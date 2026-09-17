using Microsoft.AspNetCore.Mvc;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class InventoryController : Controller
    {
        private readonly QueueStorageService _queueService;

        public InventoryController(QueueStorageService queueService)
        {
            _queueService = queueService;
        }

        // GET: /Inventory
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Inventory
        [HttpPost]
        public async Task<IActionResult> Index(string productName, int quantity, string action)
        {
            if (string.IsNullOrWhiteSpace(productName) || quantity <= 0)
            {
                ViewBag.Error = "Please enter a valid product name and quantity.";
                return View();
            }

            await _queueService.SendInventoryMessageAsync(productName, quantity, action);
            ViewBag.Message = $"Inventory message sent successfully: {action} - {productName} (Qty: {quantity})";
            return View();
        }
    }
}