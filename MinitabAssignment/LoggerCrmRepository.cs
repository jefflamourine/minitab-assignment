using Microsoft.Extensions.Logging;

namespace Minitab_Assignment
{
    public class LoggerCrmRepository : ICrmRepository
    {
        private readonly ILogger<LoggerCrmRepository> _logger;

        public LoggerCrmRepository(ILogger<LoggerCrmRepository> logger)
        {
            _logger = logger;
        }

        public void UpsertCustomer(Customer customer)
        {   
            _logger.LogInformation($"Adding customer to respository: {customer}");
        }
    }
}
