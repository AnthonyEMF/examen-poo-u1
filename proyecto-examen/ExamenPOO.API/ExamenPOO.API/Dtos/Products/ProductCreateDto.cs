using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.API.Dtos.Products
{
	public class ProductCreateDto
	{
		[Required(ErrorMessage = "El nombre del Producto es requerido.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "El precio del Producto es requerido.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "La cantidad del Producto es requerida.")]
        public int Amount { get; set; }

		[Required(ErrorMessage = "La categoria del Producto es requerida.")]
        public string Category { get; set; }
    }
}
