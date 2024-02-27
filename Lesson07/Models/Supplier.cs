namespace Lesson07.Models;

public class Supplier
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public string PhoneNumber { get; set; }

    public virtual ICollection<Supply> Supplies { get; set; }
}
