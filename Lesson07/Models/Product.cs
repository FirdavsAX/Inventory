namespace Lesson07.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime ExpireDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public virtual ICollection<SupplyProduct> SupplyProducts { get; set; }
    public virtual ICollection<SaleProduct> SaleProducts { get; set; }

    public Product()
    {
        SupplyProducts = new List<SupplyProduct>();
    }
}
