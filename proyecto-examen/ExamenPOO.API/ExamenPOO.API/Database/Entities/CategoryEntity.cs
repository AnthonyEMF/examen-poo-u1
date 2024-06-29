namespace ExamenPOO.API.Database.Entities
{
	public class CategoryEntity : BaseEntity
	{
        // Heredamos las propiedades Id y Name del BaseEntity

        public int InventoryValue { get; set; } // Valor total del inventario (Productos * Precio)
    }
}
