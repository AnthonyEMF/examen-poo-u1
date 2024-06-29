using ExamenPOO.API.Database.Entities;
using ExamenPOO.API.Dtos.Categories;
using ExamenPOO.API.Dtos.Products;
using ExamenPOO.API.Services.Interfaces;
using Newtonsoft.Json;

namespace ExamenPOO.API.Services
{
	public class CategoriesService : ICategoriesService
	{
		// **** Manejo del archivo JSON ****

		public readonly string _JSON_FILE;
		//public readonly string _JSON_FILE_PRODUCTS;

		public CategoriesService()
		{
			_JSON_FILE = "SeedData/categories.json";
			//_JSON_FILE_PRODUCTS = "SeedData/products.json";
		}

		// Leer el archivo JSON
		public async Task<List<CategoryDto>> ReadCategoriesFromFileAsync()
		{
			if (!File.Exists(_JSON_FILE))
			{
				return new List<CategoryDto>();
			}

			var json = await File.ReadAllTextAsync(_JSON_FILE);
			var categories = JsonConvert.DeserializeObject<List<CategoryEntity>>(json);

			// Convertir la lista de CategoryEntity a una lista de CategoryDto
			var dtos = categories.Select(c => new CategoryDto
			{
				Id = c.Id,
				Name = c.Name,
			}).ToList();

			return dtos;
		}

		// Actualizar archivo JSON
		private async Task WriteCategoriesToFileAsync(List<CategoryEntity> category)
		{
			var json = JsonConvert.SerializeObject(category, Formatting.Indented);

			if (File.Exists(_JSON_FILE))
			{
				await File.WriteAllTextAsync(_JSON_FILE, json);
			}
		}

		// **** Metodos del CRUD ****

		// Calcular el valor de inventario (InventoryValue)
		private void InventoryValue(ProductDto productDto)
		{
			// Logica para calcular el inventario
		}

		public async Task<List<CategoryDto>> GetCategoriesAsync()
		{
			return await ReadCategoriesFromFileAsync();
		}

		public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
		{
			var categories = await ReadCategoriesFromFileAsync();
			return categories.FirstOrDefault(c => c.Id == id);
		}

		public async Task<bool> CreateAsync(CategoryCreateDto dto)
		{
			var categoriesDtos = await ReadCategoriesFromFileAsync();

			var categoryDto = new CategoryDto
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
			};

			categoriesDtos.Add(categoryDto);

			// Pasar de CategoryDto a CategoryEntity
			var categories = categoriesDtos.Select(x => new CategoryEntity
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();

			await WriteCategoriesToFileAsync(categories);
			return true;
		}

		public async Task<bool> EditAsync(CategoryEditDto dto, Guid id)
		{
			var categoriesDtos = await ReadCategoriesFromFileAsync();

			var existingCategory = categoriesDtos.FirstOrDefault(c => c.Id == id);
			if (existingCategory is null)
			{
				return false;
			}

			for (int i=0; i < categoriesDtos.Count; i++)
			{
				if (categoriesDtos[i].Id == id)
				{
					categoriesDtos[i].Name = dto.Name;
				}
			}

			// Pasar de CategoryDto a CategoryEntity
			var categories = categoriesDtos.Select(x => new CategoryEntity
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();

			await WriteCategoriesToFileAsync(categories);
			return true;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var categoriesDtos = await ReadCategoriesFromFileAsync();
			var categoryToDelete = categoriesDtos.FirstOrDefault(c => c.Id == id);

			if (categoryToDelete is null)
			{
				return false;
			}

			categoriesDtos.Remove(categoryToDelete);

			// Pasar de CategoryDto a CategoryEntity
			var categories = categoriesDtos.Select(x => new CategoryEntity
			{
				Id = x.Id,
				Name = x.Name,
			}).ToList();

			await WriteCategoriesToFileAsync(categories);
			return true;
		}
	}
}
