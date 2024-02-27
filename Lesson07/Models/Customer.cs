namespace Lesson07.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber  { get; set; }
    public string Address { get; set; }

    public virtual ICollection<Sale> Sales { get; set; }
}
