using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.API.Dtos.Categories
{
	public class CategoryCreateDto
	{
		[Required(ErrorMessage = "El nombre de la Categoria es requerida.")]
		public string Name { get; set; }
	}
}
