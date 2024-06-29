using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.API.Database.Entities
{
	public class BaseEntity
	{
        public Guid Id { get; set; }

		[Required(ErrorMessage = "El campo Name es obligatorio.")]
		public string Name { get; set; }
	}
}
