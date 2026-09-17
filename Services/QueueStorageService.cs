using Azure.Storage.Queues;
using System.Text.Json;

namespace ABCRetail.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureStorage:ConnectionString"];
            _queueClient = new QueueClient(connectionString, "orders");
            _queueClient.CreateIfNotExists();
        }

        // Order processing message
        public async Task SendMessageAsync(string customerName, string productName, int quantity)
        {
            var orderMessage = new
            {
                Action = "Processing order",
                CustomerName = customerName,
                ProductName = productName,
                Quantity = quantity,
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string message = JsonSerializer.Serialize(orderMessage);
            await _queueClient.SendMessageAsync(message);
        }

        // Inventory processing message
        public async Task SendInventoryMessageAsync(string productName, int quantity, string action)
        {
            var inventoryMessage = new
            {
                Action = action,
                ProductName = productName,
                Quantity = quantity,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string message = JsonSerializer.Serialize(inventoryMessage);
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<List<string>> PeekMessagesAsync()
        {
            var messages = new List<string>();
            var peeked = await _queueClient.PeekMessagesAsync(maxMessages: 15);

            foreach (var msg in peeked.Value)
            {
                messages.Add(msg.MessageText);
            }

            return messages;
        }
    }
}