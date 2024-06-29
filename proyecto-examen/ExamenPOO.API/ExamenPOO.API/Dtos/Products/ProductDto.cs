namespace ExamenPOO.API.Dtos.Products
{
	public class ProductDto
	{
        public Guid Id { get; set; }
		public string Name { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
    }
}
