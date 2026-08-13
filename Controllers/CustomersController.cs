using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomersController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        // GET: /Customers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: /Customers/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customers/Create
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddCustomerAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Customers/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var customer = await _tableService.GetCustomerAsync(partitionKey, rowKey);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: /Customers/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _tableService.UpdateCustomerAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Customers/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var customer = await _tableService.GetCustomerAsync(partitionKey, rowKey);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: /Customers/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}