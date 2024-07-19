namespace BasicShopApis.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Visible { get; set; } = false;

    }
}
