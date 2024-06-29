using ExamenPOO.API.Dtos.Categories;

namespace ExamenPOO.API.Services.Interfaces
{
	public interface ICategoriesService
	{
		Task<List<CategoryDto>> GetCategoriesAsync();
		Task<CategoryDto> GetCategoryByIdAsync(Guid id);
		Task<bool> CreateAsync(CategoryCreateDto dto);
		Task<bool> EditAsync(CategoryEditDto dto, Guid id);
		Task<bool> DeleteAsync(Guid id);
	}
}
