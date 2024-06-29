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
		public readonly string _JSON_FILE_CATEGORIES;

		public ProductsService()
		{
			_JSON_FILE = "SeedData/products.json";
			_JSON_FILE_CATEGORIES = "SeedData/categories.json";
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

		public async Task<List<CategoryDto>> ReadCategoryFromFileAsync() // Leer las categorias
		{
			if (!File.Exists(_JSON_FILE_CATEGORIES))
			{
				return new List<CategoryDto>();
			}

			var json = await File.ReadAllTextAsync(_JSON_FILE_CATEGORIES);
			var categories = JsonConvert.DeserializeObject<List<CategoryEntity>>(json);

			// Convertir la lista de CategoryEntity a una lista de CategoryDto
			var dtos = categories.Select(c => new CategoryDto
			{
				Id = c.Id,
				Name = c.Name,
				InventoryValue = c.InventoryValue,
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

		public async Task<bool> CreateAsync(ProductCreateDto dto)
		{
			var productsDtos = await ReadProductsFromFileAsync();

			var productDto = new ProductDto
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
				Price = dto.Price,
				Amount = dto.Amount,
				Category = dto.Category,
			};

			// Validar si la categoria del producto ingresado existe
			var categoriesDtos = await ReadCategoryFromFileAsync();
			var productCategory = categoriesDtos.FirstOrDefault(c => c.Name == productDto.Category);
			if (productCategory is null)
			{
				return false;
			}

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

			// Validar si la categoria del producto ingresado existe
			var categoriesDtos = await ReadCategoryFromFileAsync();
			var productCategory = categoriesDtos.FirstOrDefault(c => c.Name == dto.Category);
			if (productCategory is null)
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
