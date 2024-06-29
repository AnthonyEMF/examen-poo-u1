using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.API.Database.Entities
{
	public class ProductEntity : BaseEntity
	{
        [Required(ErrorMessage = "El precio del producto es requerido.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "La cantidad del producto es requerida.")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "La categoria del producto es requerida.")]
        public string Category { get; set; }
    }
}
