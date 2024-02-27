namespace Lesson07.Models;

public class SaleProduct
{
    public int Id { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }

    public int  SaleId  { get; set; }
    public virtual Sale Sale { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}
