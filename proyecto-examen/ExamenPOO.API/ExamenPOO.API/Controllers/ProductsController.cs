using ExamenPOO.API.Dtos.Categories;
using ExamenPOO.API.Dtos.Products;
using ExamenPOO.API.Services;
using ExamenPOO.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamenPOO.API.Controllers
{
	[ApiController]
	[Route("api/products")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductsService _productsService;

		public ProductsController(IProductsService productsService)
		{
			this._productsService = productsService;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			return Ok(await _productsService.GetProductsAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> Get(Guid id)
		{
			var product = await _productsService.GetProductByIdAsync(id);

			if (product is null)
			{
				return NotFound(new { Message = "No se encontro el producto que busca." });
			}

			return Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult> Create(ProductCreateDto dto)
		{
			await _productsService.CreateAsync(dto);
			return StatusCode(201);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Edit(ProductEditDto dto, Guid id)
		{
			var result = await _productsService.EditAsync(dto, id);

			if (!result)
			{
				return NotFound();
			}

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var product = await _productsService.GetProductByIdAsync(id);

			if (product is null)
			{
				return NotFound();
			}

			await _productsService.DeleteAsync(id);
			return Ok();
		}
	}
}
