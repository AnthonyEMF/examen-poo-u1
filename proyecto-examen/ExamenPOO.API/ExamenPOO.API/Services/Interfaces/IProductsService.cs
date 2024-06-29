using ExamenPOO.API.Dtos.Categories;
using ExamenPOO.API.Dtos.Products;

namespace ExamenPOO.API.Services.Interfaces
{
	public interface IProductsService
	{
		Task<List<ProductDto>> GetProductsAsync();
		Task<ProductDto> GetProductByIdAsync(Guid id);
		Task<bool> CreateAsync(ProductCreateDto dto/*, CategoryDto categoryDto*/);
		Task<bool> EditAsync(ProductEditDto dto, Guid id);
		Task<bool> DeleteAsync(Guid id);
	}
}
