using Azure.Data.Tables;
using ABCRetail.Models;

namespace ABCRetail.Services
{
    public class TableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;

        public TableStorageService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureStorage:ConnectionString"];
            _tableServiceClient = new TableServiceClient(connectionString);
        }

        // CUSTOMERS 
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var tableClient = _tableServiceClient.GetTableClient("Customers");
            await tableClient.CreateIfNotExistsAsync();

            var customers = new List<Customer>();
            await foreach (var customer in tableClient.QueryAsync<Customer>())
            {
                customers.Add(customer);
            }
            return customers;
        }

        public async Task<Customer?> GetCustomerAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient("Customers");
            await tableClient.CreateIfNotExistsAsync();

            var response = await tableClient.GetEntityIfExistsAsync<Customer>(partitionKey, rowKey);
            return response.HasValue ? response.Value : null;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            var tableClient = _tableServiceClient.GetTableClient("Customers");
            await tableClient.CreateIfNotExistsAsync();

            customer.PartitionKey = "Customer";
            customer.RowKey = Guid.NewGuid().ToString();

            await tableClient.AddEntityAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            var tableClient = _tableServiceClient.GetTableClient("Customers");
            await tableClient.UpdateEntityAsync(customer, customer.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient("Customers");
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // PRODUCTS
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            await tableClient.CreateIfNotExistsAsync();

            var products = new List<Product>();
            await foreach (var product in tableClient.QueryAsync<Product>())
            {
                products.Add(product);
            }
            return products;
        }

        public async Task<Product?> GetProductAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            await tableClient.CreateIfNotExistsAsync();

            var response = await tableClient.GetEntityIfExistsAsync<Product>(partitionKey, rowKey);
            return response.HasValue ? response.Value : null;
        }

        public async Task AddProductAsync(Product product)
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            await tableClient.CreateIfNotExistsAsync();

            product.PartitionKey = "Product";
            product.RowKey = Guid.NewGuid().ToString();

            await tableClient.AddEntityAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            await tableClient.UpdateEntityAsync(product, product.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteProductAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient("Products");
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}