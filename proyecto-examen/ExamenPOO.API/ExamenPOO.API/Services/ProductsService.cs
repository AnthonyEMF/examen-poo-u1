using ExamenPOO.API.Database.Entities;
using ExamenPOO.API.Dtos.Categories;
using ExamenPOO.API.Dtos.Products;
using ExamenPOO.API.Services.Interfaces;
using Newtonsoft.Json;

namespace ExamenPOO.API.Services
{
	public class ProductsService : IProductsService
	{
		// **** Manejo del archivo JSON ****

		public readonly string _JSON_FILE;
		//public readonly string _JSON_FILE_CATEGORIES;

		public ProductsService()
		{
			_JSON_FILE = "SeedData/products.json";
			//_JSON_FILE_CATEGORIES = "SeedData/categories.json";
		}

		// Leer el archivo JSON
		private async Task<List<ProductDto>> ReadProductsFromFileAsync()
		{
			if (!File.Exists(_JSON_FILE))
			{
				return new List<ProductDto>();
			}

			var json = await File.ReadAllTextAsync(_JSON_FILE);
			var products = JsonConvert.DeserializeObject<List<ProductEntity>>(json);

			// Convertir la lista de ProductEntity a una lista de ProductDto
			var dtos = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Amount = p.Amount,
				Category = p.Category,
			}).ToList();

			return dtos;
		}

		// Actualizar archivo JSON
		private async Task WriteProductsToFileAsync(List<ProductEntity> product)
		{
			var json = JsonConvert.SerializeObject(product, Formatting.Indented);

			if (File.Exists(_JSON_FILE))
			{
				await File.WriteAllTextAsync(_JSON_FILE, json);
			}
		}

		// **** Metodos del CRUD ****

		public async Task<List<ProductDto>> GetProductsAsync()
		{
			return await ReadProductsFromFileAsync();
		}

		public async Task<ProductDto> GetProductByIdAsync(Guid id)
		{
			var products = await ReadProductsFromFileAsync();
			return products.FirstOrDefault(c => c.Id == id);
		}

		public async Task<bool> CreateAsync(ProductCreateDto dto/*, CategoryDto categoryDto*/)
		{
			var productsDtos = await ReadProductsFromFileAsync();

			// Validar que la categoria ingresada sea una categoria existente
			//var categoriesDtos = await ReadCategoriesFromFileAsync();

			//var existingCategory = categoriesDtos.FirstOrDefault(c => c.Name == categoryDto.Name);
			//if (existingCategory is null)
			//{
			//	return false;
			//}

			var productDto = new ProductDto
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
				Price = dto.Price,
				Amount = dto.Amount,
				Category = dto.Category,
			};

			productsDtos.Add(productDto);

			// Pasar de ProductDto a ProductEntity
			var products = productsDtos.Select(x => new ProductEntity
			{
				Id = x.Id,
				Name = x.Name,
				Price = x.Price,
				Amount = x.Amount,
				Category = x.Category,
			}).ToList();

			await WriteProductsToFileAsync(products);
			return true;
		}

		public async Task<bool> EditAsync(ProductEditDto dto, Guid id)
		{
			var productsDtos = await ReadProductsFromFileAsync();

			var existingProduct = productsDtos.FirstOrDefault(p => p.Id == id);
			if (existingProduct is null)
			{
				return false;
			}

			for (int i=0; i < productsDtos.Count; i++)
			{
				if (productsDtos[i].Id == id)
				{
					productsDtos[i].Name = dto.Name;
					productsDtos[i].Price = dto.Price;
					productsDtos[i].Amount = dto.Amount;
					productsDtos[i].Category = dto.Category;
				}
			}

			// Pasar de ProductDto a ProductEntity
			var products = productsDtos.Select(x => new ProductEntity
			{
				Id = x.Id,
				Name = x.Name,
				Price = x.Price,
				Amount = x.Amount,
				Category = x.Category,
			}).ToList();

			await WriteProductsToFileAsync(products);
			return true;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var productsDtos = await ReadProductsFromFileAsync();
			var productToDelete = productsDtos.FirstOrDefault(p => p.Id == id);

			if (productToDelete is null)
			{
				return false;
			}

			productsDtos.Remove(productToDelete);

			// Pasar de ProductDto a ProductEntity
			var products = productsDtos.Select(x => new ProductEntity
			{
				Id = x.Id,
				Name = x.Name,
				Price = x.Price,
				Amount = x.Amount,
				Category = x.Category,
			}).ToList();

			await WriteProductsToFileAsync(products);
			return true;
		}
	}
}
