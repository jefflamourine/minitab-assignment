using Microsoft.AspNetCore.Mvc;

namespace Minitab_Assignment.Controllers
{
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IAddressValidator _addressValidator;
        private ICrmRepository _crmRepository;

        public CustomersController([FromServices] IAddressValidator addressValidator, [FromServices] ICrmRepository crmRepository)
        {
            _addressValidator = addressValidator;
            _crmRepository = crmRepository;
        }

        [HttpPost]
        [Route("[controller]")]
        public IActionResult Create([FromBody] Customer customer)
        {
            if (customer.Address != null)
            {
                if (!_addressValidator.ValidateAddress(customer.Address))
                {
                    customer.Address = null;
                }
            }

            _crmRepository.UpsertCustomer(customer);

            return new EmptyResult();
        }
    }
}
