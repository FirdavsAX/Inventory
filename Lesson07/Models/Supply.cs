namespace Lesson07.Models;

public class Supply
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalDue { get; set; }
    public decimal TotalPaid { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }

    public virtual ICollection<SupplyProduct> SupplyProducts { get; set; }
}
