using Microsoft.AspNetCore.Mvc;
using apiTecnica.Models;

namespace apiTecnica.Controllers
{
	[ApiController]
	[Route("/products")]
	public class ProductsController : ControllerBase
	{
		[HttpPost]
		public IActionResult CreateProduct([FromBody] ProductCreateDto dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.ProductType))
			{
				return BadRequest("Name and ProductType are required.");
			}
            if (dto.ListPrice <= 0)
            {
                return BadRequest("Price can't be equal or less than 0");
            }
			using (var context = new PruebaTecnicaContext())
            {
                var productType = context.ProductTypes.FirstOrDefault(pt => pt.Name == dto.ProductType);
                if (productType == null)
                {
                    return BadRequest("Invalid ProductType.");
                }
                var product = new Product
                {
                    Name = dto.Name!,
                    ProductTypeId = productType.ProductTypeId,
                    ListPrice = decimal.Round(dto.ListPrice, 2),
                    CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString()
                };
                context.Products.Add(product);
                context.SaveChanges();
                return CreatedAtAction(nameof(CreateProduct), new { id = product.ProductId }, new
                {
                    id = product.ProductId,
                    name = product.Name,
                    product_type = product.ProductType.Name,
                    list_price = decimal.Round(product.ListPrice, 2)
                });
            }
		}

        public class ProductCreateDto
        {
            public string? Name { get; set; }
            public string? ProductType { get; set; }
            public decimal ListPrice { get; set; }
		}
	}
}
