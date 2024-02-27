using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson07.Models;

public class Sale
{
    public int Id { get; set; }
    public decimal TotalDue { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalDiscount { get; set; }
    public DateTime SaleDate { get; set; }

    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    public virtual ICollection<SaleProduct> SaleProducts { get; set; }
}
