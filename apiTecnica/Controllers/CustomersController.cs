
using Microsoft.AspNetCore.Mvc;
using apiTecnica.Models;

[ApiController]
[Route("/[controller]")]
public class CustomersController : ControllerBase
{
	private readonly PruebaTecnicaContext _context;

	public CustomersController(PruebaTecnicaContext context)
	{
		_context = context;
	}

	public class CreateCustomerDto
	{
		public string Name { get; set; } = string.Empty;
		public string CustomerType { get; set; } = string.Empty;
		public int CreditTermDays { get; set; }
	}

	[HttpPost]
	public IActionResult CreateCustomer([FromBody] CreateCustomerDto dto)
	{
		// Buscar el tipo de cliente por nombre
		var customerType = _context.CustomerTypes.FirstOrDefault(ct => ct.Name == dto.CustomerType);
		if (customerType == null)
		{
			return BadRequest($"CustomerType '{dto.CustomerType}' not found.");
		}

		// Buscar el término de crédito por días
		var creditTerm = _context.CreditTerms.FirstOrDefault(ct => ct.Days == dto.CreditTermDays);
		if (creditTerm == null)
		{
			return BadRequest($"CreditTerm with days '{dto.CreditTermDays}' not found.");
		}

		var customer = new Customer
        {
            Name = dto.Name,
            CustomerTypeId = customerType.CustomerTypeId,
            CreditTermsId = creditTerm.CreditTermsId,
            CreatedAt = DateTime.UtcNow,
            CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
		};

		_context.Customers.Add(customer);
		_context.SaveChanges();

		return CreatedAtAction(nameof(CreateCustomer), new { id = customer.CustomerId }, new {
			id = customer.CustomerId,
			name = customer.Name,
			customer_type = customerType.Name,
			credit_term_days = creditTerm.Days
		});
	}
	[HttpGet]
	public IActionResult GetCustomers()
	{
		var customers = _context.Customers
			.Where(c => c.DeletedAt == null)
			.Select(c => new {
				id = c.CustomerId,
				name = c.Name,
				customer_type = c.CustomerType.Name,
				credit_term_days = c.CreditTerms.Days
			})
			.ToList();

		return Ok(customers);
	}
}
