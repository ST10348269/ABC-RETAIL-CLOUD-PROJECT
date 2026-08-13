using Microsoft.AspNetCore.Mvc;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QueueStorageService _queueService;

        public OrdersController(QueueStorageService queueService)
        {
            _queueService = queueService;
        }

        // GET: /Orders/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        public async Task<IActionResult> Create(string customerName, string productName, int quantity)
        {
            await _queueService.SendMessageAsync(customerName, productName, quantity);

            ViewBag.Message = "Order successfully sent to the queue!";
            return View();
        }

        // GET: /Orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _queueService.PeekMessagesAsync();
            return View(messages);
        }
    }
}