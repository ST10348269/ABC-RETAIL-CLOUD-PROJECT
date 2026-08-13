using Microsoft.AspNetCore.Mvc;
using ABCRetail.Services;

namespace ABCRetail.Controllers
{
    public class LogsController : Controller
    {
        private readonly FileStorageService _fileService;

        public LogsController(FileStorageService fileService)
        {
            _fileService = fileService;
        }

        // GET: /Logs/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Logs/Create
        [HttpPost]
        public async Task<IActionResult> Create(string logMessage)
        {
            if (string.IsNullOrWhiteSpace(logMessage))
            {
                ViewBag.Error = "Please enter a log message.";
                return View();
            }

            await _fileService.WriteLogAsync(logMessage);
            ViewBag.Message = "Log saved successfully to Azure Files!";
            return View();
        }

        // GET: /Logs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var files = await _fileService.GetLogFilesAsync();
            return View(files);
        }
    }
}